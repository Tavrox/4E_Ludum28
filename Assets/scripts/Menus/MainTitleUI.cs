using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainTitleUI : MonoBehaviour 
{
	private GameSetup SETUP;
	private List<LevelInfo> levelInformations;
	private LevelChooser Chooser;
	public enum MenuStates
	{
		Start,
		Options,
		Credits,
		LevelChooser
	};
	public MenuStates CurrentState;
	public PlayerData PLAYERDAT;

	public GameObject awayPlace;
	public GameObject frontPlace;
	public GameObject Credits;
	public GameObject Landing;
	public GameObject LevelChooser;
	public GameObject Options;
	public Camera Cam;
	
	void Awake () 
	{
		name = "TitleMenu";
		SETUP = Resources.Load ("Tuning/GameSetup") as GameSetup;
		Chooser = FETool.findWithinChildren(gameObject, "LevelChooser").GetComponent<LevelChooser>();
		SETUP.startTranslate(SETUP.ChosenLanguage);
		SETUP.translateSceneText();	
		levelInformations = new List<LevelInfo> ();
		//Screen.SetResolution(800,600, false);
		Application.targetFrameRate = 60;

		if (GameObject.FindGameObjectWithTag("PlayerData") == null)
		{
			GameObject _dataObj = Instantiate(Resources.Load("Presets/PlayerData")) as GameObject;
			PLAYERDAT = _dataObj.GetComponent<PlayerData>();
		}
		else
		{
			PLAYERDAT = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
		}

		if (GameObject.Find("Frameworks") == null)
		{
			GameObject fmObj = Instantiate(Resources.Load("Presets/Frameworks")) as GameObject;
			fmObj.name = "Frameworks";
		}
		
		Chooser.Setup ();
		awayPlace = FETool.findWithinChildren(gameObject, "AwayPlace");
		frontPlace = FETool.findWithinChildren(gameObject, "FrontPlace");
		Credits = FETool.findWithinChildren(gameObject, "Credits");
		Landing = FETool.findWithinChildren(gameObject, "Landing");
		LevelChooser = FETool.findWithinChildren(gameObject, "LevelChooser");
		Options = FETool.findWithinChildren(gameObject, "Options");
		Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		TranslateAllInScene();
	}
	
	public void TranslateAllInScene()
	{
		print ("translateScene");
		SETUP.TextSheet.SetupTranslation(SETUP.ChosenLanguage);
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		SETUP.TextSheet.TranslateAll(ref allTxt);
	}

	public static GameSetup getSetup()
	{
		GameSetup _set = Resources.Load ("Tuning/GameSetup") as GameSetup;
		return _set;
	}

	public void makeTransition (GameObject _thingIn)
	{
		new OTTween(Cam.transform ,1f, OTEasing.QuadInOut).Tween("position", new Vector3( _thingIn.transform.position.x, _thingIn.transform.position.y, Cam.transform.position.z ));
	}

	public void backHome()
	{
		new OTTween(Credits.transform, 1f).Tween("position", awayPlace.transform.position);
		new OTTween(LevelChooser.transform, 1f).Tween("position", awayPlace.transform.position);
		new OTTween(Options.transform, 1f).Tween("position", awayPlace.transform.position);
		new OTTween(Landing.transform, 1f).Tween("position", frontPlace.transform.position);

	}
}
