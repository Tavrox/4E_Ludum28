using UnityEngine;
using System.Collections;

public class TurretAnims : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private float turretShootFrequency;
	private bool stopped, shooting;
	private Turret _myTurret;
	// Use this for initialization
	void Start ()
	{
		animSprite.Play("attack");
		turretShootFrequency = gameObject.GetComponent<Turret>().shootFrequency;
		_myTurret = gameObject.GetComponent<Turret>();
	}
	void Update() 
	{
		// Order of action matters, they need to have priorities. //
		if(animSprite.frameIndex == 2 && !stopped) {shooting=false;stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",(float)turretShootFrequency-0.435f);}
		if(animSprite.frameIndex == 4 && !shooting) {shooting=true;_myTurret.shoot();}
	}
	private IEnumerator waitB4Restart (float delayRestart) {
		yield return new WaitForSeconds(delayRestart);
		//print ("attendu");
		stopped = false;
		animSprite.Play(animSprite.frameIndex+1);
	}
}
