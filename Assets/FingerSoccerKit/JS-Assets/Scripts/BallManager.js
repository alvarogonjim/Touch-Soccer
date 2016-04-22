#pragma strict

//*****************************************************************************
// Main Ball Manager.
// This class controls ball collision with Goal triggers and gatePoles, 
// and also stops the ball when the spped is too low.
//*****************************************************************************

private var gameController : GameObject;	//Reference to main game controller
var ballHitPost : AudioClip;				//Sfx for hitting the poles

function Awake() {
	gameController = GameObject.FindGameObjectWithTag("GameController");
}

function Update () {
	manageBallFriction();
}

function LateUpdate () {
	//we restrict rotation and position once again to make sure that ball won't has an unwanted effect.
	transform.position.z = -0.5;
	transform.rotation = Quaternion.Euler(90, 0, 0);
}

//*****************************************************************************
// Check ball's speed at all times.
//*****************************************************************************
private var ballSpeed : float;
function manageBallFriction() {
	ballSpeed = GetComponent.<Rigidbody>().velocity.magnitude;
	//print("Ball Speed: " + rigidbody.velocity.magnitude);
	if(ballSpeed < 0.5) {
		//forcestop the ball
		//rigidbody.velocity = Vector3.zero;
		//rigidbody.angularVelocity = Vector3.zero;
		GetComponent.<Rigidbody>().drag = 2;
	} else {
		//let it slide
		GetComponent.<Rigidbody>().drag = 0.9;
	}
}


function OnCollisionEnter(other : Collision) {
	switch(other.gameObject.tag) {
		case "gatePost":
			playSfx(ballHitPost);
			break;
	}
}

function OnTriggerEnter(other : Collider) {
	switch(other.gameObject.tag) {
		case "opponentGoalTrigger":
			gameController.GetComponent(GlobalGameManager).managePostGoal("Player");
			break;
			
		case "playerGoalTrigger":
			gameController.GetComponent(GlobalGameManager).managePostGoal("Opponent");
			break;
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
