#pragma strict
import System.Collections.Generic;

///*************************************************************************///
/// Main Player Controller.
/// This class manages the shooting process of human players.
/// This also handles the rendering of debug lines in editor.
///*************************************************************************///

// Public Variables //
internal var unitIndex : int;				//This unit's ID (given automatically by PlayerAI class)
var selectionCircle : GameObject;			//Reference to gameObject


//Referenced GameObjects
private var helperBegin : GameObject; 		//Start helper
private var helperEnd : GameObject; 		//End Helper
private var arrowPlane : GameObject; 		//arrow plane which is used to show shotPower

private var gameController : GameObject;	//Reference to main game controller


private var currentDistance : float;		//real distance of our touch/mouse position from initial drag position
private var safeDistance : float; 			//A safe distance value which is always between min and max to avoid supershoots

private var pwr : float;					//shoot power

//this vector holds shooting direction
private var shootDirectionVector : Vector3;

//prevent player to shoot twice in a round
static var canShoot : boolean;
private var shootTime : float;
private var timeAllowedToShoot : int = 10000; //In Seconds (in this kit we give players unlimited time to perform their turn of shooting)


//*****************************************************************************
// Init
//*****************************************************************************
function Awake() {
	//Find and cache important gameObjects
	helperBegin = GameObject.FindGameObjectWithTag("mouseHelperBegin");
	helperEnd = GameObject.FindGameObjectWithTag("mouseHelperEnd");
	arrowPlane = GameObject.FindGameObjectWithTag("helperArrow");		
	gameController = GameObject.FindGameObjectWithTag("GameController");
	
	//Init Variables
	pwr = 0.1;
	currentDistance = 0;
	shootDirectionVector = Vector3(0,0,0);
	canShoot = true;
	shootTime = timeAllowedToShoot;
	arrowPlane.GetComponent.<Renderer>().enabled = false; //hide arrowPlane
}

function Start() {
	
}

function Update() {
	
	//Active the selection circles around Player units when they have the turn.
	if(GlobalGameManager.playersTurn && gameObject.tag == "Player" && !GlobalGameManager.goalHappened)
		selectionCircle.GetComponent.<Renderer>().enabled = true;
	else if(GlobalGameManager.opponentsTurn && gameObject.tag == "Player_2" && !GlobalGameManager.goalHappened)
		selectionCircle.GetComponent.<Renderer>().enabled = true;			
	else	
		selectionCircle.GetComponent.<Renderer>().enabled = false;	
}


//***************************************************************************//
// Works fine with mouse and touch
// This is the main functiuon used to manage drag on units, calculating the power and debug vectors, and set the final parameters to shoot.
//***************************************************************************//
function OnMouseDrag() {
	if( canShoot && ((GlobalGameManager.playersTurn && gameObject.tag == "Player") || (GlobalGameManager.opponentsTurn && gameObject.tag == "Player_2")) )
		{
			//print("Draged");
			currentDistance = Vector3.Distance(helperBegin.transform.position, transform.position);
			
			//limiters
			if(currentDistance <= GlobalGameManager.maxDistance)
				safeDistance = currentDistance;
			else
				safeDistance = GlobalGameManager.maxDistance;
				
			pwr = Mathf.Abs(safeDistance) * 12; //this is very important. change with extreme caution.
			
			//show the power arrow above the unit and scale is accordingly.
			manageArrowTransform();		
			
			//position of helperEnd
			//HelperEnd is the exact opposite (mirrored) version of our helperBegin object 
			//and help us to calculate debug vectors and lines for a perfect shoot.
			//Please refer to the basic geometry references of your choice to understand the math.
			var dxy : Vector3 = helperBegin.transform.position - transform.position;
			var diff : float = dxy.magnitude;
			helperEnd.transform.position = transform.position + ((dxy / diff) * currentDistance * -1);
			helperEnd.transform.position.z = -0.5;
			
			
			//debug line from initial position to our current touch position
			Debug.DrawLine(transform.position, helperBegin.transform.position, Color.red);
			//debug line from initial position to maximum power position (mirrored)
			Debug.DrawLine(transform.position, arrowPlane.transform.position, Color.blue);
			//debug line from initial position to the exact opposite position (mirrored) of our current touch position
			Debug.DrawLine(transform.position, (2 * transform.position) - helperBegin.transform.position, Color.yellow);
			//cast ray forward and collect informations
			castRay();
			
			//Not used! You can extend this function to have more precise control over physics of the game
			//sweepTest();
			
			//final vector used to shoot the unit.
			shootDirectionVector = Vector3.Normalize(helperBegin.transform.position - transform.position);
			//print(shootDirectionVector);
		}
}



//***************************************************************************//
// Cast the rigidbody's shape forward to see if it is about to hit anything.
//***************************************************************************//
function sweepTest() {
	var hit : RaycastHit;
	if ( GetComponent(Rigidbody).SweepTest( (helperEnd.transform.position - transform.position).normalized, hit, 15 )) {
		print("if hit ??? : " + hit.distance + " - " + hit.transform.gameObject.name);
	}
}


//***************************************************************************//
// Cast a ray forward and collect informations like if it hits anything...
//***************************************************************************//
private var hitInfo : RaycastHit;
private var ray : Ray;
function castRay() {
	
	//cast the ray from units position with a normalized direction out of it which is mirrored to our current drag vector.
	ray = new Ray(transform.position, (helperEnd.transform.position - transform.position).normalized );
		
	if(Physics.Raycast(ray, hitInfo, currentDistance)) {
		var objectHit : GameObject = hitInfo.transform.gameObject;
		
		//debug line whenever the ray hits something.
		Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan);
				
		//draw reflected vector like a billiard game. this is the out vector which reflects from targets geometry.
		var reflectedVector : Vector3 = Vector3.Reflect( (hitInfo.point - ray.origin),  hitInfo.normal );
		Debug.DrawRay(hitInfo.point, reflectedVector , Color.gray, 0.2);
		
		//draw inverted reflected vector (useful for fine-tuning the final shoot)
		Debug.DrawRay(hitInfo.transform.position, reflectedVector * -1, Color.white, 0.2);
		
		//draw the inverted normal which is more likely to be similar to real world response.
		Debug.DrawRay(hitInfo.transform.position, hitInfo.normal * -3, Color.red, 0.2);
		
		//Debug
		//print("Ray hits: " + objectHit.name + " At " + Time.time + " And Reflection is: " + reflectedVector);
	}
}

//***************************************************************************//
// Unhide and process the transform and scale of the power Arrow object
//***************************************************************************//
function manageArrowTransform() {
	//power arrow codes
	//hide arrowPlane
	arrowPlane.GetComponent.<Renderer>().enabled = true;
	
	//calculate position
	if(currentDistance <= GlobalGameManager.maxDistance) {
		arrowPlane.transform.position.x = (2 * transform.position.x) - helperBegin.transform.position.x;
		arrowPlane.transform.position.y = (2 * transform.position.y) - helperBegin.transform.position.y;
		arrowPlane.transform.position.z = -1.5;
	} else {
		var dxy : Vector3 = helperBegin.transform.position - transform.position;
		var diff : float = dxy.magnitude;
		arrowPlane.transform.position = transform.position + ((dxy / diff) * GlobalGameManager.maxDistance * -1);
		arrowPlane.transform.position.z = -1.5;
	}

	//calculate rotation
	var dir : Vector3 = helperBegin.transform.position - transform.position;
	var outRotation : float; // between 0 - 360
	
	if(Vector3.Angle(dir, transform.forward) > 90) 
		outRotation = Vector3.Angle(dir, transform.right);
	else
		outRotation = Vector3.Angle(dir, transform.right) * -1;
		
	arrowPlane.transform.eulerAngles = Vector3(0, 0, outRotation);
	//print(Vector3.Angle(dir, transform.forward));
	
	//calculate scale
	var scaleCoefX : float = Mathf.Log(1 + safeDistance/2, 2) * 2.2;
	var scaleCoefY : float = Mathf.Log(1 + safeDistance/2, 2) * 2.2;
	arrowPlane.transform.localScale = Vector3(1 + scaleCoefX, 1 + scaleCoefY, 0.001); //default scale
}


//***************************************************************************//
// Actual shoot fucntion
//***************************************************************************//
function OnMouseUp() {

	//Special checks for 2-player game
	if( (GlobalGameManager.playersTurn && gameObject.tag == "Player_2") || (GlobalGameManager.opponentsTurn && gameObject.tag == "Player") ) {
		arrowPlane.GetComponent.<Renderer>().enabled = false;
		return;
	}
	
	//give the player a second chance to choose another ball if drag on the unit is too low
	print("currentDistance: " + currentDistance);
	if(currentDistance < 0.75) {
		arrowPlane.GetComponent.<Renderer>().enabled = false;
		return;
	}
	
	//But if player wants to shoot anyway:
	//prevent double shooting in a round
	if(!canShoot)
		return;
	
	//no more shooting is possible	
	canShoot = false;
	
	//keep track of elapsed time after letting the ball go, 
	//so we can findout if ball has stopped and the round should be changed
	//this is the time which user released the button and shooted the ball
	shootTime = Time.time;
	
	//hide helper arrow object
	arrowPlane.GetComponent.<Renderer>().enabled = false;
	
	//do the physics calculations and shoot the ball 
	var outPower = shootDirectionVector * pwr * -1;
	
	//always make the player to move only in x-y plane and not on the z direction
	print("shoot power: " + outPower.magnitude);		
	GetComponent.<Rigidbody>().AddForce(outPower, ForceMode.Impulse);
	
	//change the turn
	if(GlobalGameManager.gameMode == 0)
		gameController.GetComponent(GlobalGameManager).managePostShoot("Player");
	else if(GlobalGameManager.gameMode == 1)
		gameController.GetComponent(GlobalGameManager).managePostShoot(gameObject.tag);
}
