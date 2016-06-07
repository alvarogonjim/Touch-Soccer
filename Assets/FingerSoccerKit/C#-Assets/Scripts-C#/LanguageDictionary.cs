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
			stringList.Add ("Partida Rapida", "Quick Match");
			stringList.Add ("Creditos\nGratis", "Free\nCredits");
			stringList.Add ("Monedas\nGratis", "Free\nCoins");
			stringList.Add ("Habilidades", "Skills");
			stringList.Add ("Agrandar", "Magnify");
			stringList.Add ("Duplica el tamaño de la chapa durante dos turnos", "Twice the size of the sheet during two shifts");
			stringList.Add ("Reventar", "Blow Up");
			stringList.Add ("Destruye una chapa del oponente", "Destroy a sheet opponent");
			stringList.Add ("Disponible:", "Available:");
			stringList.Add ("Barrera", "Wall");
			stringList.Add ("Activa un escudo defensivo en la porteria durante dos turnos", "Activate a defensive shield in the football goal during two shifts");
			stringList.Add ("Turno Extra", "Extra Shift");
			stringList.Add ("Tienes un turno extra", "You have an extra shift");
			stringList.Add ("Equipaciones", "Teams");
			stringList.Add ("Personalizadas", "Custom");
			stringList.Add ("Auras", "Auras");
			stringList.Add ("Chocolate Blanco", "White Chocolate");
			stringList.Add ("Chocolate", "Chocolate");
			stringList.Add ("Trebol", "Clover");
			stringList.Add ("Corazon", "Heart");
			stringList.Add ("Demonio", "Demon");
			stringList.Add ("Ocho", "Eight");
			stringList.Add ("Paz", "Peace");
			stringList.Add ("Yin Yang", "Yin Yang");
			stringList.Add ("Emoticon", "Emoticon");
			stringList.Add ("Catrina", "Catrina");
			stringList.Add ("Furioso", "Furious");
			stringList.Add ("Eye", "Eye");
			stringList.Add ("Atomo", "Atom");
			stringList.Add ("Boton", "Button");
			stringList.Add ("Radioactivo", "Radiactive");
			stringList.Add ("Huevo", "Egg");
			stringList.Add ("Pizza", "Pizza");
			stringList.Add ("Donut", "Donut");
			stringList.Add ("Pirata", "Pirate");
			stringList.Add ("Usar", "Use");
			stringList.Add ("Alemania", "Germany");
			stringList.Add ("España", "Spain");
			stringList.Add ("Inglaterra", "England");
			stringList.Add ("Portugal", "Portugal");
			stringList.Add ("Belgica", "Belgium");
			stringList.Add ("Italia", "Italy");
			stringList.Add ("Rusia", "Russia");
			stringList.Add ("Suiza", "Switzerland");
			stringList.Add ("Austria", "Austria");
			stringList.Add ("Croacia", "Croatia");
			stringList.Add ("Ucrania", "Ukraine");
			stringList.Add ("Republica Checa", "Czech Republic");
			stringList.Add ("Suecia", "Sweden");
			stringList.Add ("Polonia", "Poland");
			stringList.Add ("Rumania", "Romania");
			stringList.Add ("Eslovaquia", "Slovakia");
			stringList.Add ("Hungria", "Hungary");
			stringList.Add ("Turquia", "Turkey");
			stringList.Add ("Irlanda", "Ireland");
			stringList.Add ("Islandia", "Iceland");
			stringList.Add ("Gales", "Wales");
			stringList.Add ("Albania", "Albania");
			stringList.Add ("Irlanda del Norte", "North Ireland");
			stringList.Add ("Francia", "France");
			stringList.Add ("Aura Roja", "Red Aura");
			stringList.Add ("Aura Azul", "Blue Aura");
			stringList.Add ("Aura Verde", "Green Aura");
			stringList.Add ("Aura Amarilla", "Yellow Aura");
			stringList.Add ("Aura Rosa", "Pink Aura");
			stringList.Add ("Nevado", "Snow");
			stringList.Add ("Calle", "Street");
			stringList.Add ("Savanna", "Savanna");
			stringList.Add ("Basico", "Basic");
			stringList.Add ("Campos", "Fields");
			stringList.Add ("Formaciones", "Formations");
			stringList.Add ("Monedas", "Coins");
			stringList.Add ("Creditos", "Credits");





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