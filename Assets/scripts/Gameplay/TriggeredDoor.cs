using UnityEngine;
using System.Collections;

public class TriggeredDoor : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public bool isLocked = true ;

	void Start() {
		animSprite.Play("closed");
	}

	public void Unlock()
	{
//		if (isLocked == true)
//		{
			animSprite.Play("unlock");
			isLocked = false;
			collider.enabled = false;//Destroy(collider);
//		}
	}
	
	public void Lock()
	{
		animSprite.Play("lock");
		isLocked = true;
		collider.enabled = true;
	}
}
