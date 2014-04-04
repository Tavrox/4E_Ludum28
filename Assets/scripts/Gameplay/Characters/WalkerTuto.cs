using UnityEngine;
using System.Collections;

public class WalkerTuto : MonoBehaviour {

	public Walker _walker;
	private bool trigged;
	private Player _player;
	// Use this for initialization
	void Start () {
		//_walker = gameObject.GetComponent<Walker>();
		GameEventManager.GameStart += GameStart;
		_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(trigged) _walker.chasingPlayer = true;
	}
	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			trigged = true;
		}
	}
	
	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
		if(this != null && gameObject.activeInHierarchy) {
			trigged = false;
		}
	}
}
