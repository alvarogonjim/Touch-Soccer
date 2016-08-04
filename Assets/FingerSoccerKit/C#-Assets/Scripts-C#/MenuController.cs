using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;
using GooglePlayGames.BasicApi.Multiplayer;
using System;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
	        
	    ///*************************************************************************///
	    /// Main Menu Controller.
	    /// This class handles all touch events on buttons, and also updates the 
	    /// player status (wins and available-money) on screen.
	    ///*************************************************************************///

	    private float buttonAnimationSpeed = 9;        //speed on animation effect when tapped on button
	    private bool  canTap = true;                //flag to prevent double tap
	                                                //    public AudioClip tapSfx;                            //tap sound for buttons click

	    //Reference to GameObjects
	    //    public GameObject playerWins;                    //UI 3d text object
	    //    public GameObject playerMoney;                  //UI 3d text object
	    //    public GameObject banner;                                               //*****************************************************************************
	    // Init. Updates the 3d texts with saved values fetched from playerprefs.
	    //*****************************************************************************
	    private string name;
	    private Texture2D imagen;
	    
	    public AudioSource audioMute;
	    public static bool toggle = false;
		
	    void Awake() {
				//PlayerPrefs.DeleteKey ("PlayerMoney");
				int dinero = PlayerPrefs.GetInt ("PlayerMoney");
		        PlayGamesPlatform.Activate();
		        PlayGamesPlatform.DebugLogEnabled = true;
		        Time.timeScale = 1.0f;
		        Time.fixedDeltaTime = 0.005f;
//		GameObject.Find ("Dinero").GetComponent<Text> ().text = dinero.ToString();
						

				Debug.Log (dinero);
				
		        int playerGames = PlayerPrefs.GetInt("PlayerGames");
		       // playerWins.GetComponent<TextMesh>().text = "Wins:  " + PlayerPrefs.GetInt("PlayerWins");
		        //playerMoney.GetComponent<TextMesh>().text = "Coins: " + PlayerPrefs.GetInt("PlayerMoney");
		    }

	    //*****************************************************************************
	    // FSM
	    //*****************************************************************************


	    /// <summary>
        /// 
        /// </summary>
        void Start()
	    {
		        ((PlayGamesPlatform)Social.Active).Authenticate((bool success) => { }, true);
		        //banner.SetActive (true);
		        audioMute = GetComponent<AudioSource>();
		        if (Social.localUser.authenticated)
			        {
            name = Social.localUser.userName;
            GameObject.Find("NombreJugador").GetComponent<Text>().text = name;
            imagen = Social.localUser.image;
            Rect rect = new Rect(0, 0, imagen.width, imagen.height);
            Sprite sprite = Sprite.Create(imagen,rect,new Vector2(0.5f, 0.5f));
            GameObject.Find("ImagenJugador").GetComponent<Image>().sprite = sprite;


            //Necesitamos el ID del Leaderboard y probar que funciona correctamente.
          /* PlayGamesPlatform.Instance.LoadScores(
                   "CgkIqKW33aMMEAIQFQ",
                   LeaderboardStart.TopScores,
                   10,
                   LeaderboardCollection.Public,
                   LeaderboardTimeSpan.Daily,
                   (LeaderboardScoreData data) =>
                   {

                       for (int i = 0; i < 10; i++)
                       {

                            IScore score = data.PlayerScore;
                           string id = data.PlayerScore.userID;
                           IUserProfile user =  FindUsers(Social.LoadUsers(), id);
                           string name = user.userName;
                           string image = user.image;
                           
                       }
                   });
                   
            */
        }      

		    }

	    void Update (){

        if (Social.localUser.authenticated)
        {
            name = Social.localUser.userName;
            GameObject.Find("NombreJugador").GetComponent<Text>().text = name;
            imagen = Social.localUser.image;
            Rect rect = new Rect(0, 0, imagen.width, imagen.height);
            Sprite sprite = Sprite.Create(imagen, rect, new Vector2(0.5f, 0.5f));
            GameObject.Find("ImagenJugador").GetComponent<Image>().sprite = sprite;
			GameObject.Find("ImagenJugadorPerfil").GetComponent<Image>().sprite = sprite;
            GameObject.Find("Estadisticas").GetComponent<Text>().text = "Victorias: " + PlayerPrefs.GetInt("PlayerWin").ToString() +
                " \n Derrotas: " + PlayerPrefs.GetInt("PlayerLoses").ToString() +
                " \n Goles: " + PlayerPrefs.GetInt("PlayerGoals").ToString() +
                " \n Porcentajes de Victorias: " + (PlayerPrefs.GetInt("PlayerWin") / PlayerPrefs.GetInt("PlayerLoses")).ToString();
        }

        if (canTap) {
			            StartCoroutine(tapManager());
			        }
		    }
	    //*****************************************************************************
	    // This function monitors player touches on menu buttons.
	    // detects both touch and clicks and can be used with editor, handheld device and 
	    // every other platforms at once.
	    //*****************************************************************************
	    private RaycastHit hitInfo;
	    private Ray ray;
	    IEnumerator tapManager (){

		        //Mouse of touch?
		        if(    Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		        else if(Input.GetMouseButtonUp(0))
			            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		        else
			            yield break;
		            
		        if (Physics.Raycast(ray, out hitInfo)) {
			            GameObject objectHit = hitInfo.transform.gameObject;
			            switch(objectHit.name) {
			            
			                //Game Modes
			                case "gameMode_1":
				                //    playSfx(tapSfx);                            //play touch sound
				                    PlayerPrefs.SetInt("GameMode", 0);            //set game mode to fetch later in "Game" scene
				                    StartCoroutine(animateButton(objectHit));    //touch animation effect
				                    yield return new WaitForSeconds(1.0f);        //Wait for the animation to end
				                    Application.LoadLevel("Config-c#");            //Load the next scene
				                    break;
			                case "gameMode_2":
				                //    playSfx(tapSfx);
				                    PlayerPrefs.SetInt("GameMode", 1);
				                    StartCoroutine(animateButton(objectHit));
				                    yield return new WaitForSeconds(1.0f);
				                    Application.LoadLevel("Config-c#");
				                    break;        
				                        
				                //Option buttons    
			                case "Btn-01":
				                //    playSfx(tapSfx);
				                    StartCoroutine(animateButton(objectHit));
				                    yield return new WaitForSeconds(1.0f);
				                    Application.LoadLevel("Shop-c#");
				                    break;
			                case "Btn-02":
				                //    playSfx(tapSfx);
				                    StartCoroutine(animateButton(objectHit));
				                    yield return new WaitForSeconds(1.0f);
				                    Application.LoadLevel("BuyCoinPack-c#");
				                    break;
			                case "Btn-03":
				                //    playSfx(tapSfx);
				                    StartCoroutine(animateButton(objectHit));
				                    yield return new WaitForSeconds(1.0f);
				                    Application.Quit();
				                    break;    
			            }    
			        }
		    }

	    //*****************************************************************************
	    // This function animates a button by modifying it's scales on x-y plane.
	    // can be used on any element to simulate the tap effect.
	    //*****************************************************************************
	    IEnumerator animateButton ( GameObject _btn  ){
		        canTap = false;
		        Vector3 startingScale = _btn.transform.localScale;    //initial scale    
		        Vector3 destinationScale = startingScale * 1.1f;        //target scale
		        
		        //Scale up
		        float t = 0.0f; 
		        while (t <= 1.0f) {
			            t += Time.deltaTime * buttonAnimationSpeed;
			            _btn.transform.localScale = new Vector3( Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
				                                                    Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
				                                                    _btn.transform.localScale.z);
			            yield return 0;
			        }
		        
		        //Scale down
		        float r = 0.0f; 
		        if(_btn.transform.localScale.x >= destinationScale.x) {
			            while (r <= 1.0f) {
				                r += Time.deltaTime * buttonAnimationSpeed;
				                _btn.transform.localScale = new Vector3( Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
					                                                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
					                                                        _btn.transform.localScale.z);
				                yield return 0;
				            }
			        }
		        
		        if(r >= 1)
			            canTap = true;
		    }

	    //*****************************************************************************
	    // Play sound clips
	    //*****************************************************************************
	    void playSfx ( AudioClip _clip  ){
		        GetComponent<AudioSource>().clip = _clip;
		        if(!GetComponent<AudioSource>().isPlaying) {
			            GetComponent<AudioSource>().Play();
			        }
		    }


	    //****************************************************************************
	    //Liga
	    //****************************************************************************

	    public static string getPlayerLiga()
	    {
		        string res = null;
		        int wins = PlayerPrefs.GetInt("PlayerWins");

		        if (wins >= 0 && wins <= 4)
			        {
			            //Liga de bronce
			            res = "Liga de bronce";
			           
			        }
		        else if (wins >= 5 && wins <= 9)
			        {
			            //Liga de plata
			            res = "Liga de plata";
			        }
		        else if (wins >= 10 && wins <= 14)
			        {
			            //Liga de oro
			            res = "Liga de oro";
			        }
		        else if (wins >= 15 && wins <= 20)
			        {
			            //Liga de Diamante
			            res = "Liga de diamante";
			        }

		        return res;

		    }

	    //***************************************************************
	    //Metodos Utiles
	    //***************************************************************

	public void GoMultiPlayer(string levelName){
		PlayerPrefs.SetInt("GameMode",1);
		Application.LoadLevel (levelName);
	}

	    public void NextLevelButton(int index)
	        {
		            Application.LoadLevel(index);
		        }

	        public void NextLevelButton(string levelName)
	        {
		            Application.LoadLevel(levelName);
		        }

	        public void GoRanking()
	        {
		        if (Social.localUser.authenticated)
			        {
			            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI("CgkIqKW33aMMEAIQFQ");
			        }
		        else
			        {
			            Social.localUser.Authenticate((bool success) => { });
			        }
		        }

	    public void GoAchievements()
	    {
		        if (Social.localUser.authenticated)
			        {
			            ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
			        }
		        else
			        {
			            Social.localUser.Authenticate((bool success) => { });
			        }
		    }

	    public void Exit()
	    {
		        Application.Quit();
		    }

	    public void GoOnlineQuickMatch()
	    {
		        if (Social.localUser.authenticated)
			        {
			            PlayGamesPlatform.Instance.RealTime.CreateQuickGame(1, 1, 0, SoccerRealTimeMultiplayerListener.Instance);
			            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
			            PlayerPrefs.SetInt("GameMode", 2);
			        }
		        else
			        {
			            Social.localUser.Authenticate((bool success) => { });
			        }
		    }

            
	    public void muteAudio(){
		        if (toggle == false) {
			            AudioListener.volume = 0.0f;
			            toggle = true;
		        } else {
			            AudioListener.volume = 1.0f;
			            toggle = false;
			        }
		    }


    internal void LoadUsersAndDisplay(ILeaderboard lb)
    {
        // get the user ids
        List<string> userIds = new List<string>();

        foreach (IScore score in lb.scores)
        {
            userIds.Add(score.userID);
        }
        // load the profiles and display (or in this case, log)
        Social.LoadUsers(userIds.ToArray(), (users) =>
        {
            string status = "Leaderboard loading: " + lb.title + " count = " +
                lb.scores.Length;
            foreach (IScore score in lb.scores)
            {
                IUserProfile user = FindUsers(users, score.userID);
                status += "\n" + score.formattedValue + " by " +
                    (string)(
                        (user != null) ? user.userName : "**unk_" + score.userID + "**");
            }
            Debug.Log(status);
        });
    }





    private static IUserProfile FindUsers(IUserProfile[] users,string id)
    {
        IUserProfile res = null;
        foreach(IUserProfile user in users)
        {
            if (user.id.Equals(id))
            {
                res = user;
            }
        }

        return res;
    }
} 

