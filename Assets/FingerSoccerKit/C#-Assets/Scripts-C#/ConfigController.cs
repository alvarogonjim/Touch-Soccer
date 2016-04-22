using UnityEngine;
using System.Collections;

public class ConfigController : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Config Controller.
	/// This class provides the available settings to the players, like formations,
	/// gameDuration, etc... and then prepare the main game with the setting.
	///*************************************************************************///

	private float buttonAnimationSpeed = 11;	//speed on animation effect when tapped on button
	private bool  canTap = true;				//flag to prevent double tap
	public AudioClip tapSfx;					//tap sound for buttons click


	public string[] availableFormations;		//Just the string values. We setup actual values somewhere else.
	public string[] availableTimes;				//Just the string values. We setup actual values somewhere else.
	public Sprite[] formaciones;

	//Reference to gameObjects
	public GameObject p1FormationLabel;			//UI 3d text object
	public GameObject p2FormationLabel;			//UI 3d text object
	public GameObject gameTimeLabel;			//UI 3d text object
	public GameObject formaciones1;			//UI 3d text object
	public GameObject formaciones2;			//UI 3d text object

	private int p1FormationCounter = 0;			//Actual player-1 formation index
	private int p2FormationCounter = 0;			//Actual player-2 formation index
	private int timeCounter = 0;				//Actual game-time index

	//*****************************************************************************
	// Init. Updates the 3d texts with saved values fetched from playerprefs.
	//***************************************************************************
	void Awake (){
		p1FormationLabel.GetComponent<TextMesh>().text = availableFormations[p1FormationCounter];	//loads default formation
		p2FormationLabel.GetComponent<TextMesh>().text = availableFormations[p2FormationCounter];	//loads default formation
		gameTimeLabel.GetComponent<TextMesh>().text = availableTimes[timeCounter];				//loads default game-time
		formaciones1.GetComponent<SpriteRenderer>().sprite = formaciones[p1FormationCounter];
		formaciones2.GetComponent<SpriteRenderer>().sprite = formaciones[p2FormationCounter];
	}

	//*****************************************************************************
	// FSM
	//*****************************************************************************
	void Update (){	
		if(canTap) {
			StartCoroutine(tapManager());
		}
	}

	//*****************************************************************************
	// This function monitors player touches on menu buttons.
	// detects both touch and clicks and can be used with editor, handheld device and 
	// every other platforms at once.
	//*****************************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator tapManager (){

		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
			
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			
			switch(objectHit.name) {
			
				case "p1-FBL":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));	//button scale-animation to user input
					p1FormationCounter--;		//cycle through available formation indexs for this player. This is the main index value.
					fixCounterLengths();		//when reached to the last option, start from the first index of the other side.
					p1FormationLabel.GetComponent<TextMesh>().text = availableFormations[p1FormationCounter]; //set the string on the UI
				formaciones1.GetComponent<SpriteRenderer>().sprite = formaciones[p1FormationCounter];
					break;
					
				case "p1-FBR":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					p1FormationCounter++;
					fixCounterLengths();
					p1FormationLabel.GetComponent<TextMesh>().text = availableFormations[p1FormationCounter];
				formaciones1.GetComponent<SpriteRenderer>().sprite = formaciones[p1FormationCounter];
					break;
					
				case "p2-FBL":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					p2FormationCounter--;
					fixCounterLengths();
					p2FormationLabel.GetComponent<TextMesh>().text = availableFormations[p2FormationCounter];
				formaciones2.GetComponent<SpriteRenderer>().sprite = formaciones[p2FormationCounter];
					break;
				
				case "p2-FBR":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					p2FormationCounter++;
					fixCounterLengths();
					p2FormationLabel.GetComponent<TextMesh>().text = availableFormations[p2FormationCounter];
				formaciones2.GetComponent<SpriteRenderer>().sprite = formaciones[p2FormationCounter];
					break;
					
				case "durationBtnLeft":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					timeCounter--;
					fixCounterLengths();
					gameTimeLabel.GetComponent<TextMesh>().text = availableTimes[timeCounter];
					break;
					
				case "durationBtnRight":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					timeCounter++;
					fixCounterLengths();
					gameTimeLabel.GetComponent<TextMesh>().text = availableTimes[timeCounter];
					break;
					
				case "Btn-Back":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					//No need to save anything
					Application.LoadLevel("Menu-c#");
					break;
					
				case "Btn-Start":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					//Save configurations
					PlayerPrefs.SetInt("PlayerFormation", p1FormationCounter);		//save the player-1 formation index
					PlayerPrefs.SetInt("Player2Formation", p2FormationCounter);		//save the player-2 formation index
					PlayerPrefs.SetInt("GameTime", timeCounter);					//save the game-time value
					//** Please note that we just set the indexes here. We fetch the actual index values in the <<Game>> scene.
					
					yield return new WaitForSeconds(0.5f);
					Application.LoadLevel("Game-c#");
					break;			
			}	
		}
	}

	//*****************************************************************************
	// When selection form available options, when player reaches the last option,
	// and still taps on the next option, this will cycle it again to the first element of options.
	// This is for p-1, p-2 and time settings.
	//*****************************************************************************
	void fixCounterLengths (){
		//set array counters limitations
		
		//Player-1
		if(p1FormationCounter > availableFormations.Length - 1)
			p1FormationCounter = 0;
		if(p1FormationCounter < 0)
			p1FormationCounter = availableFormations.Length - 1;
			
		//Player-2	
		if(p2FormationCounter > availableFormations.Length - 1)
			p2FormationCounter = 0;
		if(p2FormationCounter < 0)
			p2FormationCounter = availableFormations.Length - 1;
			
		//GameTime
		if(timeCounter > availableTimes.Length - 1)
			timeCounter = 0;
		if(timeCounter < 0)
			timeCounter = availableTimes.Length - 1;
	}

	//*****************************************************************************
	// This function animates a button by modifying it's scales on x-y plane.
	// can be used on any element to simulate the tap effect.
	//*****************************************************************************
	IEnumerator animateButton ( GameObject _btn  ){
		canTap = false;
		Vector3 startingScale = _btn.transform.localScale;	//initial scale	
		Vector3 destinationScale = startingScale * 1.1f;		//target scale
		
		//Scale up
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * buttonAnimationSpeed;
			_btn.transform.localScale = new Vector3( Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
			                                        Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
			                                        _btn.transform.localScale.z);
			yield return 0;
		}
		
		//Scale down
		float r = 0.0f; 
		if(_btn.transform.localScale.x >= destinationScale.x) {
			while (r <= 1.0f) {
				r += Time.deltaTime * buttonAnimationSpeed;
				_btn.transform.localScale = new Vector3( Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
				                                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
				                                        _btn.transform.localScale.z);
				yield return 0;
			}
		}
		
		if(r >= 1)
			canTap = true;
	}



	//*****************************************************************************
	// Play sound clips
	//*****************************************************************************
	void playSfx ( AudioClip _clip  ){
		GetComponent<AudioSource>().clip = _clip;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}

}