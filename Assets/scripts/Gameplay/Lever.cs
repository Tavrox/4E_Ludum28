using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]

public class Lever : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public List<TriggeredDoor> doors = new List<TriggeredDoor>();

	public enum btnType { Lever, TimedBtn, SequenceBtn }
	public btnType myButtonType;
	public float delay = 0;
	public bool trigged, seqLocked, stopped;
	private Label test;
	private Rect _myTimer;
	private float _myRemainingTime;
	public Vector3 myPos;
	void OnGUI() {
		if(myButtonType == btnType.TimedBtn) {}
	}
	void Start () {
		if(myButtonType == btnType.TimedBtn) {
			GUI.Label(_myTimer, _myRemainingTime.ToString());
			//myPos = Camera.main.WorldToScreenPoint(transform.position);
			print(myPos);_myTimer = new Rect(15,15,20,20);}
		if(myButtonType == btnType.TimedBtn) { animSprite.Play("timedlock");}
		_myRemainingTime = delay;
	}
	void Update () {
		//_myTimer= new Rect(myPos.x,myPos.y,20,20);
		if(myButtonType == btnType.TimedBtn) {
			if(animSprite.frameIndex == 0 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("leverTimer");StartCoroutine("waitB4Restart",delay/4);}
			if(animSprite.frameIndex == 5 && stopped) {stopped = false;StopCoroutine("leverTimer");StopCoroutine("waitB4Restart");}
		}
	}
	private IEnumerator leverTimer() {
		yield return new WaitForSeconds(1f);
		_myRemainingTime -= 1;
		StartCoroutine("leverTimer");
	}
	private IEnumerator waitB4Restart (float delayRestart) {
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
			if (Input.GetKeyDown(KeyCode.Space) && !seqLocked)
			{
				MasterAudio.PlaySound("lever");
				trigged = !trigged;
				if(trigged) {if(myButtonType == btnType.TimedBtn) animSprite.Play("timedunlock"); else animSprite.Play("unlock");}
				else {if(myButtonType == btnType.TimedBtn){MasterAudio.PlaySound("timer_button_alarm"); animSprite.Play("timedlock");} else animSprite.Play("lock");}

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
		if (gameObject != null)
		{
			foreach (TriggeredDoor door in doors) {
				if(door.isLocked) {door.Unlock();}
				else {door.Lock();}
			}
		}
	}
	public IEnumerator resetLever () {
		yield return new WaitForSeconds(1f);
		seqLocked = false;
		trigged = false;
		if(myButtonType == btnType.TimedBtn) animSprite.Play("timedlock"); else animSprite.Play("lock");
	}
}
