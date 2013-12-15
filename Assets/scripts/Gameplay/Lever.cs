using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lever : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public List<TriggeredDoor> doors = new List<TriggeredDoor>();

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				triggerLever();
			}
		}
		
	}
	void triggerLever()
	{
		foreach (TriggeredDoor door in doors) {
			if(door.isLocked) {animSprite.Play("unlock");door.Unlock();}
			else {animSprite.Play("lock");door.Lock();}
		}
	}
}
