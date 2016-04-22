#pragma strict

///*************************************************************************///
/// Mouse Follower class.
/// mouseHelperBegin is a helper gameObject which always follows mouse position
/// and provides useful informations for various controllers.
/// This script also works fine with touch.
///*************************************************************************///

private var zOffset : float = -0.5; //fixed position on Z axis.

function Start() {
	transform.position.z = zOffset; //apply fixed offset
}

private var tmpPosition : Vector3;
function Update() {
	//get mouse position in game scene.
	tmpPosition = Camera.main.ScreenToWorldPoint( Vector3(	Input.mousePosition.x,
															Input.mousePosition.y, 
															10));
	//follow the mouse
	transform.position = Vector3(	tmpPosition.x, 
									tmpPosition.y, 
									zOffset);
}
