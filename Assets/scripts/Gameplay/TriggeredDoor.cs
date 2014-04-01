using UnityEngine;
using System;
using System.Collections;

public class TriggeredDoor : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public bool isLocked = true ;
	private bool memoryLock;
	private Player _player;
	private Transform thisTransform;

	public virtual void Awake()
	{
		thisTransform = transform;	
	}

	void Start() {
		animSprite.Play("closed");
		//if(!isLocked) Unlock();
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		GameEventManager.GameStart += GameStart;
		if (isLocked)
			memoryLock = true;
		else {
			Unlock();
			memoryLock = false;
		}
	}

	private void GameStart()
	{
		if(this != null && gameObject.activeInHierarchy) {
		isLocked = memoryLock;
			
			if (isLocked)
				Lock();
			else 
				Unlock();
		}
	}
	
	public void Lock()
	{
		//animSprite.Play("lock");
//		animSprite.PlayBackward("unlock");
		isLocked = true;

		FESound.playDistancedSound("door_close",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("door_close");
		StartCoroutine("WaitLock");
	}
	public void Unlock()
	{
		//		if (isLocked == true)
		//		{
		isLocked = false;
		//Destroy(collider);
		FESound.playDistancedSound("door_open", gameObject.transform, _player.transform,0f,"play",0.3f);//MasterAudio.PlaySound("door_open", 1f, 1f, 0.3f);
		StartCoroutine("WaitUnlock");
		//		}
	}

	IEnumerator WaitUnlock()
	{
		yield return new WaitForSeconds(0.3f);collider.enabled = false;
		animSprite.Play("unlock");
	}
	IEnumerator WaitLock()
	{
		yield return new WaitForSeconds(0.3f);collider.enabled = true;
		animSprite.PlayBackward("unlock");
	}
	public void explode() {
		print ("EXPLOOOOOODE");
		collider.enabled = false;
		animSprite.Play("destroy");
	}
}
