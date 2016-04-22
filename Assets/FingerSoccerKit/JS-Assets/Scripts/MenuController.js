#pragma strict

///*************************************************************************///
/// Main Menu Controller.
/// This class handles all touch events on buttons, and also updates the 
/// player status (wins and available-money) on screen.
///*************************************************************************///

private var buttonAnimationSpeed : float = 9;	//speed on animation effect when tapped on button
private var canTap : boolean = true;			//flag to prevent double tap
var tapSfx : AudioClip;							//tap sound for buttons click

//Reference to GameObjects
var playerWins : GameObject;					//UI 3d text object
var playerMoney : GameObject;					//UI 3d text object


//*****************************************************************************
// Init. Updates the 3d texts with saved values fetched from playerprefs.
//*****************************************************************************
function Awake() {

	Time.timeScale = 1.0;
	Time.fixedDeltaTime = 0.02;
	
	playerWins.GetComponent(TextMesh).text = "Wins:  " + PlayerPrefs.GetInt("PlayerWins");
	playerMoney.GetComponent(TextMesh).text = "Coins: " + PlayerPrefs.GetInt("PlayerMoney");
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
		
			//Game Modes
			case "gameMode_1":
				playSfx(tapSfx);						//play touch sound
				PlayerPrefs.SetInt("GameMode", 0);		//set game mode to fetch later in "Game" scene
				animateButton(objectHit);				//touch animation effect
				yield WaitForSeconds(1.0);				//Wait for the animation to end
				Application.LoadLevel("Config");		//Load the next scene
				break;
			case "gameMode_2":
				playSfx(tapSfx);
				PlayerPrefs.SetInt("GameMode", 1);
				animateButton(objectHit);
				yield WaitForSeconds(1.0);
				Application.LoadLevel("Config");
				break;		
					
			//Option buttons	
			case "Btn-01":
				playSfx(tapSfx);
				animateButton(objectHit);
				yield WaitForSeconds(1.0);
				Application.LoadLevel("Shop");
				break;
			case "Btn-02":
				playSfx(tapSfx);
				animateButton(objectHit);
				yield WaitForSeconds(1.0);
				Application.LoadLevel("BuyCoinPack");
				break;
			case "Btn-03":
				playSfx(tapSfx);
				animateButton(objectHit);
				yield WaitForSeconds(1.0);
				Application.Quit();
				break;	
		}	
	}
}

//*****************************************************************************
// This function animates a button by modifying it's scales on x-y plane.
// can be used on any element to simulate the tap effect.
//*****************************************************************************
function animateButton(_btn : GameObject) {
	canTap = false;
	var startingScale : Vector3 = _btn.transform.localScale;	//initial scale	
	var destinationScale : Vector3 = startingScale * 1.1;		//target scale
	
	//Scale up
	var t = 0.0; 
	while (t <= 1.0) {
		t += Time.deltaTime * buttonAnimationSpeed;
		_btn.transform.localScale.x = Mathf.SmoothStep(startingScale.x, destinationScale.x, t);
		_btn.transform.localScale.y = Mathf.SmoothStep(startingScale.y, destinationScale.y, t);
		yield;
	}
	
	//Scale down
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
