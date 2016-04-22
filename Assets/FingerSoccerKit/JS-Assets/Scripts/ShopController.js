#pragma strict

///*************************************************************************///
/// Main Shop Controller class.
/// This class handles all touch events on buttons.
/// It also checks if user has enough money to buy items, it items are already purchased,
/// and saves the purchased items into playerprefs for further usage.
///*************************************************************************///

private var buttonAnimationSpeed : float = 9;	//speed on animation effect when tapped on button
private var canTap : boolean = true;			//flag to prevent double tap

var coinsCheckout : AudioClip;					//buy sfx
var playerMoney : GameObject;					//Reference to 3d text
private var availableMoney : int;

var totalItemsForSale : GameObject[];			//Purchase status

//*****************************************************************************
// Init. 
//*****************************************************************************
function Awake() {
	//Updates 3d text with saved values fetched from playerprefs
	availableMoney = PlayerPrefs.GetInt("PlayerMoney");
	playerMoney.GetComponent(TextMesh).text = "Coins: " + availableMoney;
	
	//check if we previously purchased these items.
	for(var i : int; i < totalItemsForSale.length; i++) {
		//format the correct string we use to store purchased items into playerprefs
		var shopItemName : String = "shopItem-" + totalItemsForSale[i].GetComponent(ShopItemProperties).itemIndex.ToString();
		if(PlayerPrefs.GetInt(shopItemName) == 1) {
			//we already purchased this item
			totalItemsForSale[i].GetComponent.<Renderer>().material.color = new Color(0, 1, 0, 1); 	//Make it green
			totalItemsForSale[i].GetComponent(BoxCollider).enabled = false;			//Not clickable anymore
		}
	}
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
private var saveName : String = "";
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
		
			case "shopItem_1":
				//if we have enough money, purchase this item and save the event
				if(availableMoney >= objectHit.GetComponent(ShopItemProperties).itemPrice) {
					//animate the button
					animateButton(objectHit);
					
					//deduct the price from user money
					availableMoney -= objectHit.GetComponent(ShopItemProperties).itemPrice;
					
					//save new amount of money
					PlayerPrefs.SetInt("PlayerMoney", availableMoney);
					
					//save the event of purchase
					saveName = "shopItem-" + objectHit.GetComponent(ShopItemProperties).itemIndex.ToString();
					PlayerPrefs.SetInt(saveName, 1);
					
					//play sfx
					playSfx(coinsCheckout);
					
					//Wait
					yield WaitForSeconds(1.5);
					
					//Reload the level
					Application.LoadLevel(Application.loadedLevelName);
				}
				break;
				
			case "shopItem_2":
				if(availableMoney >= objectHit.GetComponent(ShopItemProperties).itemPrice) {
					animateButton(objectHit);
					availableMoney -= objectHit.GetComponent(ShopItemProperties).itemPrice;
					PlayerPrefs.SetInt("PlayerMoney", availableMoney);
					saveName = "shopItem-" + objectHit.GetComponent(ShopItemProperties).itemIndex.ToString();
					PlayerPrefs.SetInt(saveName, 1);
					playSfx(coinsCheckout);
					yield WaitForSeconds(1.5);
					Application.LoadLevel(Application.loadedLevelName);
				}
				break;
				
			case "shopItem_3":
				if(availableMoney >= objectHit.GetComponent(ShopItemProperties).itemPrice) {
					animateButton(objectHit);
					availableMoney -= objectHit.GetComponent(ShopItemProperties).itemPrice;
					PlayerPrefs.SetInt("PlayerMoney", availableMoney);
					saveName = "shopItem-" + objectHit.GetComponent(ShopItemProperties).itemIndex.ToString();
					PlayerPrefs.SetInt(saveName, 1);
					playSfx(coinsCheckout);
					yield WaitForSeconds(1.5);
					Application.LoadLevel(Application.loadedLevelName);
				}
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
	var startingScale : Vector3 = _btn.transform.localScale;
	var destinationScale : Vector3 = startingScale * 1.1;
	
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
