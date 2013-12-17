using UnityEngine;
using System.Collections;

public class TurretAnims : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private float turretShootFrequency;
	private bool stopped;
	// Use this for initialization
	void Start ()
	{
		animSprite.Play("attack");
		turretShootFrequency = gameObject.GetComponent<Turret>().shootFrequency;
	}
	void Update() 
	{
		// Order of action matters, they need to have priorities. //
		if(animSprite.frameIndex == 2 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",(float)turretShootFrequency-0.435f);}
	}
	private IEnumerator waitB4Restart (float delayRestart) {
		yield return new WaitForSeconds(delayRestart);
		//print ("attendu");
		stopped = false;
		animSprite.Play(animSprite.frameIndex+1);
	}
}
