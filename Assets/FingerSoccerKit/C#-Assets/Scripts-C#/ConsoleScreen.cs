using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleScreen : MonoBehaviour {

	public Text escribeLineas;
	public static string myLog;
	private string output ="";
	private string stack ="";
	// Update is called once per frame
	public static bool mostrarConsola=false;
	public GameObject consola;

	void HandleLogCallback (string condition, string stackTrace, LogType type)
	{
		output = condition;
		stack = stackTrace;
		myLog += "\n" + output;
	}

	void Update () {
		UnityEngine.Application.RegisterLogCallback (HandleLogCallback);
		escribeLineas.text = myLog;


	}

	public void showConsole(){
		mostrarConsola = true;
		if (mostrarConsola == true) {
			consola.SetActive (true);
		} else {
			consola.SetActive (false);
		}
		mostrarConsola = false;
	}


}
