#pragma strict
import System.Linq;
import System.Collections.Generic;

///*************************************************************************///
/// Main Player AI class.
/// why do we need a player AI class?
/// This class has a reference to all player-1 (and player-2) units, their formation and their position
/// and will be used to setup new formations for these units at the start of the game or when a goal happens.
///*************************************************************************///

static var playerTeam : GameObject[];	//array of all player-1 units
static var playerFormation : int;		//player-1 formation

//for two player game
static var player2Team : GameObject[];	//array of all player-2 units
static var player2Formation : int;		//player-2 formation

//flags
private var canChangeFormation : boolean;


//*****************************************************************************
// Init. 
//*****************************************************************************
function Awake () {
	
	canChangeFormation = true;	//we just change the formation at the start of the game. No more change of formation afterwards!
	if(PlayerPrefs.HasKey("PlayerFormation"))
		playerFormation = PlayerPrefs.GetInt("PlayerFormation");
	else	
		playerFormation = 0; //Default Formation
	
	//cache all player_1 units
	playerTeam = GameObject.FindGameObjectsWithTag("Player");
	//debug
	var i : int = 1;
	for(var unit : GameObject in playerTeam) {
		//Optional
		unit.name = "PlayerUnit-" + i;
		unit.GetComponent(playerController).unitIndex = i;
		i++;
	}
	
	//if this is a 2-player local game
	if(GlobalGameManager.gameMode == 1) {
		//cache all player_2 units
		player2Team = GameObject.FindGameObjectsWithTag("Player_2");
		var j : int = 1;
		for(var unit : GameObject in player2Team) {
			//Optional
			unit.name = "Player2Unit-" + j;
			unit.GetComponent(playerController).unitIndex = j;
			j++;		
		}
		
		//fetch player_2's formation
		if(PlayerPrefs.HasKey("Player2Formation"))
			player2Formation = PlayerPrefs.GetInt("Player2Formation");
		else	
			player2Formation = 0; //Default Formation
	}
}


function Start() {
	changeFormation(playerTeam, playerFormation, 1, 1);
	//For two-player mode,
	if(GlobalGameManager.gameMode == 1) 
		changeFormation(player2Team, player2Formation, 1, -1);
		
	canChangeFormation = false;
}

//*****************************************************************************
// changeFormation function take all units, selected formation and side of the player (left half or right half)
// and then position each unit on it's destination.
// speed is used to fasten the translation of units to their destinations.
//*****************************************************************************
function changeFormation(_team : GameObject[], _formationIndex : int, _speed : float, _dir : int) {

	//cache the initial position of all units
	var unitsSartingPosition = new List.<Vector3>();
	for(var unit in _team) {
		unitsSartingPosition.Add(unit.transform.position); //get the initial postion of this unit for later use.
		unit.GetComponent(MeshCollider).enabled = false;	//no collision for this unit till we are done with re positioning.
	}
	
	var t : float = 0;
	while(t < 1) {
		t += Time.deltaTime * _speed;
		for(var cnt : int = 0; cnt < _team.length; cnt++) {
			_team[cnt].transform.position.x = Mathf.SmoothStep(	unitsSartingPosition[cnt].x, 
																FormationManager.getPositionInFormation(_formationIndex, cnt).x * _dir,
																t);
			_team[cnt].transform.position.y = Mathf.SmoothStep(	unitsSartingPosition[cnt].y, 
																FormationManager.getPositionInFormation(_formationIndex, cnt).y,
																t);															
			_team[cnt].transform.position.z = FormationManager.fixedZ; //always fixed on -0.5
		}		
		yield;
	}
	
	if(t >= 1) {
		canChangeFormation = true;
		for(var unit in _team)
			unit.GetComponent(MeshCollider).enabled = true; //collision is now enabled.
	}
}


