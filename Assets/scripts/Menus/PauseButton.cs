using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {
	
	public enum buttonList
	{
		None,
		Resume,
		Save,
		Menu,
		Exit,
		MenuYes,
		MenuNo,
		ExitYes,
		ExitNo,
		OptionYes,
		OptionNo,
		SkipEndLvl
	};
	public buttonList buttonType;
	public OTAnimatingSprite _animBtn;
	private TextMesh txtButton;
	private Player _player;
	private Transform pauseDefault, pauseMainMenu, pauseExit, pauseOption;
	public string STATE="Default";
	private PauseButton [] _listButtons;
	public bool first=true,trigged=false;
	
	// Use this for initialization
	void Start () {
		pauseDefault = GameObject.Find("Player/IngameUI/Pause/PauseDefault").GetComponent<Transform>();
		pauseMainMenu = GameObject.Find("Player/IngameUI/Pause/PauseMainMenu").GetComponent<Transform>();
		pauseExit = GameObject.Find("Player/IngameUI/Pause/PauseExit").GetComponent<Transform>();
		pauseOption = GameObject.Find("Player/IngameUI/Pause/PauseOption").GetComponent<Transform>();
		
		_listButtons = GameObject.FindObjectsOfType(typeof(PauseButton)) as PauseButton[];
		
		_player=GameObject.Find("Player").GetComponent<Player>();
		txtButton = GetComponentInChildren<TextMesh>();
		_animBtn = GetComponentInChildren<OTAnimatingSprite>();
		_animBtn.PlayLoop("static");
		_animBtn.speed = .8f;
		foreach(PauseButton btn in _listButtons)
		{if(btn!=null && btn.gameObject.activeInHierarchy) {btn.STATE="Default";btn.first=true;btn.trigged=false;}}
	}
//	void OnEnable () {
//		foreach(PauseButton btn in _listButtons)
//		{if(btn!=null && btn.gameObject.activeInHierarchy) {/*btn.STATE="Default";btn.first=true;btn.trigged=false;*/}}
//	}
	void Update () {
//		print (STATE+" " +trigged+ " " +first);
		if (Input.GetKeyDown(_player.InputMan.Pause) && STATE != "Default")
		{
			if(!trigged) {
				foreach(PauseButton btn in _listButtons)
						btn.trigged=true;
				
				if(STATE=="Menu") {
					foreach(PauseButton btn in _listButtons)
						btn.STATE="Default";
					pauseDefault.position -= new Vector3(0,20,0);
					pauseMainMenu.position += new Vector3(0,20,0);}
				else if(STATE=="Exit") {
					foreach(PauseButton btn in _listButtons)
						btn.STATE="Default";
					pauseDefault.position -= new Vector3(0,20,0);
					pauseExit.position += new Vector3(0,20,0);}
				else if(STATE=="Option") {
					foreach(PauseButton btn in _listButtons)
						btn.STATE="Default";
					pauseDefault.position -= new Vector3(0,20,0);
					pauseOption.position += new Vector3(0,20,0);}
			}
		}
		else if (Input.GetKeyDown(_player.InputMan.Pause) && STATE == "Default" && !trigged)
		{
			if(!first) {
				foreach(PauseButton btn in _listButtons)
				{btn.STATE="Default";btn.first=true;btn.trigged=false;}
				_player.triggerPause();
			}
			else first=!first;
		}
		if(trigged==true) {
			StartCoroutine("reactivateBtn");	
		}
	}
	IEnumerator reactivateBtn() {
		yield return new WaitForSeconds(0.02f);
		trigged = false;
	}
	void OnMouseExit()
	{
		txtButton.color = new Color(1f,1f,1f,1f);
	}
	void OnMouseEnter()
	{
		txtButton.color = new Color(0f,0.4f,0.75f,1f);	
	}
	void OnMouseDown()
	{
		//print(buttonType.ToString());		
		txtButton.color = new Color(1f,1f,1f,1f);
		switch (buttonType)
		{
			case buttonList.None :
			{
				print ("this button does nothing bro" + gameObject.name);
				break;
			}
			case buttonList.Resume :
			{
				_player.triggerPause();
				break;
			}
			case buttonList.Save :
			{
				pauseDefault.position += new Vector3(0,20,0);
				pauseOption.position -= new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Option";
				break;
			}
			case buttonList.OptionNo :
			{
				pauseDefault.position -= new Vector3(0,20,0);
				pauseOption.position += new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Default";
				break;
			}
			case buttonList.OptionYes :
			{
				pauseDefault.position -= new Vector3(0,20,0);
				pauseOption.position += new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Default";
				break;
			}
			case buttonList.Menu :
			{
				//Application.LoadLevel(0);
				pauseDefault.position += new Vector3(0,20,0);
				pauseMainMenu.position -= new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Menu";
				break;
			}
			case buttonList.MenuNo :
			{
				//Application.LoadLevel(0);
				pauseDefault.position -= new Vector3(0,20,0);
				pauseMainMenu.position += new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Default";
				break;
			}
			case buttonList.MenuYes :
			{
				Application.LoadLevel(0);
				break;
			}
			case buttonList.Exit :
			{
				//Application.Quit();
				pauseDefault.position += new Vector3(0,20,0);
				pauseExit.position -= new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Exit";
				break;
			}
			case buttonList.ExitNo :
			{
				//Application.Quit();
				pauseDefault.position -= new Vector3(0,20,0);
				pauseExit.position += new Vector3(0,20,0);
				foreach(PauseButton btn in _listButtons)
					btn.STATE="Default";
				break;
			}
			case buttonList.ExitYes :
			{
				Application.Quit();
				break;
			}
		}
	}
}
