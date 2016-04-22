using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerShoot : MonoBehaviour {

	public Text timeShootP1;
	public Text timeShootP2;
	public float tiempoP1 = 0.0f;
	public float tiempoP2 = 0.0f;

	GlobalGameManager gbManager;
	public static bool changeTurn=false;

	public void Start(){
		//tiempo = 10f;
		//countTP1 = int.Parse(tiempoP1.ToString());
		//countTP2 = int.Parse(tiempoP2.ToString ());
	}
	public void Update(){
		if (GlobalGameManager.playersTurn == true) {	

			if (tiempoP1 > 0) {
				tiempoP1 -= Time.deltaTime;
				timeShootP1.text = "" + tiempoP1.ToString ("f0");
				//countTP1 = int.Parse(timeShootP1.text);
				//} else if () {

				//gbManager.roundTurnManager ("1");
				//GlobalGameManager.playersTurn = false;
				//GlobalGameManager.opponentsTurn = true;
			}
		} else if (GlobalGameManager.opponentsTurn == true) {

			if (tiempoP2 > 0) {
				tiempoP2 -= Time.deltaTime;
				timeShootP2.text = "" + tiempoP2.ToString ("f0");
			}
		}

	}

	public void CountTimeTurn(){

	}


}