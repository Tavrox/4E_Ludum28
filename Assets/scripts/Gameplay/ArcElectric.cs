using UnityEngine;
using System.Collections;

public class ArcElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	public float activeTime, inactiveTime, delayBegin;
	private float waitTime;
	public bool stateSound = false, activeState;
	private Player _player;
	
	// Use this for initialization
	void Start () {
		animSprite.Play("arcDefault");
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
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
		if(activeState) {activeState=!activeState;waitTime = activeTime;animSprite.Play("arcON");collider.enabled=true;MasterAudio.PlaySound("piston_on");}
		else {activeState=!activeState;waitTime = inactiveTime;animSprite.Play("arcDefault");collider.enabled=false; MasterAudio.PlaySound("piston_idle");}
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
		animSprite.Play("arcDefault");collider.enabled=false;
		StopCoroutine("active");
	}
	public void turnON () {
		animSprite.Play("arcON");collider.enabled=true;
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
