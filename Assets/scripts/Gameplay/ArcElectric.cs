using UnityEngine;
using System.Collections;

public class ArcElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private int cpt;
	public float activeTime, inactiveTime;
	private float waitTime;
	public bool stateSound = false;
	private Player _player;
	
	// Use this for initialization
	void Start () {
		animSprite.Play("arcDefault");
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		cpt = 0;
		if(activeTime!=0) StartCoroutine("active");
	}

	private IEnumerator active() {
		cpt++;
		if(cpt % 2 == 0) {waitTime = activeTime;animSprite.Play("arcON");collider.enabled=true;MasterAudio.PlaySound("piston_on");}
		else {waitTime = inactiveTime;animSprite.Play("arcDefault");collider.enabled=false; MasterAudio.PlaySound("piston_idle");}
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
}
