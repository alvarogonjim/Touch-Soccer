using UnityEngine;
using System.Collections;

public class SetLanguage : MonoBehaviour {

	void Awake () {
		//first we'll set "en" (english) as the default language
		LanguageDictionary.SetLanguage (SystemLanguage.English);

		//if the system language isn't included in here, then the game will show the texts only in the default language

		Debug.Log ("Language set: " + Application.systemLanguage);
	}

}