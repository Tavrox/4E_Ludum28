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
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		_myTurret = gameObject.GetComponent<Turret>();
	}
	void Update() 
	{
		if(!GameEventManager.gamePaused) {
		// Order of action matters, they need to have priorities. //
		if(animSprite.frameIndex == 2 && !stopped) {shooting=false;stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",(float)turretShootFrequency-0.435f);}
		if(animSprite.frameIndex == 4 && !shooting) {shooting=true;_myTurret.shoot();}
		}
	}
	private IEnumerator waitB4Restart (float delayRestart) {
		yield return new WaitForSeconds(delayRestart);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		//print ("attendu");
		stopped = false;
		animSprite.Play(animSprite.frameIndex+1);
	}
	private void GameStart()
	{
		if(this != null && gameObject.activeInHierarchy) {
		enabled = true;
		stopped = shooting=false;
		animSprite.Play("attack");
		//StartCoroutine("waitB4Shoot");
		}
	}
	
	private void GameOver()
	{
		if(this != null && gameObject.activeInHierarchy) {
		enabled = false;
		StopCoroutine("waitB4Restart");
		animSprite.Stop();
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
