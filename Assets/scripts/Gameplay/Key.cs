using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	private Vector3 myPosINI, _myScale;
	private Transform _myGameParent, _playerUI;
	private Player _player;
	private LevelManager _levelM;
	private OTSprite _KeySprite;

	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player").GetComponent<Player>();
		myPosINI = transform.localPosition;
		_myScale = transform.localScale;
		_myGameParent = transform.parent.transform;
		_playerUI = GameObject.Find("Player/IngameUI").transform;
		GameEventManager.GameStart += GameStart;
		GameEventManager.NextInstance += NextInstance;
		GameEventManager.GameOver += GameOver;

		_levelM = GameObject.FindObjectOfType<LevelManager>();
		_KeySprite = gameObject.GetComponentInChildren<OTSprite>();
		_KeySprite.frameIndex = _levelM.chosenVariation+4;
		if(_levelM.isBoss == true) _KeySprite.frameIndex = 4;
		InvokeRepeating("rotate",0,0.05f);
	}
	void rotate() {
		_myScale.x = Mathf.PingPong(Time.time, 2)-1;
		transform.localScale = _myScale;
	}
	void GameStart()
	{
		if(this != null && gameObject.activeInHierarchy) resetKey();
	}
	void GameOver() {
		if(this != null && gameObject.activeInHierarchy) 
		resetKey();
	}
	void NextInstance() {
		resetKey();
	}
	void resetKey() {
		if(this != null) {
			
			_KeySprite.frameIndex = _levelM.chosenVariation+4;
			if(_levelM.isBoss == true) _KeySprite.frameIndex = 5;
			gameObject.transform.parent = _myGameParent;
			transform.localPosition = myPosINI;
			//GetComponentInChildren<OTSprite>().renderer.enabled = true;
			_player.hasFinalKey = false;
		}
	}
	void OnTriggerEnter(Collider _coll)
	{
		if (_coll.name == "Player")
		{
			gameObject.transform.parent = _playerUI;
			transform.localPosition = new Vector3(-8.3f,4f,0f);
			MasterAudio.PlaySound("key_collecting");
			_player.nbKey++;
			if(_player.nbKey>2)	_player.hasFinalKey = true;
			//GetComponentInChildren<OTSprite>().renderer.enabled = false;
		}

	}
}
