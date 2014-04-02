using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetup : ScriptableObject {

	public enum LevelList
	{
		None,
		Zero,
		One,
		Two,
		Three,
		Four,
		Five
	};
	public enum languageList
	{
		french,
		english
	};
	public Vector2 GameSize;
	public float OrthelloSize;
	public string gameversion;
	public languageList ChosenLanguage;
	public string twitter_url;
	public string facebook_url;
	public string website_url;
	public DialogSheet TextSheet;

	public void changeLang( languageList _chosen)
	{
		ChosenLanguage = _chosen;
	}

	public void startTranslate(languageList _chosen)
	{
		if (TextSheet != null)
		{
			TextSheet.SetupTranslation(_chosen);
		}
		else
		{
			Debug.Log ("TextSheet is missing");
		}
	}
	public void translateSceneText()
	{
		TextSheet.SetupTranslation(ChosenLanguage);
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		TextSheet.TranslateAll(ref allTxt);
	}
}
