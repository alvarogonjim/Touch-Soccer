using UnityEngine;
using System.Collections;
//add UI access if your using the Unity UI system
using UnityEngine.UI;

public class TranslateText : MonoBehaviour {

	Text textComponent; //for NGUI texts, change it to a UILabel variable
	string text;


	void Start () {
		//Get access to the Text component
		textComponent = GetComponent<Text> (); //for NGUI, change Text to a UILabel
		text = textComponent.text;

		//It will change this object's text to its corresponding translation
		textComponent.text = LanguageDictionary.stringList [text];
	}
}