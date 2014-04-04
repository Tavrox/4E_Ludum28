﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[ExecuteInEditMode]

public class Lever : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public List<TriggeredDoor> doors = new List<TriggeredDoor>();

	public enum btnType { Lever, TimedBtn, SequenceBtn }
	public btnType myButtonType;
	public float delay = 0, SND_minDist;
	public bool trigged, seqLocked, stopped;
	private Player _player;
	//private Label test;
	//private Rect _myTimer;
	private float _myRemainingTime;
	//public Vector3 myPos;
	//public GUIText _myCpt;
	public TextMesh _myTimer;
	public InputManager InputMan;

	void Start () {
		if(myButtonType == btnType.TimedBtn) {
			_myTimer = gameObject.GetComponentInChildren<TextMesh>();
			_myRemainingTime = delay;
			_myTimer.text = _myRemainingTime.ToString();
			//GUI.Label(_myTimer, _myRemainingTime.ToString());
			//myPos = Camera.main.WorldToScreenPoint(transform.position);
			/*print(myPos);_myTimer = new Rect(15,15,20,20);*/}
		if(myButtonType == btnType.Lever || myButtonType == btnType.SequenceBtn) { animSprite.frameIndex=2;}
		if(myButtonType == btnType.TimedBtn) { animSprite.frameIndex=1;animSprite.Play("timedlock");}
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		GameEventManager.GameStart += GameStart;
		InputMan = Instantiate(Resources.Load("Tuning/InputManager")) as InputManager;
		InputMan.Setup();
	}
	void Update () {
		//_myTimer= new Rect(myPos.x,myPos.y,20,20);
		if(myButtonType == btnType.TimedBtn) {
			if(animSprite.frameIndex == 0 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("leverTimer");/*StartCoroutine("waitB4Restart",delay/4);*/}
//			if(animSprite.frameIndex == 4 && stopped) {_myRemainingTime = delay;stopped = false;StopCoroutine("leverTimer");}
//			if(animSprite.frameIndex == 5) {StopCoroutine("waitB4Restart");print ("POTS");}
		}
	}
	private IEnumerator leverTimer() {
		_myRemainingTime -= 1;
		_myTimer.text = _myRemainingTime.ToString();
		yield return new WaitForSeconds(1f);
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
			if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2)) && !seqLocked && !(myButtonType == btnType.TimedBtn && trigged))
			{
				collider.enabled=false;
				StartCoroutine("delayReactivate");
				FESound.playDistancedSound("lever",gameObject.transform, _player.transform,SND_minDist);//MasterAudio.PlaySound("lever");
				trigged = !trigged;
				if(trigged) {if(myButtonType == btnType.TimedBtn) animSprite.Play("timedunlock"); else animSprite.Play("unlock");}
				else {if(myButtonType == btnType.TimedBtn){/*MasterAudio.PlaySound("timer_button_alarm");*/ animSprite.Play("timedlock");} else animSprite.Play("lock");}

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
	IEnumerator delayReactivate() {
		yield return new WaitForSeconds(0.2f);
		collider.enabled=true;
	}
	IEnumerator delayRetrigg() {
		yield return new WaitForSeconds(delay);
		triggerLever();
		trigged = false;
		_myRemainingTime = delay;
		_myTimer.text = _myRemainingTime.ToString();
		_myRemainingTime = delay;stopped = false;StopCoroutine("leverTimer");StopCoroutine("waitB4Restart");
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
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy) {
			StopCoroutine("leverTimer");
			StopCoroutine("waitB4Restart");
			StopCoroutine("delayRetrigg");
			StopCoroutine("resetLever");
			collider.enabled=true;
			
			if(myButtonType == btnType.Lever || myButtonType == btnType.SequenceBtn) { animSprite.frameIndex=2;}
			if(myButtonType == btnType.TimedBtn) { 
				_myRemainingTime = delay;
				_myTimer.text = _myRemainingTime.ToString();
				animSprite.frameIndex=1;
				animSprite.Play("timedlock");
			}
			else animSprite.Play("lock");
			stopped = trigged = seqLocked = false;
		}
	}
}
