using UnityEngine;
using System.Collections;


public class RotateByDistance : MonoBehaviour {

	public float moveSpeed = 2f;
	float radius = 0.5f;

	void Start () 
	{
		// get sphere size from collider
		radius = GetComponent<SphereCollider>().radius;
	}


	void Update () {

		// get input
		float distance = Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime;

		// move
		transform.Translate(new Vector3(distance,0,0),Space.World);

		// rotate based on distance
		float angle=(distance*180)/(radius*Mathf.PI);
		transform.eulerAngles += new Vector3(0,0,-angle);

	
	}
}