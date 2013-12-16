using UnityEngine;
using System.Collections;

public class TriggeredDoor : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public bool isLocked = true ;

	void Start() {
		animSprite.Play("closed");
		if(!isLocked) Unlock();
	}

	public void Unlock()
	{
//		if (isLocked == true)
//		{
			isLocked = false;
			collider.enabled = false;//Destroy(collider);
			MasterAudio.PlaySound("door_open", 1f, 1f, 0.3f);
			StartCoroutine("WaitUnlock");
//		}
	}
	
	public void Lock()
	{
		//animSprite.Play("lock");
//		animSprite.PlayBackward("unlock");
		isLocked = true;
		collider.enabled = true;
		MasterAudio.PlaySound("door_close");
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
