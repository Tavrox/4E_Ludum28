using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lever : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public List<TriggeredDoor> doors = new List<TriggeredDoor>();

	public enum btnType { Lever, TimedBtn, SequenceBtn }
	public btnType myButtonType;
	public float delay = 0;
	public bool trigged, seqLocked, stopped;
	void Start () {
		if(myButtonType == btnType.TimedBtn) { animSprite.Play("timedlock");}
	}
	void Update () {
		if(myButtonType == btnType.TimedBtn) {
			if(animSprite.frameIndex == 0 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",delay/4);}
			if(animSprite.frameIndex == 5 && stopped) {stopped = false;StopCoroutine("waitB4Restart");}
		}
	}
	private IEnumerator waitB4Restart (float delayRestart) {
		print (animSprite.frameIndex+1);
		yield return new WaitForSeconds(delayRestart);
		//animSprite.Play(animSprite.frameIndex+1);
		animSprite.frameIndex = animSprite.frameIndex+1;
		//animSprite.Stop();
		StartCoroutine("waitB4Restart",delay/4);
		//print ("attendu");
	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F) && !seqLocked)
			{
				trigged = !trigged;
				if(trigged) {if(myButtonType == btnType.TimedBtn) animSprite.Play("timedunlock"); else animSprite.Play("unlock");}
				else {if(myButtonType == btnType.TimedBtn) animSprite.Play("timedlock"); else animSprite.Play("lock");}

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
		yield return new WaitForSeconds(delay+delay/4);
		triggerLever();
		trigged = false;
		animSprite.Play("timedlock");
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
		if(myButtonType == btnType.TimedBtn) animSprite.Play("timedlock"); else animSprite.Play("lock");
	}
}
