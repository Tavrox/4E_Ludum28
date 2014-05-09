using UnityEngine;
using System.Collections;
using System.Text;
[ExecuteInEditMode]

public class TextUI : MonoBehaviour {

	[HideInInspector] public GameSetup SETUP;
	[HideInInspector] public TextMesh _mesh;
	public string DIALOG_ID = "NONE";
	public string text, displayedText;
	public bool dontTranslate = false, twinkleMessage, noScrollMessage;
	public bool hasBeenTranslated = false;
	[HideInInspector] public Color initColor;
	public Color color;
	private char tempLetter;
	private int cpt;
	private StringBuilder textArray;
	public int lettersToDisplay=12;
	[Range (0,0.5f)] public float scrollSpeed=0.2f;

	public void Awake()
	{
		SETUP = Resources.Load("Tuning/GameSetup") as GameSetup;
		_mesh = GetComponent<TextMesh>();
		initColor = color;
		DIALOG_ID = gameObject.name;
	}

	void Start()
	{
		if (hasBeenTranslated == false && dontTranslate == false)
		{
			TranslateThis();
		}
		
		if(twinkleMessage) {
			//OTTween _tween = new OTTween(prefabSprite, 1f).Tween("alpha", 1f).PingPong();
			//StartCoroutine("twinkle");
			InvokeRepeating("twinkle",0,2f);
		}
		if(lettersToDisplay>text.Length) lettersToDisplay=text.Length;
		if(!noScrollMessage) InvokeRepeating("scrollText",0,scrollSpeed);
	}
	
	private void twinkle () {
	//IEnumerator twinkle() {
		//yield return new WaitForSeconds(1f);
		gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
		//StartCoroutine("twinkle");
	}	
	private void scrollText() {
		//text = text.Replace("/n", "\n");
		tempLetter = text[0];
		textArray = new StringBuilder(text);
		for (cpt = 0; cpt<text.Length-1;cpt++) {
			textArray[cpt]=textArray[cpt+1];
		}
		textArray[text.Length-1]=tempLetter;
		text=textArray.ToString();
		displayedText=textArray.ToString(0,lettersToDisplay);
		
		_mesh.text = displayedText;
		_mesh.color = color;
	}
	void Update()
	{
		if (text != SETUP.TextSheet.TranslateSingle(this) && dontTranslate == false && hasBeenTranslated == true)
		{
			hasBeenTranslated = false;
		}

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
		text = SETUP.TextSheet.TranslateSingle(this);
//		print ("translateSingle" + DIALOG_ID);
	}
	public void TranslateAllInScene()
	{
		print ("translateScene");
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
