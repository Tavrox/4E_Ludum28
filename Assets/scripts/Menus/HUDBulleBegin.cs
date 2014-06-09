using UnityEngine;
using System.Collections;

public class HUDBulleBegin : MonoBehaviour {
	
	private Player _player;
	private bool launched;
	void Start() {
		_player = GameObject.Find("Player").GetComponent<Player>();
		StartCoroutine("lockPlayer");
	}
	IEnumerator lockPlayer() {
		yield return new WaitForSeconds(0.5f);
		launched = true;
		//_player.gameObject.SetActive(false);
	}
	void Update() {
		
		if(launched) _player.locked = true;
		//if(_player.isLeft || _player.isRight || _player.isJump || _player.isCrounch) this.gameObject.SetActive(false);
	}
	void OnMouseDown()
	{
		_player.locked = false;
		//_player.gameObject.SetActive(true);
		this.gameObject.SetActive(false);	
	}
}
