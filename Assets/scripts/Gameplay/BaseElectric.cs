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
		if(activeTime!=0) StartCoroutine("waitB4Active");
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
	}
	private IEnumerator waitB4Active() {
		yield return new WaitForSeconds(delayBegin);
		StartCoroutine("active");
	}
	private IEnumerator active() {
		if(activeState) {activeState=!activeState;waitTime = activeTime;animSprite.Play("baseON");collider.enabled=true;}
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
	}
	public void turnON () {
		animSprite.Play("baseON");collider.enabled=true;
		StartCoroutine("active");
	}
	void GameOver() {
		turnOFF();
	}
	void GameStart() {
		activeState = true;
		StartCoroutine("waitB4Active");
	}
}
