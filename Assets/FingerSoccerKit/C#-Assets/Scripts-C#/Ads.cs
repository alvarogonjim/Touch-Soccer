using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Ads : MonoBehaviour
{
	public void ShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}
	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
			Debug.Log ("ESTAMOS DENTRO DEL PUTO ANUNCION CABEZA");
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			int creditos = PlayerPrefs.GetInt ("PlayerCredits");
			creditos = creditos + 5;
			Debug.Log ("Funciona cabeza");
			GameObject.Find ("Creditos").GetComponent<Text> ().text = creditos.ToString ();

			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}