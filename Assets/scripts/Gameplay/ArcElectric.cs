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
		animSprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		animSprite.Play("arcDefault");
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		activeState = true;
		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;
		/*if(delayBegin != 0)*/ collider.enabled=false;
		//MasterAudio.PlaySound("piston_idle");
	}
	
	[ContextMenu ("Setup Frame")]
	private void setFrame()
	{
		animSprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		animSprite.frameIndex = 21;
	}

	private IEnumerator waitB4Active(bool alternate) {
		yield return new WaitForSeconds(delayBegin);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		if(alternate) StartCoroutine("active");
		else StartCoroutine("activateInfinite");
	}
	private IEnumerator active() {
		if(activeState) {activeState=!activeState;
			if(activeTime>=1) waitTime = activeTime+0.88f;
			else if(activeTime<1) waitTime = activeTime+0.48f;
			//waitTime = activeTime;
			StartCoroutine("SND_activateThenOff");/*MasterAudio.PlaySound("piston_on");*/}
		else {activeState=!activeState;
			if(activeTime>=1) waitTime = inactiveTime-0.88f;
			else if(activeTime<1) waitTime = inactiveTime-0.48f;
			//waitTime = inactiveTime;
			animSprite.Play("arcDefault");collider.enabled=false; /*MasterAudio.PlaySound("piston_idle");*/}
		yield return new WaitForSeconds(waitTime);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		StartCoroutine("active");
	}
	
	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			FESound.playDistancedSound("hole",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("hole");
			_player.isDead = true;
			_player.killedByLaser = true;
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
			if(gameObject.transform!=null && _player.transform!=null) FESound.playDistancedSound("piston_off",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_off");
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
		/*if(delayBegin != 0)*/ collider.enabled=false;
		activeState = true;
		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
		}
	}
	IEnumerator activateCollider () {
		yield return new WaitForSeconds(0f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		collider.enabled=true;
	}
	IEnumerator activateInfinite() {
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		yield return new WaitForSeconds(.88f);
			if(!muted) FESound.playDistancedSound("piston_on",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("arcON");StartCoroutine("activateCollider");
		if(!muted) {yield return new WaitForSeconds(0.257f);
				FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_idle");
			}
	}
	IEnumerator SND_activateThenOff() {
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		if(activeTime>=1) yield return new WaitForSeconds(.88f);
		else if(activeTime<1) yield return new WaitForSeconds(.48f);
		if(!muted) FESound.playDistancedSound("piston_on",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		animSprite.Play("arcON");StartCoroutine("activateCollider");
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		if(!muted) {yield return new WaitForSeconds(0.257f);
		FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_idle");
		yield return new WaitForSeconds(activeTime-0.257f-0.43f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f,"fade",0.43f);//MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
		FESound.playDistancedSound("piston_off",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_off");
		yield return new WaitForSeconds(0.43f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f,"stop");//MasterAudio.StopAllOfSound("piston_idle");
			}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
