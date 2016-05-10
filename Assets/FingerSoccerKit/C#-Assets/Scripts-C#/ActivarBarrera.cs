using UnityEngine;
using System.Collections;

public class ActivarBarrera : MonoBehaviour {

	public string nombreAnimacion;
	public GameObject ObjectToAnimate;
	public Animation animBarrera;
	public static bool estaSubida;
	public GameObject barrera;


	// Use this for initialization
	void Start () {
		estaSubida = false;
	}
	public void activaBoolBarrera(){
		

		//Se ha llamado al powerup del elimina anteriormente?
		if (GlobalGameManager.llamadoPowerUpBarrera == false)
		{
			estaSubida = true;
			StartCoroutine("subeBarrera");
			//Si no vemos si tiene la habilidad disponible (mas de 0)
			if (GlobalGameManager.iPowerUpBarrera > 0)
			{
				Debug.Log(GlobalGameManager.powerUpBarrera.ToString());
				//Decrementamos la habilidad
				GlobalGameManager.iPowerUpBarrera--;
				//La habilidad la tiene
				GlobalGameManager.powerUpBarrera = true;

				//SOLO UN USO DE LA HABILIDAD:
				GlobalGameManager.soloUnaVezBarrera = 1;
				//Ponemos el llamado de elimina a true
				GlobalGameManager.llamadoPowerUpBarrera = true;

			}
			//En caso contrario falso
			else
			{
				GlobalGameManager.powerUpBarrera = false;
				barrera.SetActive (false);
			}
		}
	}
		
	 IEnumerator subeBarrera(){
		if (estaSubida == true) {
			animBarrera.CrossFade (nombreAnimacion);
			yield return new WaitForSeconds (animBarrera [nombreAnimacion].length);

		}
		estaSubida = false;
	}

}
