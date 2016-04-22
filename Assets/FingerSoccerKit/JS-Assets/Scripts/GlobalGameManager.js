#pragma strict
import System.Linq;
import System.Collections.Generic;

///*************************************************************************///
/// Main Game Controller.
/// This class controls main aspects of the game like rounds, levels, scores and ...
/// Please note that the game always happens between 2 player: (Player-1 vs Player-2) or (Player-1 vs AI)
/// Player-2 and AI are the same in some aspects like when they got their turns, but they use different controllers.
/// Player-2 uses a similar controller as Player-1, while AI uses an artificial intelligent routine to play the game.
///
/// Important! All units and ball object inside the game should be fixed at Z=-0.5 positon at all times. 
/// You can do this with RigidBody's freeze position.
///*************************************************************************///

static var player1Name : String = "Player_1";
static var player2Name : String = "Player_2";
static var cpuName : String = "CPU";

// Available Game Modes:
/*
Indexes:
0 = 1 player against cpu
1 = 2 player against each other on the same platform/device
*/
static var gameMode : int;

//Odd rounds are player (Player-1) turn and Even rounds are AI (Player-2)'s
static var round : int;

//mamixmu distance that players can drag away from selected unit to shoot the ball (is in direct relation with shoot power)
static var maxDistance : float = 3.0;

//Turns in flags
static var playersTurn : boolean;
static var opponentsTurn : boolean;

//After players did their shoots, the round changes after this amount of time.
static var timeStepToAdvanceRound : float = 3; 

//Special occasions
static var goalHappened : boolean;
static var gameIsFinished : boolean;
static var goalLimit : int = 5; //To finish the game quickly, without letting the GameTime end.

///Game timer vars
static var gameTimer : float; //in seconds
private var remainingTime : String;
private var seconds : int;
private var minutes : int;

//Game Status
static var playerGoals : int;
static var opponentGoals : int;
static var gameTime : float; //Main game timer (in seconds). Always fixed.

//gameObject references
private var playerAIController : GameObject;
private var opponentAIController : GameObject;
private var ball : GameObject;

//AudioClips
var startWistle : AudioClip;
var finishWistle : AudioClip;
var goalSfx : AudioClip[];
var goalHappenedSfx : AudioClip[];
var crowdChants : AudioClip[];
private var canPlayCrowdChants;

//Public references
var gameStatusPlane : GameObject;		//user to show win/lose result at the end of match
var statusTextureObject : GameObject;	//plane we use to show the result texture in 3d world
var statusModes : Texture2D[];			//Available status textures

//*****************************************************************************
// Init. 
//*****************************************************************************
function Awake() {	
	//init
	goalHappened = false;
	gameIsFinished = false;
	playerGoals = 0;
	opponentGoals = 0;
	gameTime = 0;
	round = 1;
	seconds = 0;
	minutes = 0;
	canPlayCrowdChants = true;
	
	//hide gameStatusPlane
	gameStatusPlane.SetActive(false);
	
	//Translate gameTimer index to actual seconds
	switch(PlayerPrefs.GetInt("GameTime")) {
		case 0:
			gameTimer = 180;
			break;
		case 1:
			gameTimer = 300;
			break;
		case 2:
			gameTimer = 480;
			break;
		
		//You can add more cases and options here.
	}
	
	
	//Get Game Mode
	if(PlayerPrefs.HasKey("GameMode"))
		gameMode = PlayerPrefs.GetInt("GameMode");
	else
		gameMode = 0; // Deafault Mode (Player-1 vs AI)
	
	playerAIController = GameObject.FindGameObjectWithTag("playerAI");
	opponentAIController = GameObject.FindGameObjectWithTag("opponentAI");
	ball = GameObject.FindGameObjectWithTag("ball");
	
	manageGameModes();
}
	

//*****************************************************************************
// We have all units inside the game scene by default, but at the start of the game,
// we check which side in playing (should be active) and deactive the side that is
// not playing by deactivating all it's units.
//*****************************************************************************
private var player2Team : GameObject[];	//array of all player-2 units in the game
private var cpuTeam : GameObject[];		//array of all AI units in the game
function manageGameModes() {
	switch(gameMode) {
		case 0:
			//find and deactive all player2 units. This is player-1 vs AI.
			player2Team = GameObject.FindGameObjectsWithTag("Player_2");
			for(var unit : GameObject in player2Team) {
				unit.SetActive(false);
			}
			break;
		
		case 1:
			//find and deactive all AI Opponent units. This is Player-1 vs Player-2.
			cpuTeam = GameObject.FindGameObjectsWithTag("Opponent");
			for(var unit : GameObject in cpuTeam) {
				unit.SetActive(false);
			}
			//deactive opponent's AI
			opponentAIController.SetActive(false);
			break;
	}
}

function Start() {
	roundTurnManager();
	yield WaitForSeconds(1.5);
	playSfx(startWistle);
}

//*****************************************************************************
// FSM
//*****************************************************************************
function Update () {
	//check game finish status every frame
	if(!gameIsFinished) {
		manageGameStatus();
	}
	
	//every now and then, play some crowd chants
	playCrowdChants();
	
	//If you ever needed debug inforamtions:
	//print("GameRound: " + round + " & turn is for: " + whosTurn + " and GoalHappened is: " + goalHappened);
}

//*****************************************************************************
// This function gives turn to players in the game.
//*****************************************************************************
static var whosTurn : String;
function roundTurnManager() {
	
	if(gameIsFinished || goalHappened)
		return;
	
	//if round number is odd, it's players turn, else it's AI or player-2 's turn
	var carry : int;
	carry = round % 2;
	if(carry == 1) {
		playersTurn = true;
		opponentsTurn = false;
		playerController.canShoot = true;
		OpponentAI.opponentCanShoot = false;
		whosTurn = "player";
	} else {
		playersTurn = false;
		opponentsTurn = true;
		playerController.canShoot = false;
		OpponentAI.opponentCanShoot = true;
		whosTurn = "opponent";
	}	
	
	//Override
	//for two player game, players can always shoot.
	//we override this because both human players play on the same device and must be able to shoot at every turn.
	//we just limit their actions to their own units.
	if(gameMode == 1)
		playerController.canShoot = true;		
}

//*****************************************************************************
// What happens after a shoot is performed?
//*****************************************************************************
function managePostShoot(_shootBy : String) {
	//get who is did the shoot
	//if we had a goal after the shoot was done and just before the round change, leave the process to other controllers.
	var t : float;
	while(t < timeStepToAdvanceRound) {	
		t += Time.deltaTime;
		if(goalHappened) {
			return;
		} 		
		yield;
	}
	
	//we had a simple shoot with no goal result
	if(t >= timeStepToAdvanceRound) {
		//add to round counters
		switch(_shootBy) {
			case "Player":
				round = 2;
				break;		
			case "Player_2":
				round = 1;
				break;	
			case "Opponent":
				round = 1;
				break;
		}	
		roundTurnManager(); //cycle again between players
	}
}
		
//*****************************************************************************
// If we had a goal in this round, this is the function that manages all aspects of it.
//*****************************************************************************								
function managePostGoal(_goalBy : String) {
	//get who did the goal.
	
	//soft pause the game for reformation and other things...
	goalHappened = true;
	
	//add to goal counters
	switch(_goalBy) {
		case "Player":
			playerGoals++;
			round = 2; //goal by player-1 and opponent should start the next round
			break;
		case "Opponent":
			opponentGoals++;
			round = 1; //goal by opponent and player-1 should start the next round
			break;
	}
	
	//wait a few seconds to show the effects , and physics cooldown
	playSfx(goalSfx[Random.Range(0, goalSfx.Length)]);
	GetComponent.<AudioSource>().PlayOneShot(goalHappenedSfx[Random.Range(0, goalHappenedSfx.Length)], 1);
	yield WaitForSeconds(1);
	
	//bring the ball back to it's initial position
	ball.GetComponent(TrailRenderer).enabled = false;
	ball.GetComponent.<Rigidbody>().velocity = Vector3.zero;
	ball.GetComponent.<Rigidbody>().angularVelocity = Vector3.zero;
	ball.transform.position = Vector3(0, -0.81, -0.7);
	yield WaitForSeconds(1);
	ball.GetComponent(TrailRenderer).enabled = true;
	
	//*** reformation of units ***//
	//Reformation for player_1
	playerAIController.GetComponent(PlayerAI).changeFormation(PlayerAI.playerTeam, PlayerPrefs.GetInt("PlayerFormation"), 0.6, 1);
	
	//if this is player-1 vs player-2 match:
	if(GlobalGameManager.gameMode == 1) {
		playerAIController.GetComponent(PlayerAI).changeFormation(PlayerAI.player2Team, PlayerPrefs.GetInt("Player2Formation"), 0.6, -1);
	} else {	//if this is player-1 vs AI match:
		//get a new random formation everytime
		opponentAIController.GetComponent(OpponentAI).changeFormation(Random.Range(0, FormationManager.formations), 0.6);	
	}
	
	yield WaitForSeconds(3);

	//check if the game is finished or not
	if(playerGoals > goalLimit || opponentGoals > goalLimit) {
		gameIsFinished = true;
		manageGameFinishState();
		return;
	} 
	
	//else, continue to the next round
	goalHappened = false;
	roundTurnManager();
	playSfx(startWistle);
}

//***************************************************************************//
// Game status manager
//***************************************************************************//
var timeText : GameObject;			//UI 3d text object
var playerGoalsText : GameObject;	//UI 3d text object
var opponentGoalsText : GameObject;	//UI 3d text object
var playerOneName : GameObject;		//UI 3d text object
var playerTwoName : GameObject;		//UI 3d text object
function manageGameStatus() {
	seconds = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) % 60;
	minutes = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) / 60; 
	
	if(seconds == 0 && minutes == 0) {
		gameIsFinished = true;
		manageGameFinishState();
	}
	
	remainingTime = String.Format("{0:00} : {1:00}", minutes, seconds); 
	timeText.GetComponent(TextMesh).text = remainingTime.ToString();

	playerGoalsText.GetComponent(TextMesh).text = playerGoals.ToString();
	opponentGoalsText.GetComponent(TextMesh).text = opponentGoals.ToString();

	if(gameMode == 0) {
		playerOneName.GetComponent(TextMesh).text = player1Name;
		playerTwoName.GetComponent(TextMesh).text = cpuName;
	} else if(gameMode == 1) {
		playerOneName.GetComponent(TextMesh).text = player1Name;
		playerTwoName.GetComponent(TextMesh).text = player2Name;
	} 
}

//*****************************************************************************
// After the game is finished, this function handles the events.
//*****************************************************************************
function manageGameFinishState() {
	//Play gameFinish wistle
	playSfx(finishWistle);
	print("GAME IS FINISHED.");
	
	//show gameStatusPlane
	gameStatusPlane.SetActive(true);
	
	//for single player game, we should give the player some bonuses in case of winning the match
	if(gameMode == 0) {
		if(playerGoals > goalLimit || playerGoals > opponentGoals) {
			print("Player 1 is the winner!!");
			
			//set the result texture
			statusTextureObject.GetComponent.<Renderer>().material.mainTexture = statusModes[0];
			
			var playerWins = PlayerPrefs.GetInt("PlayerWins");
			var playerMoney = PlayerPrefs.GetInt("PlayerMoney");
			
			PlayerPrefs.SetInt("PlayerWins", ++playerWins);			//add to wins counter
			PlayerPrefs.SetInt("PlayerMoney", playerMoney + 100);	//handful of coins as the prize!
			
		} else if(opponentGoals > goalLimit || opponentGoals > playerGoals) {
		
			print("CPU is the winner!!");
			statusTextureObject.GetComponent.<Renderer>().material.mainTexture = statusModes[1];
			
		} else if(opponentGoals == playerGoals) {
		
			print("(Single Player) We have a Draw!");
			statusTextureObject.GetComponent.<Renderer>().material.mainTexture = statusModes[4];
		}	
	} else if(gameMode == 1) {
		if(playerGoals > opponentGoals) {
			print("Player 1 is the winner!!");
			statusTextureObject.GetComponent.<Renderer>().material.mainTexture = statusModes[2];
		} else if(playerGoals == opponentGoals) {
			print("(Two-Player) We have a Draw!");
			statusTextureObject.GetComponent.<Renderer>().material.mainTexture = statusModes[4];
		} else if(playerGoals < opponentGoals) {
			print("Player 2 is the winner!!");
			statusTextureObject.GetComponent.<Renderer>().material.mainTexture = statusModes[3];
		} 
	}
}


//*****************************************************************************
// Play a random crown sfx every now and then to spice up the game
//*****************************************************************************
function playCrowdChants() {
	if(canPlayCrowdChants) {
		canPlayCrowdChants = false;
		GetComponent.<AudioSource>().PlayOneShot(crowdChants[Random.Range(0, crowdChants.Length)], 1);
		yield WaitForSeconds(Random.Range(15, 35));
		canPlayCrowdChants = true;
	}
}


//*****************************************************************************
// Play sound clips
//*****************************************************************************
function playSfx(_clip : AudioClip){
	GetComponent.<AudioSource>().clip = _clip;
	if(!GetComponent.<AudioSource>().isPlaying) {
		GetComponent.<AudioSource>().Play();
	}
}
