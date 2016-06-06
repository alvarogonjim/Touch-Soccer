using UnityEngine;
using System.Collections;
//First, add Generic Collections, so you can use dictionaries
using System.Collections.Generic;

public static class LanguageDictionary {
	//Create a public static Dictionary of strings named "stringList"
	//the first string is the KEY and the second string is the value
	public static Dictionary<string, string> stringList = new Dictionary<string, string>();

	//Create a public static function named SetLanguage with a string
	//variable named "lang":
	public static void SetLanguage (SystemLanguage lang) {

		stringList = new Dictionary<string, string>();

		//Check the chosen language ("lang"). In this case we're using 
		//English as the default language
		if (lang == SystemLanguage.English || lang == SystemLanguage.Unknown || lang == null) {
			//Set keys and values of the stringList, the key being the word
			//in default language, and the value the corresponding translation
			stringList.Add ("YES", "YES");
			stringList.Add ("Nombre Jugador" , "Player Name"); 
			stringList.Add ("RETOS", "CHALLENGES");
			stringList.Add ("Consigue 5 victorias", "Obtain 5 wins");
			stringList.Add ("Victorias:\nDerrotas:\nGoles:\nPorcentaje de victorias:", "Wins:\nDefeats:\nGoals:\nWinning percentage:");
			stringList.Add ("GRATIS" , "FREE");
			stringList.Add ("Comprar" , "Buy");
			stringList.Add ("Tienda" , "Store");
		}

		//Now, let's add translations for Spanish
		if (lang == SystemLanguage.Spanish) {
			stringList.Add ("YES", "SI");

		}

		//To end, I will provide a bigger example, for portuguese translation
		if (lang == SystemLanguage.Portuguese) {
			stringList.Add ("YES", "SIM");
			stringList.Add ("NO", "NÂO");
			stringList.Add ("START", "INICIAR");
			stringList.Add ("SETTINGS", "AJUSTES");
		}

		//You can add as many new keys to the stringList as you need
	}
}