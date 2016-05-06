using UnityEngine;
using System.Collections;

public class OnlineAI : MonoBehaviour {
    private GameObject gameController;
    private GameObject[] myTeam;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        myTeam = GameObject.FindGameObjectsWithTag("Opponent");
        //debug
        int i = 1;
        foreach (GameObject unit in myTeam)
        {
            //Optional
            unit.name = "Opponent-Player-" + i;
            unit.GetComponent<OpponentUnitController>().unitIndex = i;
            i++;
            //print("My Team: " + unit.name);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GlobalGameManager.opponentsTurn)
        {
           // Shoot(0, new Vector3(-5, -1, 0));
        }
    }

    void Shoot(int unitIndex, Vector3 force)
    {
        var shooter = myTeam[unitIndex];
        shooter.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        print(shooter.name + " shot the ball with a power of " + force.magnitude);

        StartCoroutine(gameController.GetComponent<GlobalGameManager>().managePostShoot("Opponent"));
    }

}
