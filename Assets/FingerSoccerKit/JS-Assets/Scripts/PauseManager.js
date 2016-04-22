#pragma strict

//***************************************************************************//
// This class manages pause and unpause states.
//***************************************************************************//

static var soundEnabled : boolean;
static var isPaused : boolean;
private var savedTimeScale : float;
var pausePlane : GameObject;

enum Page {
	PLAY, PAUSE
}
private var currentPage:Page = Page.PLAY;

//*****************************************************************************
// Init.
//*****************************************************************************
function Awake() {		
	soundEnabled = true;
	isPaused = false;
	
	Time.timeScale = 1.0;
	Time.fixedDeltaTime = 0.02;
	
	if(pausePlane)
    	pausePlane.SetActive(false); 
}

//*****************************************************************************
// FSM
//*****************************************************************************
function Update() {
	//touch control
	touchManager();
	
	//optional pause in Editor & Windows (just for debug)
	if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape)) {
		//PAUSE THE GAME
		switch (currentPage) {
            case Page.PLAY: 
            	PauseGame(); 
            	break;
            case Page.PAUSE: 
            	UnPauseGame(); 
            	break;
            default: 
            	currentPage = Page.PLAY;
            	break;
        }
	}
	
	//debug restart
	if(Input.GetKeyDown(KeyCode.R)) {
		Application.LoadLevel(Application.loadedLevelName);
	}
}

//*****************************************************************************
// This function monitors player touches on menu buttons.
// detects both touch and clicks and can be used with editor, handheld device and 
// every other platforms at once.
//*****************************************************************************
function touchManager() {
	if(Input.GetMouseButtonDown(0)) {
		var hitInfo : RaycastHit;
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, hitInfo)) {
			var objectHitName : String = hitInfo.transform.gameObject.name;
			switch(objectHitName) {
				case "PauseBtn":
					switch (currentPage) {
			            case Page.PLAY: 
			            	PauseGame();
			            	break;
			            case Page.PAUSE: 
			            	UnPauseGame(); 
			            	break;
			            default: 
			            	currentPage = Page.PLAY;
			            	break;
			        }
					break;
				
				case "ResumeBtn":
					switch (currentPage) {
			            case Page.PLAY: 
			            	PauseGame();
			            	break;
			            case Page.PAUSE: 
			            	UnPauseGame(); 
			            	break;
			            default: 
			            	currentPage = Page.PLAY;
			            	break;
			        }
					break;
				
				case "RestartBtn":
					UnPauseGame();
					Application.LoadLevel(Application.loadedLevelName);
					break;
					
				case "MenuBtn":
					UnPauseGame();
					Application.LoadLevel("Menu");
					break;
			}
		}
	}
}


function PauseGame() {
	print("Game in Paused...");
	isPaused = true;
	savedTimeScale = Time.timeScale;
    Time.timeScale = 0;
    AudioListener.volume = 0;
    if(pausePlane)
    	pausePlane.SetActive(true);
    currentPage = Page.PAUSE;
}

function UnPauseGame() {
	print("Unpause");
    isPaused = false;
    Time.timeScale = savedTimeScale;
    AudioListener.volume = 1.0;
	if(pausePlane)
    	pausePlane.SetActive(false);   
    currentPage = Page.PLAY;
}

