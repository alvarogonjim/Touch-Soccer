using UnityEngine;
using System.Collections;

public class SetLanguage : MonoBehaviour {

    private string res;

	void Awake () {
        //first we'll set "en" (english) as the default language
        res = PlayerPrefs.GetString("Language");

        LanguageDictionary.SetLanguage(SystemLanguage.English);

        if (res.Equals("English"))
        {
            LanguageDictionary.SetLanguage(SystemLanguage.English);
        }
        else if (res.Equals("French"))
        {
            LanguageDictionary.SetLanguage(SystemLanguage.French);
        }
        else if (res.Equals("Spanish"))
        {
            LanguageDictionary.SetLanguage(SystemLanguage.Spanish);
        }


        //if the system language isn't included in here, then the game will show the texts only in the default language

        Debug.Log ("Language set: " + Application.systemLanguage);
	}

    void Update()
    {

        if (res.Equals("English"))
        {
            LanguageDictionary.SetLanguage(SystemLanguage.English);
        }
        else if (res.Equals("French"))
        {
            LanguageDictionary.SetLanguage(SystemLanguage.French);
        }
        else if (res.Equals("Spanish"))
        {
            LanguageDictionary.SetLanguage(SystemLanguage.Spanish);
        }

    }

    public void SetEnglish(){
		LanguageDictionary.SetLanguage (SystemLanguage.English);
        res = "English";
        PlayerPrefs.SetString("Language", res);
    }

	public void SetFrench(){
		LanguageDictionary.SetLanguage (SystemLanguage.French);
        res = "French";
        PlayerPrefs.SetString("Language", res);
    }

	public void SetSpanish(){
		LanguageDictionary.SetLanguage (SystemLanguage.Spanish);
        res = "Spanish";
        PlayerPrefs.SetString("Language", res);
    }
}