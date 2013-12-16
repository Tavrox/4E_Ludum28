using UnityEngine;
using System.Collections;

public class ArcElectric : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private int cpt;
	public float activeTime, inactiveTime;
	private float waitTime;
	// Use this for initialization
	void Start () {
		animSprite.Play("arcDefault");
		cpt = 0;
		StartCoroutine("active");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	new private IEnumerator active() {
		cpt++;
		if(cpt % 2 == 0) {waitTime = activeTime;animSprite.Play("arcON");collider.enabled=true;}
		else {waitTime = inactiveTime;animSprite.Play("arcDefault");collider.enabled=false;}
		yield return new WaitForSeconds(waitTime);
		StartCoroutine("active");
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == true)
		{
			GameEventManager.TriggerGameOver();
		}
	}
}
