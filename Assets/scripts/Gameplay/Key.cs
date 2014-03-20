using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	private Vector3 myPosINI;
	private Transform _myGameParent, _playerUI;

	// Use this for initialization
	void Start () {
		myPosINI = transform.localPosition;
		_myGameParent = transform.parent.transform;
		_playerUI = GameObject.Find("Player/IngameUI").transform;
		GameEventManager.GameStart += GameStart;
		GameEventManager.NextInstance += NextInstance;
		GameEventManager.GameOver += GameOver;
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
			gameObject.transform.parent = _myGameParent;
			transform.localPosition = myPosINI;
			//GetComponentInChildren<OTSprite>().renderer.enabled = true;
			GameObject.Find("Player").GetComponent<Player>().hasFinalKey = false;
		}
	}
	void OnTriggerEnter(Collider _coll)
	{
		if (_coll.name == "Player")
		{
			gameObject.transform.parent = _playerUI;
			transform.localPosition = new Vector3(-8.3f,4f,0f);
			MasterAudio.PlaySound("key_collecting");
			GameObject.Find("Player").GetComponent<Player>().hasFinalKey = true;
			//GetComponentInChildren<OTSprite>().renderer.enabled = false;
		}

	}
}
