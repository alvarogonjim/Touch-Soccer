#pragma strict

///*************************************************************************///
/// Main Config Controller.
/// This class provides the available settings to the players, like formations,
/// gameDuration, etc... and then prepare the main game with the setting.
///*************************************************************************///

private var buttonAnimationSpeed : float = 11;	//speed on animation effect when tapped on button
private var canTap : boolean = true;			//flag to prevent double tap
var tapSfx : AudioClip;							//tap sound for buttons click

var availableFormations : String[];				//Just the string values. We setup actual values somewhere else.
var availableTimes : String[];					//Just the string values. We setup actual values somewhere else.

//Reference to gameObjects
var p1FormationLabel : GameObject;				//UI 3d text object
var p2FormationLabel : GameObject;				//UI 3d text object
var gameTimeLabel : GameObject;					//UI 3d text object

private var p1FormationCounter : int = 0;		//Actual player-1 formation index
private var p2FormationCounter : int = 0;		//Actual player-2 formation index
private var timeCounter : int = 0;				//Actual game-time index

//*****************************************************************************
// Init. Updates the 3d texts with saved values fetched from playerprefs.
//***************************************************************************
function Awake() {
	p1FormationLabel.GetComponent(TextMesh).text = availableFormations[p1FormationCounter];	//loads default formation
	p2FormationLabel.GetComponent(TextMesh).text = availableFormations[p2FormationCounter];	//loads default formation
	gameTimeLabel.GetComponent(TextMesh).text = availableTimes[timeCounter];				//loads default game-time
}

//*****************************************************************************
// FSM
//*****************************************************************************
function Update () {	
	if(canTap) {
		tapManager();
	}
}

//*****************************************************************************
// This function monitors player touches on menu buttons.
// detects both touch and clicks and can be used with editor, handheld device and 
// every other platforms at once.
//*****************************************************************************
private var hitInfo : RaycastHit;
private var ray : Ray;
function tapManager() {

	//Mouse of touch?
	if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
		ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
	else if(Input.GetMouseButtonUp(0))
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	else
		return;
		
	if (Physics.Raycast(ray, hitInfo)) {
		var objectHit : GameObject = hitInfo.transform.gameObject;
		
		switch(objectHit.name) {
		
			case "p1-FBL":
				playSfx(tapSfx);
				animateButton(objectHit);	//button scale-animation to user input
				p1FormationCounter--;		//cycle through available formation indexs for this player. This is the main index value.
				fixCounterLengths();		//when reached to the last option, start from the first index of the other side.
				p1FormationLabel.GetComponent(TextMesh).text = availableFormations[p1FormationCounter]; //set the string on the UI
				break;
				
			case "p1-FBR":
				playSfx(tapSfx);
				animateButton(objectHit);
				p1FormationCounter++;
				fixCounterLengths();
				p1FormationLabel.GetComponent(TextMesh).text = availableFormations[p1FormationCounter];
				break;
				
			case "p2-FBL":
				playSfx(tapSfx);
				animateButton(objectHit);
				p2FormationCounter--;
				fixCounterLengths();
				p2FormationLabel.GetComponent(TextMesh).text = availableFormations[p2FormationCounter];
				break;
			
			case "p2-FBR":
				playSfx(tapSfx);
				animateButton(objectHit);
				p2FormationCounter++;
				fixCounterLengths();
				p2FormationLabel.GetComponent(TextMesh).text = availableFormations[p2FormationCounter];
				break;
				
			case "durationBtnLeft":
				playSfx(tapSfx);
				animateButton(objectHit);
				timeCounter--;
				fixCounterLengths();
				gameTimeLabel.GetComponent(TextMesh).text = availableTimes[timeCounter];
				break;
				
			case "durationBtnRight":
				playSfx(tapSfx);
				animateButton(objectHit);
				timeCounter++;
				fixCounterLengths();
				gameTimeLabel.GetComponent(TextMesh).text = availableTimes[timeCounter];
				break;
				
			case "Btn-Back":
				playSfx(tapSfx);
				animateButton(objectHit);
				//No need to save anything
				Application.LoadLevel("Menu");
				break;
				
			case "Btn-Start":
				playSfx(tapSfx);
				animateButton(objectHit);
				//Save configurations
				PlayerPrefs.SetInt("PlayerFormation", p1FormationCounter);		//save the player-1 formation index
				PlayerPrefs.SetInt("Player2Formation", p2FormationCounter);		//save the player-2 formation index
				PlayerPrefs.SetInt("GameTime", timeCounter);					//save the game-time value
				//** Please note that we just set the indexes here. We fetch the actual index values in the <<Game>> scene.
				
				yield WaitForSeconds(0.5);
				Application.LoadLevel("Game");
				break;			
		}	
	}
}

//*****************************************************************************
// When selection form available options, when player reaches the last option,
// and still taps on the next option, this will cycle it again to the first element of options.
// This is for p-1, p-2 and time settings.
//*****************************************************************************
function fixCounterLengths() {
	//set array counters limitations
	
	//Player-1
	if(p1FormationCounter > availableFormations.length - 1)
		p1FormationCounter = 0;
	if(p1FormationCounter < 0)
		p1FormationCounter = availableFormations.length - 1;
		
	//Player-2	
	if(p2FormationCounter > availableFormations.length - 1)
		p2FormationCounter = 0;
	if(p2FormationCounter < 0)
		p2FormationCounter = availableFormations.length - 1;
		
	//GameTime
	if(timeCounter > availableTimes.length - 1)
		timeCounter = 0;
	if(timeCounter < 0)
		timeCounter = availableTimes.length - 1;
}


function animateButton(_btn : GameObject) {
	canTap = false;
	var startingScale : Vector3 = _btn.transform.localScale;
	var destinationScale : Vector3 = startingScale * 1.2;
	var t = 0.0; 
	while (t <= 1.0) {
		t += Time.deltaTime * buttonAnimationSpeed;
		_btn.transform.localScale.x = Mathf.SmoothStep(startingScale.x, destinationScale.x, t);
		_btn.transform.localScale.y = Mathf.SmoothStep(startingScale.y, destinationScale.y, t);
		yield;
	}
	
	var r = 0.0; 
	if(_btn.transform.localScale.x >= destinationScale.x) {
		while (r <= 1.0) {
			r += Time.deltaTime * buttonAnimationSpeed;
			_btn.transform.localScale.x = Mathf.SmoothStep(destinationScale.x, startingScale.x, r);
			_btn.transform.localScale.y = Mathf.SmoothStep(destinationScale.y, startingScale.y, r);
			yield;
		}
	}
	
	if(r >= 1)
		canTap = true;
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