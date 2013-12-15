using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lever : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public List<TriggeredDoor> doors = new List<TriggeredDoor>();

	public enum btnType { Lever, TimedBtn, SequenceBtn }
	public btnType myButtonType;
	public float delay = 0;
	public bool trigged, seqLocked;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F) && !seqLocked)
			{
				trigged = !trigged;
				if(trigged) animSprite.Play("unlock");
				else animSprite.Play("lock");

				if(myButtonType == btnType.SequenceBtn) {seqLocked = true;}
				else {
					triggerLever();
					if(myButtonType == btnType.TimedBtn) {
						StartCoroutine("delayRetrigg");
					}
				}
			}
		}
		
	}
	IEnumerator delayRetrigg() {
		yield return new WaitForSeconds(delay);
		triggerLever();
	}
	public void triggerLever()
	{
		foreach (TriggeredDoor door in doors) {
			if(door.isLocked) {door.Unlock();}
			else {door.Lock();}
		}
	}
	public IEnumerator resetLever () {
		yield return new WaitForSeconds(1f);
		seqLocked = false;
		trigged = false;
		animSprite.Play("lock");
	}
}
