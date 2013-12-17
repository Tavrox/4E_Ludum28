using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameEventManager.GameStart += GameStart;
	}

	void GameStart()
	{
		GetComponentInChildren<OTSprite>().renderer.enabled = true;
		GameObject.Find("Player").GetComponent<Player>().hasFinalKey = false;
	}

	void OnTriggerEnter(Collider _coll)
	{
		if (_coll.name == "Player")
		{
			MasterAudio.PlaySound("key_collecting");
			GameObject.Find("Player").GetComponent<Player>().hasFinalKey = true;
			GetComponentInChildren<OTSprite>().renderer.enabled = false;
		}

	}
}
