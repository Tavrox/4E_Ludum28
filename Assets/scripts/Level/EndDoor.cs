﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndDoor : MonoBehaviour {

	public OTSprite sprite;
	public bool triggered;
	public int levelToGo = 0;
	private Player _player;
	private GameObject _UINeedKey, _EndLvlPanel, _EndLvlSprite;
	private Timer _lvlTimer;
	public int _nbKeyRequired=3;
	public List<BatteryLevels> batteriesColor = new List<BatteryLevels>();
	public OTAnimatingSprite _shieldActivateAnim;
	public List<Transform> energyFX = new List<Transform>();
	private LevelManager _lvlManager;
	//private PlayerData _playerdata;
	
	public InputManager InputMan;
	
	
	void Start() {
		_player = GameObject.Find("Player").GetComponent<Player>();
		_UINeedKey = GameObject.Find("Player/IngameUI/NeedKey").gameObject;
		_EndLvlPanel = GameObject.Find("Player/IngameUI/EndLVLPanel").gameObject;
		_EndLvlSprite = GameObject.Find("Player/IngameUI/EndLVLPanel/panel").gameObject;
		if(_EndLvlSprite.transform.localScale.x != 10f) {
			OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartIn).Tween("localScale", new Vector3( 10f, 10f, 1f )).OnFinish(hideEndLvlPanel);
		}
		else _EndLvlPanel.gameObject.SetActive(false);
		_lvlManager = GameObject.Find("Level").GetComponent<LevelManager>();
		_lvlTimer = GameObject.Find("Player/IngameUI/Timer").GetComponent<Timer>();
		//if(GameObject.Find(this.name +"/shieldActivation")!=null) {
		if(_shieldActivateAnim==null) _shieldActivateAnim = GameObject.Find(this.name +"/shieldActivation").GetComponent<OTAnimatingSprite>();
		_shieldActivateAnim.gameObject.name = "shieldActivation"+Random.Range(0,20).ToString();//}
		//_playerdata = _player.GetComponent<PlayerData>();
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.GameStart += GameStart;
		GameEventManager.NextInstance += NextInstance;
		GameEventManager.FinishLevel += FinishLevel;
		InputMan = Instantiate(Resources.Load("Tuning/InputManager")) as InputManager;
		InputMan.Setup();
		sprite.frameIndex = 26;
	}
	public void nextState() {
		sprite.frameIndex += 1;
		batteriesColor[_player.nbKey-1].batteryOK();
		energyFX[_player.nbKey-1].gameObject.SetActive(true);
	}
	public void activeStateReached() {
		_player.hasFinalKey = true;
	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !triggered && !GameEventManager.gamePaused)
		{
			if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2) || Input.GetKey(InputMan.Action3)) && _player.hasFinalKey == true)
			{
				//sprite.frameIndex += 1;
				triggered = _player.finishedLevel = true;
				StartCoroutine("lastFrameBuzzer");
				_shieldActivateAnim.PlayOnce("activate");
				GameEventManager.TriggerFinishLevel();
				//Destroy (_UINeedKey);
				//FinishLevel();
				MasterAudio.PlaySound("key_door");
			}
			else if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2) || Input.GetKey(InputMan.Action3))  && _player.hasFinalKey == false)
			{
				_UINeedKey.GetComponent<IngameUI>().fadeOut();

			}
		}
	}
	private IEnumerator lastFrameBuzzer () {
		yield return new WaitForSeconds(1f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		//sprite.frameIndex += 1;
	}
	private void FinishLevel() {
		if(this != null && gameObject.activeInHierarchy) {
		_lvlTimer.pauseTimer = true;
		MasterAudio.PlaySound("win");
		MasterAudio.FadePlaylistToVolume(0f, 2f);
		MasterAudio.FadeAllPlaylistsToVolume(0f, 2f);
			MasterAudio.FadeOutAllOfSound("bg",2f);
			MasterAudio.FadeOutAllOfSound("intro",2f);
			//MasterAudio.FadeOutAllOfSound("tuto",2f);
			MasterAudio.FadeOutAllOfSound("jam",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_1",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_2",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_3",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_4",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_5",2f);
			MasterAudio.FadeOutAllOfSound("boss_theme",2f);

//		_playerdata.addLevelUnlocked(levelToGo);

		StartCoroutine("EndGame");
		}
	}

	IEnumerator EndGame()
	{
		
//		print (_EndLvlPanel.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Transform>().gameObject.name);
		_EndLvlPanel.gameObject.SetActive(true);
//		_EndLvlPanel.transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,_EndLvlPanel.transform.position.z);
		OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartOut).Tween("localScale", new Vector3( 1f, 1f, 1f ));	
		_lvlManager.updateScore();
		yield return new WaitForSeconds(3f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		MasterAudio.StopAllOfSound("bg");
		MasterAudio.StopAllOfSound("intro");
		//MasterAudio.StopAllOfSound("tuto");
		MasterAudio.StopAllOfSound("jam");
		MasterAudio.StopAllOfSound("level_theme_1");
		MasterAudio.StopAllOfSound("level_theme_2");
		MasterAudio.StopAllOfSound("level_theme_3");
		MasterAudio.StopAllOfSound("level_theme_4");
		MasterAudio.StopAllOfSound("level_theme_5");
		MasterAudio.StopAllOfSound("boss_theme");
					
		//GameEventManager.TriggerNextInstance();
		//Application.LoadLevel(levelToGo);
	}
	private void hideEndLvlPanel (OTTween tween) {
		_EndLvlPanel.gameObject.SetActive(false);
	}
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy) {
			if(_EndLvlSprite.transform.localScale.x != 10f) {
//			_EndLvlPanel.transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,_EndLvlPanel.transform.position.z);
			OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartIn).Tween("localScale", new Vector3( 10f, 10f, 1f )).OnFinish(hideEndLvlPanel);
		}}
	}
	private void NextInstance ()
	{
	}
	private void NextLevel ()
	{
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
