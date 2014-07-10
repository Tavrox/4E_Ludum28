﻿using UnityEngine;
using System.Collections;

public class InvasionAnims : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private Player _player;
	private bool stopped;
	private float ratio;
	// Use this for initialization
	void Start ()
	{
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		animSprite.frameIndex = 0;
		_player = GameObject.Find("Player").GetComponent<Player>();
		stopped = false;
		invade();
		if(!_player.killedByBlob) animSprite.renderer.enabled = false;
		if(FETool.Round(((float) Screen.width/(float) Screen.height),1)!=1.3) {
			ratio = FETool.Round(((float) Screen.width/(float) Screen.height),1) - 1.3f;
			gameObject.transform.localScale += new Vector3(ratio,ratio,0f);
		}
	}
	void Update () {
		if(animSprite.frameIndex == 18 && !stopped) {
			stopped = true;
			animSprite.Stop();
			MasterAudio.PlaySound("rewind");
			_player.angleRotation = 0;
			_player.StartCoroutine("rewind");
			_player.StartCoroutine("stopRewind",1.3f);
			StartCoroutine("reset");
		}
		if ((Input.GetKey(_player.InputMan.Enter)  || Input.GetKey(_player.InputMan.PadSkipDeath))) 
		{
			skipDeathAnim ();
		}
	}
	public IEnumerator reset() {
		//print ("reset");
		yield return new WaitForSeconds(2.25f);//print ("DE-invade");
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		animSprite.PlayBackward("invade");
		_player.transform.position = _player.spawnPos;
		transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,-5f);
		StartCoroutine("finalReset");

	}
	public void invade () {
		//print ("invade");
		transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,-5f);
		_player.transform.position = new Vector3 (_player.transform.position.x,_player.transform.position.y,-20f);
		animSprite.Play("invade");
	}
	public IEnumerator finalReset() {
		yield return new WaitForSeconds(1f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		stopped = false;
		_player.StopCoroutine("rewind");
		_player.StopCoroutine("stopRewind");
		Destroy(gameObject);
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
	void skipDeathAnim () {
		MasterAudio.FadeOutAllOfSound("rewind", 0.5f);
		StopCoroutine("reset");
		StopCoroutine("finalReset");
		_player.StopCoroutine("rewind");
		_player.StopCoroutine("stopRewind");
		_player.StopCoroutine("resetGame");
		_player.thisTransform.rotation = new Quaternion(0f,0f,0f,0f);
		_player.transform.position = _player.spawnPos;
		Destroy(gameObject);
		GameEventManager.TriggerGameStart();
	}
}
