using UnityEngine;
using System.Collections;

public class TriggeredDoor : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public bool isLocked = true ;
	private bool memoryLock;
	private Player _player;

	void Start() {
		animSprite.Play("closed");
		//if(!isLocked) Unlock();
		if (isLocked)
			memoryLock = true;
		else {
			Unlock();
			memoryLock = false;
		}
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		GameEventManager.GameStart += GameStart;
	}

	public void Unlock()
	{
//		if (isLocked == true)
//		{
			isLocked = false;
			collider.enabled = false;//Destroy(collider);
			FESound.playDistancedSound("door_open",gameObject.transform, _player.transform,0f,"play",0.3f);//MasterAudio.PlaySound("door_open", 1f, 1f, 0.3f);
			StartCoroutine("WaitUnlock");
//		}
	}

	private void GameStart()
	{
		isLocked = memoryLock;
		
		if (isLocked)
			Lock();
		else 
			Unlock();
	}
	
	public void Lock()
	{
		//animSprite.Play("lock");
//		animSprite.PlayBackward("unlock");
		isLocked = true;
		collider.enabled = true;
		FESound.playDistancedSound("door_close",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("door_close");
		StartCoroutine("WaitLock");
	}

	IEnumerator WaitUnlock()
	{
		yield return new WaitForSeconds(0.3f);
		animSprite.Play("unlock");
	}
	IEnumerator WaitLock()
	{
		yield return new WaitForSeconds(0.3f);
		animSprite.PlayBackward("unlock");
	}
}
