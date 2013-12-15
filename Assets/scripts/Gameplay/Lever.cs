using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public TriggeredDoor linkedDoor;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Input.GetKey(KeyCode.F))
			{
				triggerLever();
			}
		}
		
	}
	
	void triggerLever()
	{
		//		animSprite.Play("trigger");
		//		ScaleMode = new Vector3(-1,0,0);
		if(linkedDoor.isLocked) {animSprite.Play("unlock");linkedDoor.Unlock();}
		else {animSprite.Play("lock");linkedDoor.Lock();}
		
	}
}
