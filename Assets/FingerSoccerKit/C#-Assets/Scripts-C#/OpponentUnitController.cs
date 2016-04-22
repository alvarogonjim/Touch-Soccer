using UnityEngine;
using System.Collections;

public class OpponentUnitController : MonoBehaviour {

	///*************************************************************************///
	/// Unit controller class for AI units
	///*************************************************************************///

	internal int unitIndex;					//every AI unit has an index. this is for the AI controller to know which unit must be selected.
											//Indexes are given to units by the AIController itself.

	private bool  canShowSelectionCircle;	//if the turn is for AI, units can show the selection circles.
	public GameObject selectionCircle;		//reference to gameObject.

	void Awake (){
		canShowSelectionCircle = true;
	}

	void Update (){	
		if(GlobalGameManager.opponentsTurn && canShowSelectionCircle && !GlobalGameManager.goalHappened)
			selectionCircle.GetComponent<Renderer>().enabled = true;
		else	
			selectionCircle.GetComponent<Renderer>().enabled = false;			
	}
}