using UnityEngine;
using System.Collections;

public class ArcElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public float activeTime, inactiveTime, delayBegin;
	private float waitTime;
	public bool muted = false, activeState;
	private Player _player;
	
	// Use this for initialization
	void Start () {
		animSprite.Play("arcDefault");
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		activeState = true;
		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		//MasterAudio.PlaySound("piston_idle");
	}
	
	private IEnumerator waitB4Active(bool alternate) {
		yield return new WaitForSeconds(delayBegin);
		if(alternate) StartCoroutine("active");
		else StartCoroutine("activateInfinite");
	}
	private IEnumerator active() {
		if(activeState) {activeState=!activeState;waitTime = activeTime;StartCoroutine("SND_activateThenOff");/*MasterAudio.PlaySound("piston_on");*/}
		else {activeState=!activeState;waitTime = inactiveTime;animSprite.Play("arcDefault");collider.enabled=false; /*MasterAudio.PlaySound("piston_idle");*/}
		yield return new WaitForSeconds(waitTime);
		StartCoroutine("active");
	}
	
	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			MasterAudio.PlaySound("hole");
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
	}
	public void turnOFF () {
		animSprite.Play("arcDefault");collider.enabled=false;
		StopCoroutine("active");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		if(!muted) {MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
			MasterAudio.PlaySound("piston_off");}
	}
	public void turnON () {
		animSprite.Play("arcON");collider.enabled=true;
		activeState = true;
		if(inactiveTime==0) StartCoroutine("activateInfinite");
		else if(activeTime!=0) StartCoroutine("active");
	}
	void GameOver() {
		turnOFF();
	}
	void GameStart() {
		activeState = true;
		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
	}
	IEnumerator activateInfinite() {
		if(!muted) MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("arcON");collider.enabled=true;
		if(!muted) {yield return new WaitForSeconds(0.257f);
			MasterAudio.PlaySound("piston_idle");}
	}
	IEnumerator SND_activateThenOff() {
		if(!muted) MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("arcON");collider.enabled=true;
		if(!muted) {yield return new WaitForSeconds(0.257f);
		MasterAudio.PlaySound("piston_idle");
		yield return new WaitForSeconds(activeTime-0.257f-0.43f);
		MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
		MasterAudio.PlaySound("piston_off");
		yield return new WaitForSeconds(0.43f);
			MasterAudio.StopAllOfSound("piston_idle");}
	}
}
