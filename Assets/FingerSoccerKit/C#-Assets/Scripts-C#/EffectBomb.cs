using UnityEngine;
using System.Collections;

public class EffectBomb : MonoBehaviour {

	public GameObject effectBomb;
	// Use this for initialization
	void Start () {
		effectBomb = GameObject.Find ("Bomb");

	}

	public void explota(){
		effectBomb.GetComponent<ParticleSystem> ().Play ();

	}

}
