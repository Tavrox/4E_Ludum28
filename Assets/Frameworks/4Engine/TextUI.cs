using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

public class TextUI : MonoBehaviour {

	[HideInInspector] public GameSetup SETUP;
	[HideInInspector] public TextMesh _mesh;
	public string DIALOG_ID = "NONE";
	public string text;
	public bool dontTranslate = false;
	public bool hasBeenTranslated = false;
	[HideInInspector] public Color initColor;
	public Color color;

	public void Awake()
	{
		SETUP = Resources.Load("Tuning/GameSetup") as GameSetup;
		_mesh = GetComponent<TextMesh>();
		initColor = color;
		DIALOG_ID = gameObject.name;
		if (hasBeenTranslated == false)
		{
			SETUP.startTranslate(SETUP.ChosenLanguage);
			text = SETUP.TextSheet.TranslateSingle(this);
			hasBeenTranslated = true;
		}
	}
	
	void Update()
	{
		if (hasBeenTranslated == false)
		{
			text = SETUP.TextSheet.TranslateSingle(this);
			hasBeenTranslated = true;
		}
		text = text.Replace("/n", "\n");
		_mesh.text = text;
		_mesh.color = color;
	}

	public void makeFadeOut()
	{
		new OTTween(this, 0.4f).Tween("color", Color.clear);
	}

	public void makeFadeIn()
	{
		new OTTween(this, 0.4f).Tween("color", initColor);
	}

	public void Format()
	{
		text = text.Replace("/n", "\n");
	}
	public void TranslateThis()
	{
		SETUP.TextSheet.SetupTranslation(SETUP.ChosenLanguage);
//		text = SETUP.TextSheet.TranslateSingle(this);
	}
	public void TranslateAllInScene()
	{
		SETUP.TextSheet.SetupTranslation(SETUP.ChosenLanguage);
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		SETUP.TextSheet.TranslateAll(ref allTxt);
	}
	public void resetAllDialogID()
	{
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		foreach (TextUI _tx in allTxt)
		{
			if (_tx.DIALOG_ID == "" || _tx.DIALOG_ID == " ")
			{
				_tx.DIALOG_ID = "NONE";
			}
		}
	}
	public void renameAllTextObject()
	{
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		foreach (TextUI _tx in allTxt)
		{
			if (_tx.DIALOG_ID != "" || _tx.DIALOG_ID != " " || _tx.DIALOG_ID != "NONE")
			{
				_tx.gameObject.name = _tx.DIALOG_ID;
			}
		}	
	}
	public void SetupDialogIDFromGameObject()
	{
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		foreach (TextUI _tx in allTxt)
		{
			_tx.DIALOG_ID = _tx.gameObject.name;
		}			
	}
}
