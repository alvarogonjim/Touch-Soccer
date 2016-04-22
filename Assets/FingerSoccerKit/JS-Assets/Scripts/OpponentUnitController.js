#pragma strict

///*************************************************************************///
/// Unit controller class for AI units
///*************************************************************************///

internal var unitIndex : int;					//every AI unit has an index. this is for the AI controller to know which unit must be selected.
												//Indexes are given to units by the AIController itself.

private var canShowSelectionCircle : boolean;	//if the turn is for AI, units can show the selection circles.
var selectionCircle : GameObject;				//reference to gameObject.

function Awake() {
	canShowSelectionCircle = true;
}

function Update() {	
	if(GlobalGameManager.opponentsTurn && canShowSelectionCircle && !GlobalGameManager.goalHappened)
		selectionCircle.GetComponent.<Renderer>().enabled = true;
	else	
		selectionCircle.GetComponent.<Renderer>().enabled = false;			
}