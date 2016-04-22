#pragma strict

//***************************************************************************//
// This class simulates a heart-beat animation (by modifying the scales)
// when being attached to any 3D object.
//***************************************************************************//

var intensity : float = 1.2;	//size increse
var animSpeed : float = 1.0;	//animation speed

private var animationFlag : boolean;
private var startScaleX : float;
private var startScaleY : float;
private var endScaleX : float;
private var endScaleY : float;

function Start () {
	animationFlag = true;
	startScaleX = transform.localScale.x;
	startScaleY = transform.localScale.y;
	endScaleX = startScaleX * intensity;
	endScaleY = startScaleY * intensity;
}

function Update () {
	if(animationFlag) {
		animationFlag = false;
		animatePulse(this.gameObject);
	}
}

function animatePulse(_btn : GameObject) {
	yield WaitForSeconds(0.1);
	var t = 0.0; 
	while (t <= 1.0) {
		t += Time.deltaTime * 5.5 * animSpeed;
		_btn.transform.localScale.x = Mathf.SmoothStep(startScaleX, endScaleX, t);
		_btn.transform.localScale.y = Mathf.SmoothStep(startScaleY, endScaleY, t);
		yield;
	}
	
	var r = 0.0; 
	if(_btn.transform.localScale.x >= endScaleX) {
		while (r <= 1.0) {
			r += Time.deltaTime * 2 * animSpeed;
			_btn.transform.localScale.x = Mathf.SmoothStep(endScaleX, startScaleX, r);
			_btn.transform.localScale.y = Mathf.SmoothStep(endScaleY, startScaleY, r);
			yield;
		}
	}
	
	if(_btn.transform.localScale.x <= startScaleX) {
		yield WaitForSeconds(0.2);
		animationFlag = true;
	}
}
