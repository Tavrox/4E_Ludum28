using UnityEngine;
using System.Collections;

public class BaseElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private int cpt;
	public float activeTime, inactiveTime;
	private float waitTime;
	// Use this for initialization
	void Start () {
		animSprite.Play("baseDefault");
		cpt = 0;
		StartCoroutine("active");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	new private IEnumerator active() {
		cpt++;
		if(cpt % 2 == 0) {waitTime = activeTime;animSprite.Play("baseON");collider.enabled=true;}
		else {waitTime = inactiveTime;animSprite.Play("baseDefault");collider.enabled=false;}
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
}
