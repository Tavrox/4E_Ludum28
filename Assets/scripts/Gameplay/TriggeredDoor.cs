using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TriggeredDoor : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public bool isLocked = true ;
	private bool memoryLock;
	private Player _player;
	private Transform thisTransform;
	public List<Crate> touchingCrates = new List<Crate>();
	private BoxCollider _myCol;
	public virtual void Awake()
	{
		thisTransform = transform;	
	}

	void Start() {
		animSprite.Play("closed");
		//if(!isLocked) Unlock();
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		_myCol = (BoxCollider)this.collider;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
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
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Crate")) 
			touchingCrates.Add(other.collider.gameObject.GetComponent<Crate>());
	}
	IEnumerator WaitUnlock()
	{
		yield return new WaitForSeconds(0.3f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		collider.enabled = false;
		foreach(Crate c in touchingCrates) c.SendMessage("OnTriggerExit",this.collider); //c.blockCrate = false;
		//touchingCrates.Clear();
		animSprite.Play("unlock");
	}
	IEnumerator WaitLock()
	{
		yield return new WaitForSeconds(0.3f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		collider.enabled = true;
		transform.Translate (Vector3.up * 10000);
		transform.Translate (Vector3.down * 10000);
//		foreach(Crate c in touchingCrates) {
//			c.SendMessage("OnTriggerEnter",this.collider);
//			c.blockCrate = false;
//		}
		animSprite.PlayBackward("unlock");
		yield return new WaitForSeconds(0.2f);		
		if((_player.transform.position.x-thisTransform.position.x)<0 && (_player.transform.position.x-thisTransform.position.x)>-0.77f
		   && Mathf.Abs(Vector2.Distance(_player.transform.position, thisTransform.position))<(5.812499f*this.transform.localScale.y))
			_player.transform.position -= new Vector3(0.77f,0,0);
		else if((_player.transform.position.x-thisTransform.position.x)>=0 && (_player.transform.position.x-thisTransform.position.x)<0.77f
		        && Mathf.Abs(Vector2.Distance(_player.transform.position, thisTransform.position))<(5.812499f*this.transform.localScale.y)) 
			_player.transform.position += new Vector3(0.77f,0,0);
	}
	public void explode() {
		//print ("EXPLOOOOOODE");
<<<<<<< HEAD
=======
		if(!isResistant) {
>>>>>>> b1ba6ab688ea10df7f90144a02ab7c45cf62d8ed
		collider.enabled = false;
		animSprite.Play("destroy");
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
