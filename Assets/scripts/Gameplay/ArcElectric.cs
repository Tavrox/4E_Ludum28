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
		GameEventManager.FinishLevel += FinishLevel;
		if(delayBegin != 0) collider.enabled=false;
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
			FESound.playDistancedSound("hole",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("hole");
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
			FESound.playDistancedSound("piston_off",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_off");
		}
	}
	public void turnON () {
		animSprite.Play("arcON");StartCoroutine("activateCollider");
		activeState = true;
		if(inactiveTime==0) StartCoroutine("activateInfinite");
		else if(activeTime!=0) StartCoroutine("active");
	}
	void GameOver() {
		if(this != null && gameObject.activeInHierarchy) {
		//turnOFF();
		StopCoroutine("active");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		}
	}
	private void FinishLevel() {
		if(this != null) {
		StopCoroutine("active");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		enabled = false;
		collider.enabled=false;
		}
	}
	void GameStart() {
		if(this != null && gameObject.activeInHierarchy) {
		animSprite.Play("arcDefault");
		if(delayBegin != 0) collider.enabled=false;
		activeState = true;
		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
		}
	}
	IEnumerator activateCollider () {
		yield return new WaitForSeconds(0f);
		collider.enabled=true;
	}
	IEnumerator activateInfinite() {
		yield return new WaitForSeconds(.88f);
			if(!muted) FESound.playDistancedSound("piston_on",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("arcON");StartCoroutine("activateCollider");
		if(!muted) {yield return new WaitForSeconds(0.257f);
				FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_idle");
			}
	}
	IEnumerator SND_activateThenOff() {
		yield return new WaitForSeconds(.88f);
		if(!muted) FESound.playDistancedSound("piston_on",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("arcON");StartCoroutine("activateCollider");
		if(!muted) {yield return new WaitForSeconds(0.257f);
		FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_idle");
		yield return new WaitForSeconds(activeTime-0.257f-0.43f);
		FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f,"fade",0.43f);//MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
		FESound.playDistancedSound("piston_off",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_off");
		yield return new WaitForSeconds(0.43f);
		FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f,"stop");//MasterAudio.StopAllOfSound("piston_idle");
			}
	}
}
