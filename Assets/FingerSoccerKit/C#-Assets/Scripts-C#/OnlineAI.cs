using UnityEngine;
using System.Collections;

public class OnlineAI : MonoBehaviour {
    private GameObject gameController;
    private GameObject[] myTeam;

    private bool testShoot = true;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        myTeam = GameObject.FindGameObjectsWithTag("Player_2");
        //debug
        int i = 1;
        foreach (GameObject unit in myTeam)
        {
            //Optional
            unit.name = "Online-Player-" + i;
            unit.GetComponent<playerController>().unitIndex = i;
            i++;
            //print("My Team: " + unit.name);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (testShoot && Time.deltaTime > 0.3)
        {
            testShoot = true;
            Shoot(0, new Vector3(-40, -1, 0));
        }
    }

    public void Shoot(int unitIndex, Vector3 force)
    {
        var shooter = myTeam[unitIndex];
        shooter.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        print(shooter.name + " shot the ball with a power of " + force.magnitude);

        StartCoroutine(gameController.GetComponent<GlobalGameManager>().managePostShoot("Opponent", unitIndex, force));
    }

}
