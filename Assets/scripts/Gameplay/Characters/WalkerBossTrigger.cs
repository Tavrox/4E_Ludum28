using UnityEngine;
using System.Collections;

public class WalkerBossTrigger : MonoBehaviour {
	
	public WalkerBoss _walker;
	private bool trigged;
	private Player _player;
	// Use this for initialization
	void Start () {
		//_walker = gameObject.GetComponent<Walker>();
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		_player = GameObject.Find("Player").GetComponent<Player>();
		StartCoroutine("blockActivation");

	}
	IEnumerator blockActivation() {
		yield return new WaitForSeconds(0.6f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		_walker.activated = false;
	}
	// Update is called once per frame
	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			_walker.activated = true;
		}
	}
	
	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
		if(this != null && gameObject.activeInHierarchy) {
			StartCoroutine("blockActivation");
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
