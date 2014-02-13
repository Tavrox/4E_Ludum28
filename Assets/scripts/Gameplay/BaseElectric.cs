using UnityEngine;
using System.Collections;

public class BaseElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public float activeTime, inactiveTime, delayBegin;
	private float waitTime;
	public bool stateSound = false, activeState;
	// Use this for initialization
	void Start () {
		animSprite.Play("baseDefault");

		activeState = true;
		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
	}
	private IEnumerator waitB4Active(bool alternate) {
		yield return new WaitForSeconds(delayBegin);
		if(alternate) StartCoroutine("active");
		else StartCoroutine("activateInfinite");
	}
	private IEnumerator active() {
		if(activeState) {activeState=!activeState;waitTime = activeTime;StartCoroutine("SND_activateThenOff");}
		else {activeState=!activeState;waitTime = inactiveTime;animSprite.Play("baseDefault");collider.enabled=false;}
		yield return new WaitForSeconds(waitTime);
		StartCoroutine("active");
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
	}
	public void turnOFF () {
		animSprite.Play("baseDefault");collider.enabled=false;
		StopCoroutine("active");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
		MasterAudio.PlaySound("piston_off");
	}
	public void turnON () {
		animSprite.Play("baseON");collider.enabled=true;
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
		MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("baseON");collider.enabled=true;
		yield return new WaitForSeconds(0.257f);
		MasterAudio.PlaySound("piston_idle");
	}
	IEnumerator SND_activateThenOff() {
		MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("baseON");collider.enabled=true;
		yield return new WaitForSeconds(0.257f);
		MasterAudio.PlaySound("piston_idle");
		yield return new WaitForSeconds(activeTime-0.257f-0.43f);
		MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
		MasterAudio.PlaySound("piston_off");
		yield return new WaitForSeconds(0.43f);
		MasterAudio.StopAllOfSound("piston_idle");
	}
}
