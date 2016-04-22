#pragma strict
import System.Linq;
import System.Collections.Generic;

///*************************************************************************///
/// Main AI Controller.
/// This class manages the shooting process of AI (CPU opponent).
/// This also handles the rendering of AI debug lines in editor.
///*************************************************************************///

static var myTeam : GameObject[];				//List of all AI units
private var target : GameObject;				//reference to main Ball
private var distanceToTarget : float;			//Distance of selected unit to ball
private var directionToTarget : Vector3;		//Direction of selected unit to ball
private var shootPower : float = 40;			//AI shoot power. Edit with extreme caution!!!!

static var opponentCanShoot : boolean;			//Allowed to shoot? flag
private var shootTime : float;					//Allowed time to perform the shoot
private var isReadyToShoot : boolean;			//if all processes are done, flag

//AI
private var actor : GameObject;					//Selected unit to shoot	
private var gameController : GameObject;		//Reference to main game controller
private var PlayerBasketCenter : GameObject;	//helper object which shows the center of player gate to the AI
//static var scoreQueue : int;					//
private var opponentFormation : int;			//Selected formation for AI
private var canChangeFormation : boolean;		//Is allowed to change formation on the fly?

//*****************************************************************************
// Init. Updates the 3d texts with saved values fetched from playerprefs.
//*****************************************************************************
function Awake() {
	gameController = GameObject.FindGameObjectWithTag("GameController");
	target = GameObject.FindGameObjectWithTag("ball");
	PlayerBasketCenter = GameObject.FindGameObjectWithTag("PlayerBasketCenter");
	isReadyToShoot = false;
	opponentCanShoot = true;
	
	canChangeFormation = true;
	if(PlayerPrefs.HasKey("OpponentFormation"))
		opponentFormation = PlayerPrefs.GetInt("OpponentFormation");
	else	
		opponentFormation = 0; //Default Formation
	
	//cache all available units
	myTeam = GameObject.FindGameObjectsWithTag("Opponent");
	//debug
	var i : int = 1;
	for(var unit : GameObject in myTeam) {
		//Optional
		unit.name = "Opponent-Player-" + i;
		unit.GetComponent(OpponentUnitController).unitIndex = i;
		i++;		
		//print("My Team: " + unit.name);
	}
}


function Start() {
	changeFormation(opponentFormation, 1);
	canChangeFormation = false;
}


//*****************************************************************************
// AI can change it's formation to a new one after it deliver or receive a goal.
//*****************************************************************************
function changeFormation(_formationIndex : int, _speed : float) {

	//cache the initial position of all units
	var unitsSartingPosition = new List.<Vector3>();
	for(var unit in myTeam) {
		unitsSartingPosition.Add(unit.transform.position);	//get the initial postion of this unit for later use.
		unit.GetComponent(MeshCollider).enabled = false;	//no collision for this unit till we are done with re positioning.
	}
	
	var t : float = 0;
	while(t < 1) {
		t += Time.deltaTime * _speed;
		for(var cnt : int = 0; cnt < myTeam.length; cnt++) {
			myTeam[cnt].transform.position.x = Mathf.SmoothStep(	unitsSartingPosition[cnt].x, 
																	FormationManager.getPositionInFormation(_formationIndex, cnt).x * -1,
																	t);
			myTeam[cnt].transform.position.y = Mathf.SmoothStep(	unitsSartingPosition[cnt].y, 
																	FormationManager.getPositionInFormation(_formationIndex, cnt).y,
																	t);															
			myTeam[cnt].transform.position.z = FormationManager.fixedZ;
		}		
		yield;
	}
	
	if(t >= 1) {
		canChangeFormation = true;
		for(var unit in myTeam)
			unit.GetComponent(MeshCollider).enabled = true;	//collision is now enabled.
	}
}


function Update() {
	//prepare to shoot
	if(GlobalGameManager.opponentsTurn && opponentCanShoot) {
		opponentCanShoot = false;
		shoot();	
	}
}

/// *************************************************************
/// Shoot the selected unit.
/// All AI steps are described and fully commented.
/// *************************************************************
private var bestShooter : GameObject;	//select the best unit to shoot.
function shoot() {

	//wait for a while to fake thinking process :)
	yield WaitForSeconds(1.0);

	//init
	bestShooter = null;

	//1. find units with good position to shoot
	//Units that are in the right hand side of the ball are considered a better options. 
	//They can have better angle to the player's gate.
	var shooters = new List.<GameObject>();		//list of all good units
	var distancesToBall = new List.<float>();	//distance of these good units to the ball
	for(var shooter : GameObject in myTeam) {
		if(shooter.transform.position.x > target.transform.position.x + 1.5) {
			shooters.Add(shooter);
			distancesToBall.Add( Vector3.Distance(shooter.transform.position, target.transform.position) );
		}
	}
	
	//if we found atleast one good unit...
	if(shooters.Count > 0) {
		//print("we have " + shooters.Count + " unit(s) in a good shoot position");
		var minDistance : float = 1000;
		var minDistancePlayerIndex : int = 0;
		for(var i : int; i < distancesToBall.Count; i++) {
			if(distancesToBall[i] <= minDistance) {
				minDistance = distancesToBall[i];
				minDistancePlayerIndex = i;
			}
			//print(shooters[i] + " distance to ball is " + distancesToBall[i]);
		}
		//find the unit which is most closed to ball.
		bestShooter = shooters[minDistancePlayerIndex];
		//print("MinDistance to ball is: " + minDistance + " by opponent " + bestShooter.name);	
	} else {
		//print("no player is availabe for a good shoot!");
		//Select a random unit
		bestShooter = myTeam[Random.Range(0, myTeam.Length)];
	}
	
	//calculate direction and power and add a little randomness to the shoot (can be used to make the game easy or hard)
	var distanceCoef : float;
	if(minDistance <= 5 && minDistance >= 0) 
		distanceCoef = Random.Range(1.0, 2.5);
	else
		distanceCoef = Random.Range(2.0, 4.0);
		
	
	//////////////////////////////////////////////////////////////////////////////////////////////////
	// Detecting the best angle for the shoot
	//////////////////////////////////////////////////////////////////////////////////////////////////
	var vectorToGate : Vector3;					//direct vector from shooter to gate
	var vectorToBall : Vector3;					//direct vector from shooter to ball
	var straightAngleDifferential : float;		//angle between "vectorToGate" and "vectorToBall" vectors
	vectorToGate = PlayerBasketCenter.transform.position - bestShooter.transform.position;
	vectorToBall = target.transform.position - bestShooter.transform.position;
	straightAngleDifferential = Vector3.Angle(vectorToGate, vectorToBall);
	//if angle between these two vector is lesser than 10 for example, we have a clean straight shot to gate.
	//but if the angle is more, we have to calculate the correct angle for the shoot.
	print("straightAngleDifferential: " + straightAngleDifferential);
	//////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////
	
	var shootPositionDifferential : float = bestShooter.transform.position.y - target.transform.position.y;
	print("Y differential for shooter is: " + shootPositionDifferential);
	
	if(straightAngleDifferential <= 10) {
		
		//direct shoot
		directionToTarget = target.transform.position - bestShooter.transform.position;
	
	} else if( Mathf.Abs(shootPositionDifferential) <= 0.5 ) {
	
		//direct shoot
		directionToTarget = target.transform.position - bestShooter.transform.position;
		
	} else if( Mathf.Abs(shootPositionDifferential) > 0.5 && Mathf.Abs(shootPositionDifferential) <= 1 ) {
	
		if(shootPositionDifferential > 0)
			directionToTarget = (target.transform.position - Vector3(0, bestShooter.transform.localScale.z / 2.5, 0)) - bestShooter.transform.position;
		else
			directionToTarget = (target.transform.position + Vector3(0, bestShooter.transform.localScale.z / 2.5, 0)) - bestShooter.transform.position;
			
	} else if( Mathf.Abs(shootPositionDifferential) > 1 && Mathf.Abs(shootPositionDifferential) <= 2 ) {
	
		if(shootPositionDifferential > 0)
			directionToTarget = (target.transform.position - Vector3(0, bestShooter.transform.localScale.z / 2, 0)) - bestShooter.transform.position;
		else
			directionToTarget = (target.transform.position + Vector3(0, bestShooter.transform.localScale.z / 2, 0)) - bestShooter.transform.position;
			
	} else if( Mathf.Abs(shootPositionDifferential) > 2 && Mathf.Abs(shootPositionDifferential) <= 3 ) {
	
		if(shootPositionDifferential > 0)
			directionToTarget = (target.transform.position - Vector3(0, bestShooter.transform.localScale.z / 1.6, 0)) - bestShooter.transform.position;
		else
			directionToTarget = (target.transform.position + Vector3(0, bestShooter.transform.localScale.z / 1.6, 0)) - bestShooter.transform.position;
			
	} else if( Mathf.Abs(shootPositionDifferential) > 3 ) {
	
		if(shootPositionDifferential > 0)
			directionToTarget = (target.transform.position - Vector3(0, bestShooter.transform.localScale.z / 1.25, 0)) - bestShooter.transform.position;
		else
			directionToTarget = (target.transform.position + Vector3(0, bestShooter.transform.localScale.z / 1.25, 0)) - bestShooter.transform.position;
			
	}
	
	//set the shhot power based on direction and distance to ball
	var appliedPower = Vector3.Normalize(directionToTarget) * shootPower;
	bestShooter.GetComponent.<Rigidbody>().AddForce(appliedPower, ForceMode.Impulse);
	
	print(bestShooter.name + " shot the ball with a power of " + appliedPower.magnitude);
	visualDebug();

	gameController.GetComponent(GlobalGameManager).managePostShoot("Opponent");
}


//*****************************************************************************
// Draw the debug lines of AI controller in editor
//*****************************************************************************
function visualDebug() {
	//Visual debug
	while(!isReadyToShoot) {
	
		//draw helper line from shooter unit to ball
		Debug.DrawLine(bestShooter.transform.position, target.transform.position, Color.green);
		
		//draw helper line which gets out of ball after direct impact
		Debug.DrawLine(target.transform.position, (target.transform.position * 2 - bestShooter.transform.position), Color.gray);
		
		//draw helper line from shooter unit to ball with ball's tangent in mind
		Debug.DrawLine(bestShooter.transform.position, target.transform.position + Vector3(0, target.transform.localScale.z / 2, 0) , Color.red);
		Debug.DrawLine(bestShooter.transform.position, target.transform.position - Vector3(0, target.transform.localScale.z / 2, 0) , Color.red);
		
		//draw helper line from shooter unit to ball with shooter's tangent in mind
		Debug.DrawLine(bestShooter.transform.position, target.transform.position + Vector3(0, bestShooter.transform.localScale.z / 2, 0), Color.yellow);
		Debug.DrawLine(bestShooter.transform.position, target.transform.position - Vector3(0, bestShooter.transform.localScale.z / 2, 0), Color.yellow);
		
		//draw helper line from shooter unit to player's gate		
		Debug.DrawLine(bestShooter.transform.position, PlayerBasketCenter.transform.position, Color.cyan);
		
		//draw helper line from ball to player's gate
		Debug.DrawLine(target.transform.position, PlayerBasketCenter.transform.position, Color.green);
		
		yield;
	};
}

