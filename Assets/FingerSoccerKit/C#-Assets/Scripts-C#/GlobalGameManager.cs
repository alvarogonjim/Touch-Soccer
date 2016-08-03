using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;

public class GlobalGameManager : MonoBehaviour
{

    ///*************************************************************************///
    /// Main Game Controller.
    /// This class controls main aspects of the game like rounds, levels, scores and ...
    /// Please note that the game always happens between 2 player: (Player-1 vs Player-2) or (Player-1 vs AI)
    /// Player-2 and AI are the same in some aspects like when they got their turns, but they use different controllers.
    /// Player-2 uses a similar controller as Player-1, while AI uses an artificial intelligent routine to play the game.
    ///
    /// Important! All units and ball object inside the game should be fixed at Z=-0.5f positon at all times. 
    /// You can do this with RigidBody's freeze position.
    ///*************************************************************************///

    public static string player1Name = "Adan";
    public static string player2Name = "Sara";
    public static string cpuName = "Amanda";


    // Available Game Modes:
    /*
	Indexes:
	0 = 1 player against cpu
	1 = 2 player against each other on the same platform/device
	*/
    public static int gameMode;
    //Online
    public static bool amIPlayerOne;

    //Odd rounds are player (Player-1) turn and Even rounds are AI (Player-2)'s
    public static int round;

    //mamixmu distance that players can drag away from selected unit to shoot the ball (is in direct relation with shoot power)
    public static float maxDistance = 3.0f;

    //Turns in flags
    public static bool playersTurn;
    public static bool opponentsTurn;

    //After players did their shoots, the round changes after this amount of time.
    public static float timeStepToAdvanceRound = 3;

    //Special occasions
    public static bool goalHappened;
    public static bool gameIsFinished;
    public static int goalLimit = 5; //To finish the game quickly, without letting the GameTime end.

    ///Game timer vars
    public static float gameTimer; //in seconds
    private string remainingTime;
    private int seconds;
    private int minutes;

    //Game Status
    static int playerGoals;
    static int opponentGoals;
    public static float gameTime; //Main game timer (in seconds). Always fixed.

    //gameObject references
    private GameObject playerAIController;
    private GameObject opponentAIController;
    private GameObject ball;

    //AudioClips
    public AudioClip startWistle;
    public AudioClip finishWistle;
    public AudioClip[] goalSfx;
    public AudioClip[] goalHappenedSfx;
    public AudioClip[] crowdChants;
	public AudioClip finalPartido;

    private bool canPlayCrowdChants;

    //Public references
    public GameObject gameStatusPlane;      //user to show win/lose result at the end of match
    public Text statusTextureObject;    //plane we use to show the result texture in 3d world
    public string[] statusModes;         //Available status textures

    //Timer
    public float timeLeft = 15.0f;
    public Text timerCountTurn;
    //Scores
    private long score;

    //Doble gol
    private bool fueGol = false;

    //Animation of goal
    public Animation AnimGoal;
    public GameObject ObjectToAnimate;
    public string nombreAni;
    public static bool flagGoal;

	//Animation finishgame
	public Animation AnimFinish;
	public GameObject ObjectToAnimateFinish;
	public string nombreAniFinish;
	public static bool flagFinish = false;


    //PowerUps
    public static bool llamadoPowerUpTamano = false;
    public static bool powerUpTamano;
    public static int iPowerUpTamano;
    public static int soloUnaVezTamano;

    public static bool llamadoPowerUpElimina = false;
    public static bool powerUpElimina;
    public static int iPowerUpElimina;
    public static int soloUnaVezElimina;

    public static bool llamadoPowerUpTurnoExtra = false;
    public static bool powerUpTurnoExtra;
    public static int iPowerUpTurnoExtra;
    public static int soloUnaVezTurnoExtra;

    //volumen del sonido al final del partido
    private float volumen =0.6f;
	private float reducir = 0.0f;
	public static bool ok = false;
	public int segundos = 1;

    public static bool llamadoPowerUpBarrera = false;
    public static bool powerUpBarrera;
    public static int iPowerUpBarrera;
    public static int soloUnaVezBarrera;
	public static int contadorTurnoBarrera = 2;

    public string nombreAnimacion;
    public GameObject ObjectToAnimateBarrera;
    public Animation animBarrera;
    public static bool estaSubida;
    public GameObject barrera;

	public GameObject[] playerTeam2;

    public AudioSource sonidoBarrera;

	public GameObject GoldIcon;
	public GameObject Rewards;
	public GameObject ParticleWin;

    GameObject myButton;
    private string liga;
    //*****************************************************************************
    // Init. 
    //*****************************************************************************
    void Awake()
    {
        //init
        liga = MenuController.getPlayerLiga();
        goalHappened = false;
        gameIsFinished = false;
        playerGoals = 0;
        opponentGoals = 0;
        gameTime = 0;
        round = 1;
        seconds = 0;
        minutes = 0;
        canPlayCrowdChants = true;
        estaSubida = false;
        cambiarCampo();


		int index = PlayerPrefs.GetInt("Skin");
		Sprite mat = Resources.Load(index.ToString(), typeof(Sprite)) as Sprite;
		GameObject.Find ("EscudoJugador1").GetComponent<Image> ().sprite = mat;


        iPowerUpTamano=PlayerPrefs.GetInt("Agrandar");
        iPowerUpElimina=PlayerPrefs.GetInt("Eliminar");
        iPowerUpBarrera=PlayerPrefs.GetInt("Barrera");

        GameObject.Find("DisponibleAgrandar").GetComponent<Text>().text = iPowerUpTamano.ToString();
//        GameObject.Find("DisponibleEliminar").GetComponent<Text>().text = iPowerUpElimina.ToString();
        GameObject.Find("DisponibleBarrera").GetComponent<Text>().text = iPowerUpBarrera.ToString();

  //hide gameStatusPlane
        gameStatusPlane.SetActive(false);

        //Translate gameTimer index to actual seconds
        switch (PlayerPrefs.GetInt("GameTime"))
        {
            case 0:
                gameTimer = 180;
                break;
            case 1:
                gameTimer = 300;
                break;
            case 2:
                gameTimer = 480;
                break;

                //You can add more cases and options here.
        }


        //Get Game Mode
        if (PlayerPrefs.HasKey("GameMode"))
            gameMode = PlayerPrefs.GetInt("GameMode");
        else
            gameMode = 0; // Deafault Mode (Player-1 vs AI)


        PlayerPrefs.DeleteKey("GameMode");

        round = gameMode == 2 ? 0 : 1; //Si es online, round empieza en 0
        
        playerAIController = GameObject.FindGameObjectWithTag("playerAI");
        opponentAIController = GameObject.FindGameObjectWithTag("opponentAI");
        ball = GameObject.FindGameObjectWithTag("ball");

        manageGameModes();
    }



    //*****************************************************************************
    // We have all units inside the game scene by default, but at the start of the game,
    // we check which side in playing (should be active) and deactive the side that is
    // not playing by deactivating all it's units.
    //*****************************************************************************
    private GameObject[] player2Team;   //array of all player-2 units in the game
    private GameObject[] cpuTeam;       //array of all AI units in the game
    void manageGameModes()
    {
        switch (gameMode)
        {
            case 0:
                //find and deactive all player2 units. This is player-1 vs AI.
                player2Team = GameObject.FindGameObjectsWithTag("Player_2");
                foreach (GameObject unit in player2Team)
                {
                    unit.SetActive(false);
                }


                break;

            case 1:
                //find and deactive all AI Opponent units. This is Player-1 vs Player-2.
                cpuTeam = GameObject.FindGameObjectsWithTag("Opponent");
                foreach (GameObject unit in cpuTeam)
                {
                    unit.SetActive(false);
                }
                //deactive opponent's AI
                opponentAIController.SetActive(false);
                break;

            case 2:
                //find and deactive all AI Opponent units. This is online
                cpuTeam = GameObject.FindGameObjectsWithTag("Opponent");
                foreach (GameObject unit in cpuTeam)
                {
                    unit.SetActive(false);
                }
                //deactive opponent's AI
                opponentAIController.SetActive(false);
                break;
        }
    

        }
    

    IEnumerator Start()
    {
		if (GlobalGameManager.gameMode == 1)
		{
			StartCoroutine(playerAIController.GetComponent<PlayerAI>().changeFormation(GameObject.FindGameObjectsWithTag("Player_2"),  PlayerPrefs.GetInt("PlayerFormation"), 0.6f, -1));
		}


        //AnimGoal = GetComponent<Animation> ();
        roundTurnManager();
        yield return new WaitForSeconds(1.5f);
        playSfx(startWistle);


    }

    //*****************************************************************************
    // FSM
    //*****************************************************************************
    void Update()
    {
      if (gameMode == 2)
        {
            UpdateOnlineMode();
            return;
        }
        //check game finish status every frame
        if (!gameIsFinished)
        {
            manageGameStatus();
        }


        //every now and then, play some crowd chants
        StartCoroutine(playCrowdChants());

        //Debug.Log (timeLeft);


        //Countdown
		if (timeLeft > 0 && round == 1)
        {
            timeLeft -= Time.deltaTime;
			float i = timeLeft / 15;
			GameObject.Find ("TimeBar1").GetComponent < Scrollbar > ().size = i;




           
           
        }

		else if (timeLeft > 0 && round == 2)
		{
			timeLeft -= Time.deltaTime;
			float i = timeLeft / 15;
			GameObject.Find ("TimeBar2").GetComponent < Scrollbar > ().size = i;






		}

		//If the time is 0 change the round --> change the turn
        else if (timeLeft <= 0 && round == 1)
        {

            if(llamadoPowerUpTurnoExtra == true)
            {
                round = 1;
                timeLeft = 15;
                GameObject.Find("TimeBar1").GetComponent<Scrollbar>().size = 1;
                roundTurnManager();
            }

            round = 2;
            timeLeft = 15;
			GameObject.Find ("TimeBar1").GetComponent < Scrollbar > ().size = 1;
            roundTurnManager();
         
            
            
            //If the time is 0 change the round --> change the turn
        }
        else if (timeLeft <= 0 && round == 2)
        {
            round = 1;
            timeLeft = 15;
			GameObject.Find ("TimeBar2").GetComponent < Scrollbar > ().size = 1;
            roundTurnManager();
        }

        if (flagGoal == true)
        {
            StartCoroutine("GoalOcurred");
            //flagGoal = false;
        }

		if (flagFinish == true) {
			StartCoroutine("partidoFinaliza");
		}

		if (contadorTurnoBarrera == 0) {
			barrera.SetActive (false);
		}

        //If you ever needed debug inforamtions:
        //print("GameRound: " + round + " & turn is for: " + whosTurn + " and GoalHappened is: " + goalHappened);

		if (ok) {
			reducir += Time.deltaTime / segundos;
			volumen = Mathf.Lerp (0.5f, 1.0f, reducir);
		} else {
			volumen = 1.0f;
			reducir = 0f;
		}
		GetComponent<AudioSource>().volume = volumen;
    }

    void UpdateOnlineMode()
    {
        
        //check game finish status every frame
        if (!gameIsFinished)
        {
            manageGameStatus();
        }

        //every now and then, play some crowd chants
        StartCoroutine(playCrowdChants());

        if (flagGoal == true)
        {
            StartCoroutine("GoalOcurred");
            //flagGoal = false;
        }
		if (flagFinish == true) {
			StartCoroutine("partidoFinaliza");
		}
    }

    //*****************************************************************************
    // This function gives turn to players in the game.
    //*****************************************************************************
    public static string whosTurn;
    void roundTurnManager()
    {

        if (gameMode == 2)
        {
            //Modo online tenemos método aparte
            roundTurnManagerOnline();
            return;
        }


        if (gameIsFinished || goalHappened)
            return;

        //if round number is odd, it's players turn, else it's AI or player-2 's turn
        int carry;
        carry = round % 2;
        if (carry == 1)
        {
			if (llamadoPowerUpBarrera == true) {
				contadorTurnoBarrera--;
			}
            playersTurn = true;
            opponentsTurn = false;
            playerController.canShoot = true;
            OpponentAI.opponentCanShoot = false;
            whosTurn = "player";
            fueGol = false;
            timeLeft = 15;
        }//This else if is because the opponent can shot two times.
        else if (carry == 0 && opponentsTurn == true && OpponentAI.opponentCanShoot == true)
        {
            round = 1;
            opponentsTurn = false;
            OpponentAI.opponentCanShoot = false;
            //In every rounds we have to increase the timer again.
            fueGol = false;
            timeLeft = 15;
            timerCountTurn.text = timeLeft.ToString();
            //Si en el turno se ha llamado a powerup de tama�o decrementamos la variable contador
            if (llamadoPowerUpTamano == true)
            {
                Debug.Log(playerController.contadorPowerUpTamano);
                playerController.contadorPowerUpTamano--;
            }

            if (llamadoPowerUpElimina == true)
            {
                Debug.Log(playerController.contadorPowerUpElimina);
                playerController.contadorPowerUpElimina--;
            }

            if (llamadoPowerUpBarrera == true)
            {
                Debug.Log(playerController.contadorPowerUpBarrera);
                playerController.contadorPowerUpBarrera--;
            }
        }

        else
        {
			if (llamadoPowerUpBarrera == true) {
				contadorTurnoBarrera--;
			}
            playersTurn = false;
            opponentsTurn = true;
            //barrera.SetActive (false);
            playerController.canShoot = false;
            OpponentAI.opponentCanShoot = true;
            whosTurn = "opponent";
            //In every rounds we have to increase the timer again.
            timeLeft = 15;
            //Si en el turno se ha llamado a powerup de tama�o decrementamos la variable contador
            Debug.Log(llamadoPowerUpTamano);
            if (llamadoPowerUpTamano == true)
            {
                Debug.Log(llamadoPowerUpTamano);
                Debug.Log(playerController.contadorPowerUpTamano);
                playerController.contadorPowerUpTamano = playerController.contadorPowerUpTamano - 1;

            }

            if (llamadoPowerUpElimina == true)
            {
                Debug.Log(llamadoPowerUpElimina);
                Debug.Log(playerController.contadorPowerUpElimina);
                playerController.contadorPowerUpElimina = playerController.contadorPowerUpElimina - 1;

            }
        }
        if (llamadoPowerUpBarrera == true)
        {
            Debug.Log(playerController.contadorPowerUpBarrera);
            playerController.contadorPowerUpBarrera--;

        }
        //Override
        //for two player game, players can always shoot.
        //we override this because both human players play on the same device and must be able to shoot at every turn.
        //we just limit their actions to their own units.
        if (gameMode == 1)
            playerController.canShoot = true;
    }

    void roundTurnManagerOnline()
    {
        if (gameIsFinished || goalHappened)
        {
            return;
        }

        if (round == 0)
        {
            //Primer turno
            if (amIPlayerOne)
            {
                round = 1;
            }
            else
            {
                round = 2;
            }
            roundTurnManagerOnline();
        }
        else if (round == 1)
        {
            playerController.canShoot = true;
            round = 2;
        }
        else if (round == 2)
        {
            playerController.canShoot = false;
            round = 1;
        }
    }

    //*****************************************************************************
    // What happens after a shoot is performed?
    //*****************************************************************************
    public IEnumerator managePostShoot(string _shootBy, int unitIndex, Vector3 outPower)
    {
		ConsoleScreen.Log ("shoot " + outPower);
        //get who is did the shoot
        //if we had a goal after the shoot was done and just before the round change, leave the process to other controllers.

        timeLeft = 15;
        

        float t = 0;
        while (t < timeStepToAdvanceRound)
        {
            t += Time.deltaTime;
            if (goalHappened)
            {
                yield break;
            }
            yield return 0;
        }

        //we had a simple shoot with no goal result
        if (t >= timeStepToAdvanceRound)
        {
            //add to round counters
            switch (_shootBy)
            {
                case "Player":
                    round = 2;
                    break;
                case "Player_2":
                    round = 1;
                    break;
                case "Opponent":
                    round = 1;
                    break;
            }

            //TODO-REE Online 
            if (gameMode == 2)
            {
                SoccerRealTimeMultiplayerListener.Instance.SendShoot(unitIndex, outPower);
            }

            roundTurnManager(); //cycle again between players
        }
    }

    //*****************************************************************************
    // If we had a goal in this round, this is the function that manages all aspects of it.
    //*****************************************************************************								
    public IEnumerator managePostGoal(string _goalBy)
    {
        //get who did the goal.

        //soft pause the game for reformation and other things...
        goalHappened = true;
        flagGoal = true;

        //add to goal counters
        switch (_goalBy)
        {

            case "Player":
                if (fueGol == false)
                {
                    playerGoals++;
                    round = 2; //goal by player-1 and opponent should start the next round
                    fueGol = true;
                }
                break;

            case "Opponent":
                if (fueGol == false)
                {
                    opponentGoals++;
                    round = 1; //goal by opponent and player-1 should start the next round
                    fueGol = true;
                }
                break;
        }

        //wait a few seconds to show the effects , and physics cooldown
        playSfx(goalSfx[Random.Range(0, goalSfx.Length)]);
        GetComponent<AudioSource>().PlayOneShot(goalHappenedSfx[Random.Range(0, goalHappenedSfx.Length)], 1);
        yield return new WaitForSeconds(1);

        //bring the ball back to it's initial position
        ball.GetComponent<TrailRenderer>().enabled = false;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.position = new Vector3(-0.12f, 1.33f, -0.5f);
        yield return new WaitForSeconds(1);
        ball.GetComponent<TrailRenderer>().enabled = true;

        //*** reformation of units ***//
        //Reformation for player_1
        StartCoroutine(playerAIController.GetComponent<PlayerAI>().changeFormation(PlayerAI.playerTeam, PlayerPrefs.GetInt("PlayerFormation"), 0.6f, 1));

        //if this is player-1 vs player-2 match:
        if (GlobalGameManager.gameMode == 1)
        {
			StartCoroutine(playerAIController.GetComponent<PlayerAI>().changeFormation(GameObject.FindGameObjectsWithTag("Player_2"),  PlayerPrefs.GetInt("PlayerFormation"), 0.6f, -1));
        }
        else
        {   //if this is player-1 vs AI match:
            //get a new random formation everytime
            StartCoroutine(opponentAIController.GetComponent<OpponentAI>().changeFormation(Random.Range(0, FormationManager.formations), 0.6f));
        }

        yield return new WaitForSeconds(3);

        //check if the game is finished or not
        if (playerGoals > goalLimit || opponentGoals > goalLimit)
        {
            gameIsFinished = true;
			flagFinish = true;
            manageGameFinishState();
            yield break;
        }

        //else, continue to the next round
        goalHappened = false;
        flagGoal = false;
        roundTurnManager();
        playSfx(startWistle);
    }

    //***************************************************************************//
    // Game status manager
    //***************************************************************************//
    public Text timeText;                   //UI text 
    public Text playerGoalsText;            //UI text
    public Text opponentGoalsText;          //UI text
    public Text playerOneName;              //UI text 
    public Text playerTwoName;              //UI text 
    void manageGameStatus()
    {
        seconds = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) % 60;
        minutes = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) / 60;

		if (seconds == 50 && minutes == 2) {
			ok = true;
			playSfx (finalPartido);
		}

        if (seconds == 0 && minutes == 0)
        {
            gameIsFinished = true;
			flagFinish = true;
            manageGameFinishState();
        }

        remainingTime = string.Format("{0:00} : {1:00}", minutes, seconds);
        timeText.text = remainingTime.ToString();

        playerGoalsText.text = playerGoals.ToString();
        opponentGoalsText.text = opponentGoals.ToString();

        if (gameMode == 0)
        {
            playerOneName.text = player1Name;
            playerTwoName.text = cpuName;
        }
        else if (gameMode == 1)
        {
            playerOneName.text = player1Name;
            playerTwoName.text = player2Name;
        }
    }

    //*****************************************************************************
    // After the game is finished, this function handles the events.
    //*****************************************************************************
    void manageGameFinishState()
    {
        //Play gameFinish wistle
        playSfx(finishWistle);
        print("GAME IS FINISHED.");

        //show gameStatusPlane
        gameStatusPlane.SetActive(true);

        //for single player game, we should give the player some bonuses in case of winning the match
        if (gameMode == 0)
        {
            if (playerGoals > goalLimit || playerGoals > opponentGoals)
            {
				
                print("Player 1 is the winner!!");

                //ENVIAMOS EL DATO A GOOGLE
               // PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQBg", 1);

                //set the result texture
                statusTextureObject.GetComponent<Text>().text = statusModes[0];

                int playerMoney = PlayerPrefs.GetInt("PlayerMoney");
                playerMoney = playerMoney + 200;
				Rewards.GetComponent<Text> ().text = "+ 200";
                PlayerPrefs.SetInt("PlayerMoney", playerMoney);



            }
            else if (opponentGoals > goalLimit || opponentGoals > playerGoals)
            {
                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQBw", 1);


                print("CPU is the winner!!");
				GoldIcon.SetActive(false);
				ParticleWin.SetActive(false);
				Rewards.GetComponent<Text> ().text = " ";
                statusTextureObject.GetComponent<Text>().text = statusModes[1];

            }
            else if (opponentGoals == playerGoals)
            {

                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQCg", 1);
                print("(Single Player) We have a Draw!");
				GoldIcon.SetActive(false);
				ParticleWin.SetActive(false);
				Rewards.GetComponent<Text> ().text = " ";
                statusTextureObject.GetComponent<Text>().text = statusModes[4];
            }
        }
        else if (gameMode == 1)
        {
            if (playerGoals > opponentGoals)
            {
                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQBg", 1);
                print("Player 1 is the winner!!");
                statusTextureObject.GetComponent<Text>().text = statusModes[2];

            }
            else if (playerGoals == opponentGoals)
            {
                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQCg", 1);
                print("(Two-Player) We have a Draw!");

                statusTextureObject.GetComponent<Text>().text = statusModes[4];
            }
            else if (playerGoals < opponentGoals)
            {

                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQBw", 1);
                print("Player 2 is the winner!!");
                statusTextureObject.GetComponent<Text>().text = statusModes[3];
            }
        }
       

    }
    //*****************************************************************************
    // Play a random crown sfx every now and then to spice up the game
    //*****************************************************************************
    IEnumerator playCrowdChants()
    {
        if (canPlayCrowdChants)
        {
            canPlayCrowdChants = false;
            GetComponent<AudioSource>().PlayOneShot(crowdChants[Random.Range(0, crowdChants.Length)], 1);
            yield return new WaitForSeconds(Random.Range(15, 35));
            canPlayCrowdChants = true;
        }
    }
    //*****************************************************************************
    // Play sound clips
    //*****************************************************************************
    void playSfx(AudioClip _clip)
    {
        GetComponent<AudioSource>().clip = _clip;
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    //***************************************************************
    //Metodos Utiles
    //***************************************************************

    public void NextLevelButton(int index)
    {
        Application.LoadLevel(index);
    }

    public void NextLevelButton(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void Exit()
    {
        Application.Quit();
    }


    //***************************************************************
    //Animacion de Gol
    //***************************************************************


    IEnumerator GoalOcurred()
    {
        flagGoal = false;
        AnimGoal.CrossFade(nombreAni);
        yield return new WaitForSeconds(AnimGoal[nombreAni].length);
        Debug.Log(flagGoal);


        //		GameObject Camera = 
        //		GameObject.Find ("Camera");
        //		Animation AnimGoal = Camera.GetComponent<Animation> ();
        //		AnimGoal.Play();
        //		//flagGoal = false;
        //		//AnimGoal.CrossFade("AnimGoal");
        //		yield return new WaitForSeconds (AnimGoal.clip.length);
        //		Debug.Log (flagGoal);
        //		//goalHappened =! goalHappened;
    }
    //***************************************************************
    //PowerUps
    //***************************************************************

    //Power up Tamano
    public void PWTamano()
    {
        //Se ha llamado al powerup del tama�o anteriormente?
        if (llamadoPowerUpTamano == false)
        {
            //Si no vemos si tiene la habilidad disponible (mas de 0)
            if (iPowerUpTamano > 0)
            {
                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQCw", 1);
                Debug.Log(powerUpTamano.ToString());
                //Decrementamos la habilidad
                iPowerUpTamano--;
                GameObject.Find("DisponibleAgrandar").GetComponent<Text>().text = iPowerUpTamano.ToString();
                //La habilidad la tiene
                powerUpTamano = true;

                //SOLO UN USO DE LA HABILIDAD:
                soloUnaVezTamano = 1;
                //Ponemos el llamado de tama�o a true
                llamadoPowerUpTamano = true;
               
            }
            //En caso contrario falso
            else
            {
                powerUpTamano = false;
            }
        }
    }

    public void PWElimina()
    {
        //Se ha llamado al powerup del elimina anteriormente?
        if (llamadoPowerUpElimina == false)
        {
            //Si no vemos si tiene la habilidad disponible (mas de 0)
            if (iPowerUpElimina > 0)
            {

                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQDA", 1);

                Debug.Log(powerUpElimina.ToString());
                //Decrementamos la habilidad
                iPowerUpElimina--;
                GameObject.Find("DisponibleEliminar").GetComponent<Text>().text = iPowerUpElimina.ToString();
                //La habilidad la tiene
                powerUpElimina = true;

                //SOLO UN USO DE LA HABILIDAD:
                soloUnaVezElimina = 1;
                //Ponemos el llamado de elimina a true
                llamadoPowerUpElimina = true;

            }
            //En caso contrario falso
            else
            {
                powerUpElimina = false;
            }
        }
    }


    public void PWTurnoExtra()
    {
        //Se ha llamado al powerup del elimina anteriormente?
        if (llamadoPowerUpTurnoExtra == false)
        {
            //Si no vemos si tiene la habilidad disponible (mas de 0)
            if (iPowerUpTurnoExtra > 0)
            {

                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQDA", 1);

                Debug.Log(powerUpTurnoExtra.ToString());
                //Decrementamos la habilidad
                iPowerUpTurnoExtra--;
                GameObject.Find("DisponibleTurno").GetComponent<Text>().text = iPowerUpTurnoExtra.ToString();
                //La habilidad la tiene
                powerUpTurnoExtra = true;

                //SOLO UN USO DE LA HABILIDAD:
                soloUnaVezTurnoExtra = 1;
                //Ponemos el llamado de elimina a true
                llamadoPowerUpTurnoExtra = true;

            }
            //En caso contrario falso
            else
            {
                powerUpTurnoExtra = false;
            }
        }
    }


    public void PWBarrera()
    {
        //Se ha llamado al powerup de la barrera anteriormente?
        if (llamadoPowerUpBarrera == false)
        {

            //Si no vemos si tiene la habilidad disponible (mas de 0)
            if (iPowerUpBarrera > 0)
            {
                estaSubida = true;
                StartCoroutine("subeBarrera");
                //PlayGamesPlatform.Instance.Events.IncrementEvent("CgkIqKW33aMMEAIQDQ", 1);

                Debug.Log(powerUpBarrera.ToString());
                //Decrementamos la habilidad
                iPowerUpBarrera--;
                GameObject.Find("DisponibleBarrera").GetComponent<Text>().text = iPowerUpBarrera.ToString();
                //La habilidad la tiene
                powerUpBarrera = true;

                //SOLO UN USO DE LA HABILIDAD:
                soloUnaVezBarrera = 1;
                //Ponemos el llamado de barrera a true
                llamadoPowerUpBarrera = true;

            }
            //En caso contrario falso
            else
            {
                powerUpBarrera = false;

            }
        }
    }

    IEnumerator subeBarrera()
    {
        if (estaSubida == true)
        {
            animBarrera.CrossFade(nombreAnimacion);
            sonidoBarrera.Play();
            yield return new WaitForSeconds(animBarrera[nombreAnimacion].length);

        }
        estaSubida = false;
    }

	IEnumerator partidoFinaliza(){
		if (flagFinish == true) {
			flagFinish = false;
			AnimFinish.CrossFade (nombreAniFinish);
			yield return new WaitForSeconds (AnimFinish [nombreAniFinish].length);
		}
		//flagFinish = false;
	}
    public void cambiarCampo()
    {

        int index = PlayerPrefs.GetInt("Campos");
        Texture2D mat = Resources.Load("Campos/"+index.ToString(), typeof(Texture2D)) as Texture2D;

        GameObject campo=GameObject.Find("Background");
        campo.GetComponent<Renderer>().material.SetTexture("_MainTex", mat);

    }

}

