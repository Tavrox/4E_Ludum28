﻿using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	private Vector3 myPosINI, _myScale;
	private Transform _myGameParent, _playerUI;
	private Player _player;
	private LevelManager _levelM;
	//private OTSprite _KeySprite;
	public OTAnimatingSprite _KeySprite;
	public EndDoor _myEndDoor;
	private int _nbKeyRequired;
	private Camera _camera;
	private HUDLevels _myHUDTarget;
	public ParticleSystem _myParticle;
	private OTTween resizeKey, moveKey, scaleKey;

	[ContextMenu ("Setup Frame")]
	private void setFrame()
	{
		_KeySprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		_KeySprite.frameIndex = 0;
	}
		// Use this for initialization
	void Start () {
		_nbKeyRequired = _myEndDoor._nbKeyRequired;
		_player = GameObject.Find("Player").GetComponent<Player>();
		myPosINI = transform.localPosition;
		_myScale = transform.localScale;
		_myGameParent = transform.parent.transform;
		_playerUI = GameObject.Find("Player/IngameUI").transform;
		GameEventManager.GameStart += GameStart;
		GameEventManager.NextInstance += NextInstance;
		GameEventManager.GameOver += GameOver;
		
		_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		_levelM = GameObject.FindObjectOfType<LevelManager>();
		_KeySprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		//_KeySprite.frameIndex = _levelM.chosenVariation+4+25;
		_KeySprite.Play("keyBattery");
		if(_levelM.isBoss == true) _KeySprite.frameIndex = 29;
		_myHUDTarget = GameObject.Find("Player/IngameUI/HUDLevels").GetComponent<HUDLevels>();
		_myParticle = gameObject.GetComponent<ParticleSystem>();
		//InvokeRepeating("rotate",0,0.05f);
	}
	void rotate() {
		_myScale.x = Mathf.PingPong(Time.time, 2)-1;
		transform.localScale = _myScale;
	}
	void GameStart()
	{
		if(this != null && gameObject.activeInHierarchy) {
			resetKey();
		}
	}
	void GameOver() {
		if(this != null && gameObject.activeInHierarchy) 
		resetKey();
	}
	void NextInstance() {
		resetKey();
	}
	void resetKey() {
		if(this != null && gameObject.activeInHierarchy) {
			//_KeySprite.frameIndex = _levelM.chosenVariation+4+25;
//			print (gameObject.name);
			if(resizeKey != null) resizeKey.Stop();
			if(moveKey != null) moveKey.Stop();
			if(scaleKey != null) scaleKey.Stop();
			StopCoroutine("waitB4rescale");
			_KeySprite.Play("keyBattery");
			if(_levelM.isBoss == true) _KeySprite.frameIndex = 29;
			gameObject.transform.parent = _myGameParent;
			transform.localPosition = myPosINI;
			transform.localScale = _myScale;
			_myParticle.simulationSpace = UnityEngine.ParticleSystemSimulationSpace.World;
			_myParticle.enableEmission = true;
			//GetComponentInChildren<OTSprite>().renderer.enabled = true;
			_player.hasFinalKey = false;
		}
	}
	void OnTriggerEnter(Collider _coll)
	{
		if (_coll.name == "Player" && !GameEventManager.gamePaused)
		{
			gameObject.transform.parent = _playerUI;
			MasterAudio.PlaySound("key_collecting");
			_player.nbKey++;
//			transform.localPosition = new Vector3(-11f,5f-_player.nbKey,0f);
//			
//			GameObject StargatePlace = GameObject.FindGameObjectWithTag("SpaceGate");
//			print(gameObject.name+"resize ping pong");
			resizeKey = new OTTween(gameObject.transform, 1f, OTEasing.BackOut).Tween("localScale", new Vector3(1.5f, 1.5f, 1f)).PingPong();
//			new OTTween(gameObject.transform, 1f, OTEasing.CircInOut).Tween("localPosition", new Vector3(_player.nbKey-(0.3f*_player.nbKey),4f,gameObject.transform.position.z));
			moveKey = new OTTween(gameObject.transform, 1f, OTEasing.CircInOut).Tween("localPosition", new Vector3(_myHUDTarget.transform.localPosition.x-2f+_player.nbKey-(0.35f*_player.nbKey), _myHUDTarget.transform.localPosition.y-1.4f,gameObject.transform.position.z));
//			print(gameObject.name+"place HUD");
			if(this != null && gameObject.activeInHierarchy) StartCoroutine("waitB4rescale");
			_myEndDoor.nextState();
			if(_player.nbKey>=_nbKeyRequired) {
				_myEndDoor.activeStateReached();
			}
			//GetComponentInChildren<OTSprite>().renderer.enabled = false;
		}

	}
	IEnumerator waitB4rescale () {
//		print(gameObject.name+"wait b4 rescale");
		yield return new WaitForSeconds(1.005f);
		
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
//		print(gameObject.name+"scale kEY");
		scaleKey = new OTTween(gameObject.transform, .5f, OTEasing.Linear).Tween("localScale", new Vector3(0.65f,0.65f, 1f));
		_myParticle.enableEmission = false;
		_myParticle.simulationSpace = UnityEngine.ParticleSystemSimulationSpace.Local;
		if(_player.nbKey ==5) {				
			foreach(Key k in _player.GetComponentsInChildren<Key>())
			{
				k._myParticle.enableEmission = true;
			}
		}
	}
}