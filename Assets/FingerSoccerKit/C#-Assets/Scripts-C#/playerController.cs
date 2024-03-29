using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour {

	///*************************************************************************///
	/// Main Player Controller.
	/// This class manages the shooting process of human players.
	/// This also handles the rendering of debug lines in editor.
	///*************************************************************************///

	// Public Variables //
	internal int unitIndex;				//This unit's ID (given automatically by PlayerAI class)
	public GameObject selectionCircle;	//Reference to gameObject
	//Referenced GameObjects
	private GameObject helperBegin; 	//Start helper
	private GameObject helperEnd; 		//End Helper
	private GameObject arrowPlane; 		//arrow plane which is used to show shotPower
    public static GameObject[] chapas;
	public static GameObject[] enemigos;
	private GameObject enemigo;
	public string opponent;
    private GameObject chapa;
    private GameObject gameController;	//Reference to main game controller
	private float currentDistance;		//real distance of our touch/mouse position from initial drag position
	private float safeDistance; 			//A safe distance value which is always between min and max to avoid supershoots
	EffectBomb eB;


	private float pwr;					//shoot power

	//this vector holds shooting direction
	private Vector3 shootDirectionVector;
	//private Vector3 shootDirection;

	//prevent player to shoot twice in a round
	public static bool canShoot;
	internal float shootTime;
	private int timeAllowedToShoot = 10000; //In Seconds (in this kit we give players unlimited time to perform their turn of shooting)


    public static int contadorPowerUpTamano=1;
    public static int contadorPowerUpElimina = 1;
    public static int contadorPowerUpBarrera = 1;
	public AudioSource sonidoTamano;
	public AudioSource sonidoElimina;
	//public GameObject effectBomb;


    private string nombreEnemigo;

    //*****************************************************************************
    // Init
    //*****************************************************************************
    void Awake (){
       
		//Find and cache important gameObjects
		helperBegin = GameObject.FindGameObjectWithTag("mouseHelperBegin");
		helperEnd = GameObject.FindGameObjectWithTag("mouseHelperEnd");
		arrowPlane = GameObject.FindGameObjectWithTag("helperArrow");		
		gameController = GameObject.FindGameObjectWithTag("GameController");
        chapas  = GameObject.FindGameObjectsWithTag("Player");
		enemigos = GameObject.FindGameObjectsWithTag ("Opponent");

        //Init Variables
        pwr = 0.1f;
		currentDistance = 0;
		shootDirectionVector = new Vector3(0,0,0);
		//shootDirection = new Vector3 (0, 0, 0);
		canShoot = true;
		shootTime = timeAllowedToShoot;
		arrowPlane.GetComponent<Renderer>().enabled = false; //hide arrowPlane

	}

    void Start()
    {

        Debug.Log("EL NUMERO ES " + PlayerPrefs.GetInt("Skin"));
        cambiarSkin();
        cambiarAura();
    }

	void Update (){

        if (GlobalGameManager.powerUpElimina == true && GlobalGameManager.soloUnaVezElimina > 0)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                opponent = hit.transform.gameObject.name;
 

                if (opponent.Equals("Opponent-Player-1") || opponent.Equals("Opponent-Player-2") || opponent.Equals("Opponent-Player-3")
                    || opponent.Equals("Opponent-Player-4") || opponent.Equals("Opponent-Player-5"))
                {

                    nombreEnemigo = opponent;
                    enemigo = GameObject.Find(opponent);
                    enemigo.GetComponent<MeshRenderer>().enabled = false;
                    enemigo.GetComponent<MeshCollider>().enabled = false;
                    enemigo.GetComponent<OpponentUnitController>().enabled = false;
					sonidoElimina.Play ();
					GameObject.Find ("Bomb").transform.position = new Vector3(enemigo.transform.position.x,enemigo.transform.position.y + 4.0f,enemigo.transform.position.z );
					enemigo.GetComponent<EffectBomb> ().explota ();
				
                    GlobalGameManager.powerUpElimina = false;
                    GlobalGameManager.soloUnaVezElimina = 0;
                    contadorPowerUpElimina++;
                    GlobalGameManager.iPowerUpElimina = GlobalGameManager.iPowerUpElimina - 1;


                }
                else if(opponent.Equals("Player2Unit-1") || opponent.Equals("Player2Unit-2") || opponent.Equals("Player2Unit-3")
                    || opponent.Equals("Player2Unit-4") || opponent.Equals("Player2Unit-5"))
                {

                    nombreEnemigo = opponent;
                    enemigo = GameObject.Find(opponent);
                    enemigo.GetComponent<MeshRenderer>().enabled = false;
                    enemigo.GetComponent<MeshCollider>().enabled = false;
                    enemigo.GetComponent<OpponentUnitController>().enabled = false;
                    enemigo.GetComponent<playerController>().enabled = false;
					sonidoElimina.Play ();
					enemigo.GetComponent<EffectBomb> ().explota ();

                    GlobalGameManager.powerUpElimina = false;
                    GlobalGameManager.soloUnaVezElimina = 0;
                    contadorPowerUpElimina++;
                    GlobalGameManager.iPowerUpElimina = GlobalGameManager.iPowerUpElimina - 1;




                }

            }

        }
        if (nombreEnemigo != null)
        {
            enemigo = GameObject.Find(nombreEnemigo);
            enemigo.GetComponent<MeshCollider>().enabled = false;
        }
            //Active the selection circles around Player units when they have the turn.
        if (GlobalGameManager.playersTurn && gameObject.tag == "Player" && !GlobalGameManager.goalHappened)
			selectionCircle.GetComponent<Renderer>().enabled = true;
		else if(GlobalGameManager.opponentsTurn && gameObject.tag == "Player_2" && !GlobalGameManager.goalHappened)
			selectionCircle.GetComponent<Renderer>().enabled = true;			
		else	
			selectionCircle.GetComponent<Renderer>().enabled = false;

        
        foreach (GameObject chapa in chapas)
          {
            if(contadorPowerUpTamano == 0)
                chapa.transform.localScale = new Vector3(2.5f, 0.5f, 2.5f);  

             }
	  
        }


	

	//***************************************************************************//
	// Works fine with mouse and touch
	// This is the main functiuon used to manage drag on units, calculating the power and debug vectors, and set the final parameters to shoot.
	//***************************************************************************//
	void OnMouseDrag (){
        if (canShoot && ((GlobalGameManager.playersTurn && gameObject.tag == "Player") || (GlobalGameManager.opponentsTurn && gameObject.tag == "Player_2")))
                {
            
                   //print("Draged");
                    currentDistance = Vector3.Distance(helperBegin.transform.position, transform.position);

                    //limiters
                    if(currentDistance <= GlobalGameManager.maxDistance)
                        safeDistance = currentDistance;
                    else
                        safeDistance = GlobalGameManager.maxDistance;

                    pwr = Mathf.Abs(safeDistance) * 17; //this is very important. change with extreme caution.

                    //show the power arrow above the unit and scale is accordingly.
                    manageArrowTransform();		

                    //position of helperEnd
                    //HelperEnd is the exact opposite (mirrored) version of our helperBegin object 
                    //and help us to calculate debug vectors and lines for a perfect shoot.
                    //Please refer to the basic geometry references of your choice to understand the math.
                    Vector3 dxy = helperBegin.transform.position - transform.position;
                    float diff = dxy.magnitude;
                    helperEnd.transform.position = transform.position + ((dxy / diff) * currentDistance * -1);

                    helperEnd.transform.position = new Vector3( helperEnd.transform.position.x,
                                                                helperEnd.transform.position.y,
                                                                -0.5f);				

                    //debug line from initial position to our current touch position
                    Debug.DrawLine(transform.position, helperBegin.transform.position, Color.red);
                    //debug line from initial position to maximum power position (mirrored)
                    Debug.DrawLine(transform.position, arrowPlane.transform.position, Color.blue);
                    //debug line from initial position to the exact opposite position (mirrored) of our current touch position
                    Debug.DrawLine(transform.position, (2 * transform.position) - helperBegin.transform.position, Color.yellow);
                    //cast ray forward and collect informations
                    castRay();

                    //Not used! You can extend this function to have more precise control over physics of the game
                    //sweepTest();

                    //final vector used to shoot the unit.


                    shootDirectionVector = Vector3.Normalize(helperBegin.transform.position - transform.position);
                //shootDirectionVector =  Vector3.Normalize(sh);
                    //print(shootDirectionVector);

                }
    }

	//***************************************************************************//
	// Cast the rigidbody's shape forward to see if it is about to hit anything.
	//***************************************************************************//
	void sweepTest (){
		RaycastHit hit;
		if ( GetComponent<Rigidbody>().SweepTest( (helperEnd.transform.position - transform.position).normalized, out hit, 15 )) {
			print("if hit ??? : " + hit.distance + " - " + hit.transform.gameObject.name);
		}
	}

	//***************************************************************************//
	// Cast a ray forward and collect informations like if it hits anything...
	//***************************************************************************//
	private RaycastHit hitInfo;
	private Ray ray;
	void castRay (){
		
		//cast the ray from units position with a normalized direction out of it which is mirrored to our current drag vector.
		ray = new Ray(transform.position, (helperEnd.transform.position - transform.position).normalized );
			
		if(Physics.Raycast(ray, out hitInfo, currentDistance)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			
			//debug line whenever the ray hits something.
			Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan);
					
			//draw reflected vector like a billiard game. this is the out vector which reflects from targets geometry.
			Vector3 reflectedVector = Vector3.Reflect( (hitInfo.point - ray.origin),  hitInfo.normal );
			Debug.DrawRay(hitInfo.point, reflectedVector , Color.gray, 0.2f);
			
			//draw inverted reflected vector (useful for fine-tuning the final shoot)
			Debug.DrawRay(hitInfo.transform.position, reflectedVector * -1, Color.white, 0.2f);
			
			//draw the inverted normal which is more likely to be similar to real world response.
			Debug.DrawRay(hitInfo.transform.position, hitInfo.normal * -3, Color.red, 0.2f);
			
			//Debug
			print("Ray hits: " + objectHit.name + " At " + Time.time + " And Reflection is: " + reflectedVector);
		}
	}

	//***************************************************************************//
	// Unhide and process the transform and scale of the power Arrow object
	//***************************************************************************//
	void manageArrowTransform (){
		//power arrow codes
		//hide arrowPlane
		arrowPlane.GetComponent<Renderer>().enabled = true;
		
		//calculate position
		if(currentDistance <= GlobalGameManager.maxDistance) {
			arrowPlane.transform.position = new Vector3(	(2 * transform.position.x) - helperBegin.transform.position.x,
			                                            	(2 * transform.position.y) - helperBegin.transform.position.y,
			                                          		-1.5f );
		} else {
			Vector3 dxy = helperBegin.transform.position - transform.position;
			float diff = dxy.magnitude;
			arrowPlane.transform.position = transform.position + ((dxy / diff) * GlobalGameManager.maxDistance * -1);
			arrowPlane.transform.position = new Vector3(arrowPlane.transform.position.x,
			                                            arrowPlane.transform.position.y,
			                                            -1.5f);
		}

		//calculate rotation
		Vector3 dir = helperBegin.transform.position - transform.position;
		float outRotation; // between 0 - 360
		
		if(Vector3.Angle(dir, transform.forward) > 90) 
			outRotation = Vector3.Angle(dir, transform.right);
		else
			outRotation = Vector3.Angle(dir, transform.right) * -1;
			
		arrowPlane.transform.eulerAngles = new Vector3(0, 0, outRotation);
		//print(Vector3.Angle(dir, transform.forward));
		
		//calculate scale
		float scaleCoefX = Mathf.Log(1 + safeDistance/2, 2) * 2.2f;
		float scaleCoefY = Mathf.Log(1 + safeDistance/2, 2) * 2.2f;
		arrowPlane.transform.localScale = new Vector3(1 + scaleCoefX, 1 + scaleCoefY, 0.001f); //default scale
	}

	//***************************************************************************//
	// Actual shoot fucntion
	//***************************************************************************//
	void OnMouseUp (){

		//Special checks for 2-player game
		if( (GlobalGameManager.playersTurn && gameObject.tag == "Player_2") || (GlobalGameManager.opponentsTurn && gameObject.tag == "Player") ) {
			arrowPlane.GetComponent<Renderer>().enabled = false;
			return;
		}
		
		//give the player a second chance to choose another ball if drag on the unit is too low
		print("currentDistance: " + currentDistance);
		if(currentDistance < 0.75f) {
			arrowPlane.GetComponent<Renderer>().enabled = false;
			return;
		}
		
		//But if player wants to shoot anyway:
		//prevent double shooting in a round
		if(!canShoot)
			return;
		
		//no more shooting is possible	
		canShoot = false;
		
		//keep track of elapsed time after letting the ball go, 
		//so we can findout if ball has stopped and the round should be changed
		//this is the time which user released the button and shooted the ball
		shootTime = Time.time;
		
		//hide helper arrow object
		arrowPlane.GetComponent<Renderer>().enabled = false;
		
		//do the physics calculations and shoot the ball 
		Vector3 outPower = shootDirectionVector * pwr * -1;
		
		//always make the player to move only in x-y plane and not on the z direction
		print("shoot power: " + outPower.magnitude + " " + outPower);		
		GetComponent<Rigidbody>().AddForce(outPower, ForceMode.Impulse);
		
		//change the turn
		if(GlobalGameManager.gameMode == 0)
			StartCoroutine(gameController.GetComponent<GlobalGameManager>().managePostShoot("Player", unitIndex, outPower));
		else
			StartCoroutine(gameController.GetComponent<GlobalGameManager>().managePostShoot(gameObject.tag, unitIndex, outPower));
	}

    
    public static void cambiarSkin()
    {
        int index = PlayerPrefs.GetInt("Skin");
        Texture2D mat = Resources.Load(index.ToString(), typeof(Texture2D)) as Texture2D;

        foreach (GameObject chapa in chapas)
        {
            chapa.GetComponent<Renderer>().material.SetTexture("_MainTex", mat);
        }
    }

    public static void cambiarAura()
    {
        int i = PlayerPrefs.GetInt("Aura");
       /* rojo
            azul
            verde
            amarillo
            rosa
    */     
    GameObject[] circulos = GameObject.FindGameObjectsWithTag("selectionCirclePlayer");


        switch (i)
        {
            case 0:

                foreach (GameObject circulo in circulos)
                {
                    Renderer rend = circulo.GetComponent<Renderer>();
                    rend.material.color = Color.red;
                }
                break;
            case 1:
                foreach (GameObject circulo in circulos)
                {
                    Renderer rend = circulo.GetComponent<Renderer>();
                    rend.material.color = Color.blue;
                }
                break;
            case 2:
                foreach (GameObject circulo in circulos)
                {
                    Renderer rend = circulo.GetComponent<Renderer>();
                    rend.material.color = Color.green;
                }
                break;
            case 3:
                foreach (GameObject circulo in circulos)
                {
                    Renderer rend = circulo.GetComponent<Renderer>();
                    rend.material.color = Color.yellow;
                }
                break;
            case 4:
                foreach (GameObject circulo in circulos)
                {
                 Renderer rend = circulo.GetComponent<Renderer>();
				rend.material.color = Color.magenta;
                }

                break;
            }
    }

    //**************************************************
    //PowerUps
    //**************************************************

    void OnMouseDown()
    {


        if (GlobalGameManager.powerUpTamano == true && GlobalGameManager.soloUnaVezTamano > 0)
        {
            Vector3 final = new Vector3(5.5f, 0.5f, 5.5f);
            transform.localScale = final;
            
            sonidoTamano.Play();
            GlobalGameManager.soloUnaVezTamano = 0;
            contadorPowerUpTamano++;
            GlobalGameManager.iPowerUpTamano = GlobalGameManager.iPowerUpTamano - 1;
    
         }

    }

}

