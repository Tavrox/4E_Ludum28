using UnityEngine;
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

		_levelM = GameObject.FindObjectOfType<LevelManager>();
		_KeySprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		//_KeySprite.frameIndex = _levelM.chosenVariation+4+25;
		_KeySprite.Play("keyBattery");
		if(_levelM.isBoss == true) _KeySprite.frameIndex = 29;
		//InvokeRepeating("rotate",0,0.05f);
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
			//_KeySprite.frameIndex = _levelM.chosenVariation+4+25;
			_KeySprite.Play("keyBattery");
			if(_levelM.isBoss == true) _KeySprite.frameIndex = 29;
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
			MasterAudio.PlaySound("key_collecting");
			_player.nbKey++;
//			transform.localPosition = new Vector3(-11f,5f-_player.nbKey,0f);
//			
//					GameObject StargatePlace = GameObject.FindGameObjectWithTag("SpaceGate");
					new OTTween(gameObject.transform, 1f, OTEasing.BackOut).Tween("localScale", new Vector3(1.5f, 1.5f, 1f)).PingPong();
					new OTTween(gameObject.transform, 1f, OTEasing.CircInOut).Tween("localPosition", new Vector3(-11f,5f-_player.nbKey,gameObject.transform.position.z));
			_myEndDoor.nextState();
			if(_player.nbKey>=_nbKeyRequired) {
				_myEndDoor.activeStateReached();
			}
			//GetComponentInChildren<OTSprite>().renderer.enabled = false;
		}

	}
}
