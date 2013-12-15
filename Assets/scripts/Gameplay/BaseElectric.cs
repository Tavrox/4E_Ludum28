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
	private IEnumerator active() {
		cpt++;
		if(cpt % 2 == 0) {waitTime = activeTime;animSprite.Play("baseON");collider.enabled=true;}
		else {waitTime = inactiveTime;animSprite.Play("baseDefault");collider.enabled=false;}
		yield return new WaitForSeconds(waitTime);
		StartCoroutine("active");
	}
}
