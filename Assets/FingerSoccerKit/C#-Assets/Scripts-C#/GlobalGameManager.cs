using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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

    public static string player1Name = "Player_1";
    public static string player2Name = "Player_2";
    public static string cpuName = "CPU";


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

    //PowerUps
    public static bool llamadoPowerUpTamano = false;
    public static bool powerUpTamano;
    public static int iPowerUpTamano = 2;
    public static int soloUnaVezTamano;

    public static bool llamadoPowerUpElimina = false;
    public static bool powerUpElimina;
    public static int iPowerUpElimina = 2;
    public static int soloUnaVezElimina;

    public static bool llamadoPowerUpBarrera = false;
    public static bool powerUpBarrera;
    public static int iPowerUpBarrera = 2;
    public static int soloUnaVezBarrera;

    public string nombreAnimacion;
    public GameObject ObjectToAnimateBarrera;
    public Animation animBarrera;
    public static bool estaSubida;
    public GameObject barrera;


    public AudioSource sonidoBarrera;
    public AudioSource sonidoTamano;

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
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerCountTurn.text = timeLeft.ToString();
            decimal aux = decimal.Parse(string.Format("{0:N0}", timeLeft));
            timerCountTurn.text = aux.ToString();
        }//If the time is 0 change the round --> change the turn
        else if (timeLeft <= 0 && round == 1)
        {
            round = 2;
            timeLeft = 15;
            timerCountTurn.text = timeLeft.ToString();
            roundTurnManager();
            //If the time is 0 change the round --> change the turn
        }
        else if (timeLeft <= 0 && round == 2)
        {
            round = 1;
            timeLeft = 15;
            timerCountTurn.text = timeLeft.ToString();
            roundTurnManager();
        }

        if (flagGoal == true)
        {
            StartCoroutine("GoalOcurred");
            //flagGoal = false;
        }
        //If you ever needed debug inforamtions:
        //print("GameRound: " + round + " & turn is for: " + whosTurn + " and GoalHappened is: " + goalHappened);


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
            playersTurn = false;
            opponentsTurn = true;
            //barrera.SetActive (false);
            playerController.canShoot = false;
            OpponentAI.opponentCanShoot = true;
            whosTurn = "opponent";
            //In every rounds we have to increase the timer again.
            timeLeft = 15;
            timerCountTurn.text = timeLeft.ToString();
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
        timerCountTurn.text = timeLeft.ToString();

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
        ball.transform.position = new Vector3(0, -0.81f, -0.7f);
        yield return new WaitForSeconds(1);
        ball.GetComponent<TrailRenderer>().enabled = true;

        //*** reformation of units ***//
        //Reformation for player_1
        StartCoroutine(playerAIController.GetComponent<PlayerAI>().changeFormation(PlayerAI.playerTeam, PlayerPrefs.GetInt("PlayerFormation"), 0.6f, 1));

        //if this is player-1 vs player-2 match:
        if (GlobalGameManager.gameMode == 1)
        {
            StartCoroutine(playerAIController.GetComponent<PlayerAI>().changeFormation(PlayerAI.player2Team, PlayerPrefs.GetInt("Player2Formation"), 0.6f, -1));
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

        if (seconds == 0 && minutes == 0)
        {
            gameIsFinished = true;
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

                //set the result texture
                statusTextureObject.GetComponent<Text>().text = statusModes[0];

                int playerWins = PlayerPrefs.GetInt("PlayerWins");
                int playerMoney = PlayerPrefs.GetInt("PlayerMoney");
                int playerGames = PlayerPrefs.GetInt("PlayerGames");



                if (playerGames < 20)
                {
                    //Si aun no ha jugado los 20 partidos, le sumamos 1
                    PlayerPrefs.SetInt("PlayerGames", ++playerGames);
                }
                else
                {
                    //Si ha jugado los 20 partidos vuelve a estar el contador a 0
                    PlayerPrefs.SetInt("PlayerGames", 0);
                }

                score = playerWins;
                PlayerPrefs.SetInt("PlayerWins", ++playerWins);         //add to wins counter
                PlayerPrefs.SetInt("PlayerMoney", playerMoney + 100);   //handful of coins as the prize!

                if (playerWins >= 1)
                {
                    Social.ReportProgress("CgkIqKW33aMMEAIQBA", 100.0f, (bool success) => {
                    });

                }
                Social.ReportScore(score, "CgkIqKW33aMMEAIQBQ", (bool success) => {

                });


            }
            else if (opponentGoals > goalLimit || opponentGoals > playerGoals)
            {

                print("CPU is the winner!!");
                statusTextureObject.GetComponent<Text>().text = statusModes[1];

            }
            else if (opponentGoals == playerGoals)
            {

                print("(Single Player) We have a Draw!");
                statusTextureObject.GetComponent<Text>().text = statusModes[4];
            }
        }
        else if (gameMode == 1)
        {
            if (playerGoals > opponentGoals)
            {
                print("Player 1 is the winner!!");
                statusTextureObject.GetComponent<Text>().text = statusModes[2];
            }
            else if (playerGoals == opponentGoals)
            {
                print("(Two-Player) We have a Draw!");
                statusTextureObject.GetComponent<Text>().text = statusModes[4];
            }
            else if (playerGoals < opponentGoals)
            {
                print("Player 2 is the winner!!");
                statusTextureObject.GetComponent<Text>().text = statusModes[3];
            }
        }
        NextLevelButton("Shop-c#");

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
                Debug.Log(powerUpTamano.ToString());
                //Decrementamos la habilidad
                iPowerUpTamano--;
                //La habilidad la tiene
                powerUpTamano = true;

                //SOLO UN USO DE LA HABILIDAD:
                soloUnaVezTamano = 1;
                //Ponemos el llamado de tama�o a true
                llamadoPowerUpTamano = true;
                sonidoTamano.Play();

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
                Debug.Log(powerUpElimina.ToString());
                //Decrementamos la habilidad
                iPowerUpElimina--;
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
    public void PWBarrera()
    {
        //Se ha llamado al powerup de la barrera anteriormente?
        if (llamadoPowerUpBarrera == false)
        {
            estaSubida = true;
            StartCoroutine("subeBarrera");
            //Si no vemos si tiene la habilidad disponible (mas de 0)
            if (iPowerUpBarrera > 0)
            {
                Debug.Log(powerUpBarrera.ToString());
                //Decrementamos la habilidad
                iPowerUpBarrera--;
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

}

