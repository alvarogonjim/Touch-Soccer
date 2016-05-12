using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {
		
	//***************************************************************************//
	// This class manages pause and unpause states.
	//***************************************************************************//

	//static bool  soundEnabled;
	internal bool isPaused;
	private float savedTimeScale;
	public GameObject pausePlane;
	//public GameObject banner;

	enum Page {
		PLAY, PAUSE
	}
	private Page currentPage = Page.PLAY;

	//*****************************************************************************
	// Init.
	//*****************************************************************************
	void Awake (){		
		//soundEnabled = true;
		isPaused = false;
	//	banner.SetActive (false);
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.005f;
		
		if(pausePlane)
	    	pausePlane.SetActive(false); 
	}

	//*****************************************************************************
	// FSM
	//*****************************************************************************
	void Update (){
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
	void touchManager (){
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo)) {
				string objectHitName = hitInfo.transform.gameObject.name;
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
						Application.LoadLevel("Menu-c#");
						break;
				}
			}
		}
	}

	public void PauseGame (){
		//banner.SetActive (true);
		print("Game in Paused...");
		isPaused = true;
		savedTimeScale = Time.timeScale;
	    Time.timeScale = 0;
	    AudioListener.volume = 0;
	    if(pausePlane)
	    	pausePlane.SetActive(true);
	    currentPage = Page.PAUSE;
	}

	public void UnPauseGame (){
		//banner.SetActive (false);
		print("Unpause");
	    isPaused = false;
	    Time.timeScale = savedTimeScale;
	    AudioListener.volume = 1.0f;
		if(pausePlane)
	    	pausePlane.SetActive(false);   
	    currentPage = Page.PLAY;
	}
	public void RestartGame(){
		UnPauseGame();
		Application.LoadLevel(Application.loadedLevelName);
	}
}

