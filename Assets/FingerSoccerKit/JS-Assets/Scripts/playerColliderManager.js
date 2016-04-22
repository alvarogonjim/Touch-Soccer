#pragma strict

///*************************************************************************///
/// Optional controller for collision of player units vs other items in the scene like ball or opponent units
///*************************************************************************///

var unitsBallHit : AudioClip;		//units hits the ball sfx
var unitsGeneralHit : AudioClip;	//units general hit sfx (Not used)

function OnCollisionEnter (other : Collision) {
	switch(other.gameObject.tag) {
		case "Opponent":
			//PlaySfx(unitsGeneralHit);
			break;
		case "ball":
			PlaySfx(unitsBallHit);
			break;
	}
}
	
//*****************************************************************************
// Play sound clips
//*****************************************************************************
function PlaySfx(_clip : AudioClip){
	GetComponent.<AudioSource>().clip = _clip;
	if(!GetComponent.<AudioSource>().isPlaying) {
		GetComponent.<AudioSource>().Play();
	}
}