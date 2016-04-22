#pragma strict

///*************************************************************************///
/// Main CoinPack purchase Controller.
/// This class handles all touch events on coin packs.
/// You can easily integrate your own (custom) IAB system to deliver a nice 
/// IAP options to the player.
///*************************************************************************///

private var buttonAnimationSpeed : float = 9;	//speed on animation effect when tapped on button
private var canTap : boolean = true;			//flag to prevent double tap
var coinsCheckout : AudioClip;					//purchase sound

//Reference to GameObjects
var playerMoney : GameObject;					//UI 3d text object
private var availableMoney : int;				//UI 3d text object

//*****************************************************************************
// Init. Updates the 3d texts with saved values fetched from playerprefs.
//*****************************************************************************
function Awake() {
	availableMoney = PlayerPrefs.GetInt("PlayerMoney");
	playerMoney.GetComponent(TextMesh).text = "Coins: " + availableMoney;
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
		
			case "coinPack_1":
				//Here you should implement your own in-app purchase routines.
				//But for simplicity, we add the basic functions.
				
				//** Required steps **
				//Lead the player to the in-app gate and after the purchase is done, go to next line.
				//You should open the pay gateway, make the transaction, close the gateway, get the response and then consume the purchased item.
				//Then you can grant the user access to the item.
				//For security, you can avoid having money or similar purchasable items in plant text (string) and encode them with custom hash.
				
				//animate the button
				animateButton(objectHit);
				
				//add the purchased coins to the available user money
				availableMoney += 200;
				
				//save new amount of money
				PlayerPrefs.SetInt("PlayerMoney", availableMoney);
				
				//play sfx
				playSfx(coinsCheckout);
				
				//Wait
				yield WaitForSeconds(1.5);
				
				//Reload the level
				Application.LoadLevel(Application.loadedLevelName);
				
				break;
				
			case "coinPack_2":
				animateButton(objectHit);
				availableMoney += 500;
				PlayerPrefs.SetInt("PlayerMoney", availableMoney);
				playSfx(coinsCheckout);
				yield WaitForSeconds(1.5);
				Application.LoadLevel(Application.loadedLevelName);
				break;
				
			case "coinPack_3":
				animateButton(objectHit);
				availableMoney += 2500;
				PlayerPrefs.SetInt("PlayerMoney", availableMoney);
				playSfx(coinsCheckout);
				yield WaitForSeconds(1.5);
				Application.LoadLevel(Application.loadedLevelName);
				break;
			
			case "Btn-Back":
				animateButton(objectHit);
				yield WaitForSeconds(1.0);
				Application.LoadLevel("Menu");
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
