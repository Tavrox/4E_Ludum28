using UnityEngine;
using System.Collections;

public class BaseElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public float activeTime, inactiveTime, delayBegin;
	private float waitTime;
	public bool muted = false, activeState;
	public Player _player;
	// Use this for initialization
	void Start () {
		animSprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		animSprite.Play("baseDefault");
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
	}
	
	[ContextMenu ("Setup Frame")]
	private void setFrame()
	{
		animSprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		animSprite.frameIndex = 24;
	}

	private IEnumerator waitB4Active(bool alternate) {
		yield return new WaitForSeconds(delayBegin);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
        if (alternate) StartCoroutine("activation");
		else StartCoroutine("activateInfinite");
	}
    private IEnumerator activation()
    {
		if(activeState) {activeState=!activeState;
			if(activeTime>=1) waitTime = activeTime+0.88f;
			else if(activeTime<1) waitTime = activeTime+0.48f;
			//waitTime = activeTime;
			StartCoroutine("SND_activateThenOff");}
		else {activeState=!activeState;
			if(activeTime>=1) waitTime = inactiveTime-0.88f;
			else if(activeTime<1) waitTime = inactiveTime-0.48f;
			//waitTime = inactiveTime;
			StartCoroutine("deactivate");collider.enabled=false;}
		yield return new WaitForSeconds(waitTime);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
        StartCoroutine("activation");
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
		animSprite.Play("baseDefault");collider.enabled=false;
        StopCoroutine("activation");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		if(!muted && gameObject.transform!=null && _player.transform!=null) {FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f,"fade",0.43f);//MasterAudio.FadeOutAllOfSound("piston_idle",0.43f);
			FESound.playDistancedSound("piston_off",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_off");
		}
	}
	public void turnON () {
		animSprite.Play("baseON");collider.enabled=true;//StartCoroutine("activateCollider");
		activeState = true;
		if(inactiveTime==0) StartCoroutine("displayInfinite");
        else if (activeTime != 0) StartCoroutine("activation");
//		if(inactiveTime==0) StartCoroutine("waitB4Active",false);
//		else if(activeTime!=0) StartCoroutine("waitB4Active",true);
	}
	void GameOver() {
		if(this != null && gameObject.activeInHierarchy) {
		//turnOFF();
            StopCoroutine("activation");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		}
	}
	private void FinishLevel() {
		if(this != null) {
            StopCoroutine("activation");
		StopCoroutine("waitB4Active");
		StopCoroutine("activateInfinite");
		StopCoroutine("SND_activateThenOff");
		enabled = false;
		collider.enabled=false;
		}
	}
	void GameStart() {
		if(this != null && gameObject.activeInHierarchy) {
		animSprite.Play("baseDefault");
		activeState = true;
		/*if(delayBegin != 0)*/ collider.enabled=false;
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
	IEnumerator deactivate() {
		animSprite.animation.fps = 18f;
		animSprite.PlayBackward("baseActivation");
		yield return new WaitForSeconds(0.08f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		animSprite.animation.fps = 12.66667f;
		animSprite.Play("baseDefault");
	}
	IEnumerator activateInfinite() {
		animSprite.animation.fps = 15f;
		animSprite.Play("baseAlert");
		yield return new WaitForSeconds(.8f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		animSprite.animation.fps = 18f;
		animSprite.Play("baseActivation");
		yield return new WaitForSeconds(0.08f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		StartCoroutine("displayInfinite");
	}
	IEnumerator displayInfinite() {
		
		if(!muted) FESound.playDistancedSound("piston_on",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		animSprite.animation.fps = 12.66667f;
		animSprite.Play("baseON");StartCoroutine("activateCollider");
		if(!muted) {yield return new WaitForSeconds(0.257f);
			FESound.playDistancedSound("piston_idle",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_idle");
		}
	}
	IEnumerator SND_activateThenOff() {
		
		animSprite.animation.fps = 15f;
		animSprite.Play("baseAlert");
		if(activeTime>=1) yield return new WaitForSeconds(.8f);
		else if(activeTime<1) yield return new WaitForSeconds(.4f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		animSprite.animation.fps = 18f;
		animSprite.Play("baseActivation");
		yield return new WaitForSeconds(0.08f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		if(!muted) FESound.playDistancedSound("piston_on",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("piston_on");
		yield return new WaitForSeconds(0.12f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		animSprite.animation.fps = 12.66667f;
		animSprite.Play("baseON");StartCoroutine("activateCollider");
		if(!muted) {yield return new WaitForSeconds(0.257f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
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
