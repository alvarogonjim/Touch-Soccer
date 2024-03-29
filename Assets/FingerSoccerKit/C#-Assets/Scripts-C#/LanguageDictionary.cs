﻿using UnityEngine;
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
			//stringList.Add ("Nombre Jugador" , "Player Name"); 
			stringList.Add ("RETOS", "CHALLENGES");
			stringList.Add ("Consigue 5 victorias", "Obtain 5 wins");
			stringList.Add ("Victorias:\nDerrotas:\nGoles:\nPorcentaje de victorias:", "Wins:\nDefeats:\nGoals:\nWinning percentage:");
			stringList.Add ("GRATIS" , "FREE");
			stringList.Add ("Comprar" , "Buy");
			stringList.Add ("Tienda" , "Store");
			stringList.Add ("Partida Rapida", "Quick Match");
			stringList.Add ("Creditos Gratis", "Free Credits");
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
			stringList.Add ("Perfil", "Profile");
			stringList.Add ("Chapas", "Teams");
			stringList.Add ("Partida Online", "Online Match");
			stringList.Add ("Equipos", "Teams");
			stringList.Add ("Idioma", "Language");
			stringList.Add ("Ganancias:", "Earnings:");
			stringList.Add ("Recompensa:", "Reward:");
			stringList.Add ("Estadisticas:", "Statistics:");
			stringList.Add ("División:", "Division:");
			stringList.Add ("1 VS 1", "1 VS 1");
			stringList.Add ("Proximamente", "Coming Soon");
			stringList.Add ("Perfil", "Profile");
			stringList.Add ("Obtenido", "Obtained");
			stringList.Add ("Salir", "Exit");


		}
	
		if (lang == SystemLanguage.French) {

			//stringList.Add ("Nombre Jugador" , "Player Name"); 
			stringList.Add ("RETOS", "DÉFIS");
			stringList.Add ("Consigue 5 victorias", "Obtenez 5 victoires");
			stringList.Add ("Victorias:\nDerrotas:\nGoles:\nPorcentaje de victorias:", "Victoires:\nPertes:\nObjectifs:\nPourcentage de victoires:");
			stringList.Add ("GRATIS" , "GRATUIT");
			stringList.Add ("Comprar" , "Acheter");
			stringList.Add ("Tienda" , "Boutique");
			stringList.Add ("Partida Rapida", "Quickplay");
			stringList.Add ("Creditos Gratis", "Gratuit Credits");
			stringList.Add ("Monedas\nGratis", "Gratuit\nPièces");
			stringList.Add ("Habilidades", "Compétences");
			stringList.Add ("Agrandar", "Agrandir");
			stringList.Add ("Duplica el tamaño de la chapa durante dos turnos", "Deux fois la taille de la feuille pendant deux quarts");
			stringList.Add ("Reventar", "Éclat");
			stringList.Add ("Destruye una chapa del oponente", "Détruisez un adversaire de la feuille");
			stringList.Add ("Disponible:", "Disponible:");
			stringList.Add ("Barrera", "Barrière");
			stringList.Add ("Activa un escudo defensivo en la porteria durante dos turnos", "Active un bouclier défensif dans le but pendant deux quarts");
			stringList.Add ("Turno Extra", "Troisième Extra");
			stringList.Add ("Tienes un turno extra", "Vous avez un décalage supplémentaire");
			stringList.Add ("Equipaciones", "Equipes");
			stringList.Add ("Personalizadas", "Custom");
			stringList.Add ("Auras", "Auras");
			stringList.Add ("Chocolate Blanco", "Chocolat blanc");
			stringList.Add ("Chocolate", "Chocolat");
			stringList.Add ("Trebol", "Trèfle");
			stringList.Add ("Corazon", "Coeur");
			stringList.Add ("Demonio", "Demon");
			stringList.Add ("Ocho", "Huit");
			stringList.Add ("Paz", "Paix");
			stringList.Add ("Yin Yang", "Yin Yang");
			stringList.Add ("Emoticon", "Émoticône");
			stringList.Add ("Catrina", "Catrina");
			stringList.Add ("Furioso", "Furieux");
			stringList.Add ("Eye", "œil");
			stringList.Add ("Atomo", "Atome");
			stringList.Add ("Boton", "Bouton");
			stringList.Add ("Radioactivo", "Radioactif");
			stringList.Add ("Huevo", "œuf");
			stringList.Add ("Pizza", "Pizza");
			stringList.Add ("Donut", "Beignet");
			stringList.Add ("Pirata", "Pirate");
			stringList.Add ("Usar", "Utiliser");
			stringList.Add ("Alemania", "Allemagne");
			stringList.Add ("España", "Espagne");
			stringList.Add ("Inglaterra", "England");
			stringList.Add ("Portugal", "Portugal");
			stringList.Add ("Belgica", "Belgique");
			stringList.Add ("Italia", "Italie");
			stringList.Add ("Rusia", "Russie");
			stringList.Add ("Suiza", "Suisse");
			stringList.Add ("Austria", "Autriche");
			stringList.Add ("Croacia", "Croatie");
			stringList.Add ("Ucrania", "Ukraine");
			stringList.Add ("Republica Checa", "République Tchèque");
			stringList.Add ("Suecia", "Suède");
			stringList.Add ("Polonia", "Pologne");
			stringList.Add ("Rumania", "Roumanie");
			stringList.Add ("Eslovaquia", "Slovaquie");
			stringList.Add ("Hungria", "Hongrie");
			stringList.Add ("Turquia", "Turquie");
			stringList.Add ("Irlanda", "Irlande");
			stringList.Add ("Islandia", "Islande");
			stringList.Add ("Gales", "Pays de Galles");
			stringList.Add ("Albania", "Albanie");
			stringList.Add ("Irlanda del Norte", "Irlande du Nord");
			stringList.Add ("Francia", "France");
			stringList.Add ("Aura Roja", "Rouge Aura");
			stringList.Add ("Aura Azul", "Bleue Aura");
			stringList.Add ("Aura Verde", "Vert Aura");
			stringList.Add ("Aura Amarilla", "Jaune Aura");
			stringList.Add ("Aura Rosa", "Rose Aura");
			stringList.Add ("Nevado", "Neigeux");
			stringList.Add ("Calle", "Rue");
			stringList.Add ("Savanna", "Savane");
			stringList.Add ("Basico", "De Base");
			stringList.Add ("Campos", "Terrains");
			stringList.Add ("Formaciones", "Formations");
			stringList.Add ("Monedas", "Pièces");
			stringList.Add ("Creditos", "Crédits");
			stringList.Add ("Perfil", "Profil");
			stringList.Add ("Chapas", "Équipes");
			stringList.Add ("Partida Online", "Jeu En Ligne");
			stringList.Add ("Equipos", "Équipes");
			stringList.Add ("Idioma", "Langue");
			stringList.Add ("Ganancias:", "Gains:");
			stringList.Add ("Recompensa:", "Récompense:");
			stringList.Add ("Estadisticas:", "Statistiques:");
			stringList.Add ("División:", "Division:");
			stringList.Add ("1 VS 1", "1 VS 1");
			stringList.Add ("Proximamente", "Arrive bientot");
			stringList.Add ("Perfil", "Profil");
			stringList.Add ("Obtenido", "Obtenu");
			stringList.Add ("Salir", "Sortie");
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
			stringList.Add ("RETOS", "DESAFIOS");
			stringList.Add ("Consigue 5 victorias", "Obter 5 vitórias");
			stringList.Add ("Victorias:\nDerrotas:\nGoles:\nPorcentaje de victorias:", "Vitórias:\nDerrotas:\nGols:\nPercentagem de vitórias:");
			stringList.Add ("GRATIS" , "LIVRE");
			stringList.Add ("Comprar" , "Comprar");
			stringList.Add ("Tienda" , "Loja");
			stringList.Add ("Partida Rapida", "Jogo Rápido");
			//stringList.Add ("Creditos\nGratis", "Créditos\Gratis");
			stringList.Add ("Monedas\nGratis", "Moedas\nde Livre");
			stringList.Add ("Habilidades", "Habilidades");
			stringList.Add ("Agrandar", "Ampliar");
			stringList.Add ("Duplica el tamaño de la chapa durante dos turnos", "Duas vezes o tamanho da folha durante dois turnos");
			stringList.Add ("Reventar", "Explosão");
			stringList.Add ("Destruye una chapa del oponente", "Destruir um oponente folha");
			stringList.Add ("Disponible:", "Disponível:");
			stringList.Add ("Barrera", "Barreira");
			stringList.Add ("Activa un escudo defensivo en la porteria durante dos turnos", "Ativa um escudo de defesa no gol durante dois turnos");
			stringList.Add ("Turno Extra", "Turno Extra");
			stringList.Add ("Tienes un turno extra", "Você tem um turno extra");
			stringList.Add ("Equipaciones", "Equipes");
			stringList.Add ("Personalizadas", "Personalizado");
			stringList.Add ("Auras", "Auras");
			stringList.Add ("Chocolate Blanco", "Chocolate Branco");
			stringList.Add ("Chocolate", "Chocolate");
			stringList.Add ("Trebol", "Trevo");
			stringList.Add ("Corazon", "Coração");
			stringList.Add ("Demonio", "Demônio");
			stringList.Add ("Ocho", "Oito");
			stringList.Add ("Paz", "Paz");
			stringList.Add ("Yin Yang", "Yin Yang");
			stringList.Add ("Emoticon", "Emoticon");
			stringList.Add ("Catrina", "Catrina");
			stringList.Add ("Furioso", "Furioso");
			stringList.Add ("Eye", "Eye");
			stringList.Add ("Atomo", "Atomo");
			stringList.Add ("Boton", "Botão");
			stringList.Add ("Radioactivo", "Radioativo");
			stringList.Add ("Huevo", "Ovo");
			stringList.Add ("Pizza", "Pizza");
			stringList.Add ("Donut", "Donut");
			stringList.Add ("Pirata", "Pirata");
			stringList.Add ("Usar", "Uso");
			stringList.Add ("Alemania", "Alemanha");
			stringList.Add ("España", "Espanha");
			stringList.Add ("Inglaterra", "Inglaterra");
			stringList.Add ("Portugal", "Portugal");
			stringList.Add ("Belgica", "Belgica");
			stringList.Add ("Italia", "Italia");
			stringList.Add ("Rusia", "Rússia");
			stringList.Add ("Suiza", "Suíça");
			stringList.Add ("Austria", "Áustria");
			stringList.Add ("Croacia", "Croácia");
			stringList.Add ("Ucrania", "Ucrânia");
			stringList.Add ("Republica Checa", "Republica Checa");
			stringList.Add ("Suecia", "Suécia");
			stringList.Add ("Polonia", "Polônia");
			stringList.Add ("Rumania", "Romênia");
			stringList.Add ("Eslovaquia", "Eslováquia");
			stringList.Add ("Hungria", "Hungria");
			stringList.Add ("Turquia", "Turquia");
			stringList.Add ("Irlanda", "Irlanda");
			stringList.Add ("Islandia", "Islândia");
			stringList.Add ("Gales", "País de Gales");
			stringList.Add ("Albania", "Albânia");
			stringList.Add ("Irlanda del Norte", "Irlanda do Norte");
			stringList.Add ("Francia", "França");
			stringList.Add ("Aura Roja", "Aura Vermelha");
			stringList.Add ("Aura Azul", "Aura Azul");
			stringList.Add ("Aura Verde", "Aura Verde");
			stringList.Add ("Aura Amarilla", "Aura amarela");
			stringList.Add ("Aura Rosa", "Aura Rosa");
			stringList.Add ("Nevado", "De Neve");
			stringList.Add ("Calle", "Rua");
			stringList.Add ("Savanna", "Savanna");
			stringList.Add ("Basico", "Básico");
			stringList.Add ("Campos", "Campos");
			stringList.Add ("Formaciones", "Formações");
			stringList.Add ("Monedas", "Moedas");
			stringList.Add ("Creditos", "Créditos");
			stringList.Add ("Perfil", "Perfil");
			stringList.Add ("Chapas", "Folhas");
			stringList.Add ("Partida Online", "Jogo on-line");
			stringList.Add ("Equipos", "Times de futebol");
			stringList.Add ("1 VS 1", "1 VS 1");
			stringList.Add ("Proximamente", "Em breve");
			stringList.Add ("Salir", "Saída");
		}

		//You can add as many new keys to the stringList as you need
	}
}