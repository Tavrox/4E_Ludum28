using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {


	public Projectile prefabProj;
	public float shootFrequency, shootSpeed;
	private Projectile instProj;
	private Player _player;
	
	public enum shootDir { Right, Left, Up, Down }
	public shootDir myShootDir;

	// Use this for initialization
	void Start () {
		//StartCoroutine("waitB4Shoot");
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}

	private void GameStart()
	{
		//gameObject.GetComponent<TurretAnims>().animSprite.Play();
		//StartCoroutine("waitB4Shoot");
	}

	private void GameOver()
	{
		//gameObject.GetComponent<TurretAnims>().animSprite.frameIndex=0;
		//gameObject.GetComponent<TurretAnims>().animSprite.Stop();
		//gameObject.GetComponent<TurretAnims>().animSprite.StopCoroutine("waitB4Restart");
		//StopCoroutine("waitB4Shoot");
		//StopCoroutine("shoot");
	}

//	private IEnumerator waitB4Shoot () {
//		yield return new WaitForSeconds(0.35f);
//		StartCoroutine("shoot");
//	}
	public void shoot () {
		//yield return new WaitForSeconds(shootFrequency);
		GameObject _proj = Instantiate (Resources.Load("Objects/Projectile")) as GameObject;
		instProj = _proj.GetComponent<Projectile>();
//		if (transform.position.x < _player.transform.position.x - 800
//		    && transform.position.x > _player.trans.position.x + 800
//		    && transform.position.y < _player.trans.position.x - 600
//		    && transform.position.y > _player.trans.position.x + 600)
//		{
		FESound.playDistancedSound("turret_shot",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("turret_shot");
//		}
		switch (myShootDir) {
			case shootDir.Down:
				instProj.direction = Vector3.down;
			instProj.transform.position = new Vector3((gameObject.transform.position.x-gameObject.transform.localScale.x/2-0.1f),gameObject.transform.position.y,gameObject.transform.position.z-1);
				break;
			case shootDir.Up:
				instProj.direction = Vector3.up;
			instProj.transform.position = new Vector3((gameObject.transform.position.x+gameObject.transform.localScale.x/2-0.1f),gameObject.transform.position.y,gameObject.transform.position.z-1);
				break;
			case shootDir.Left:
				instProj.direction = Vector3.left;
				instProj.transform.position = new Vector3(gameObject.transform.position.x,(gameObject.transform.position.y+gameObject.transform.localScale.y/2-0.1f),gameObject.transform.position.z-1);
				break;
			case shootDir.Right:
				instProj.direction = Vector3.right;
			instProj.transform.position = new Vector3(gameObject.transform.position.x,(gameObject.transform.position.y+gameObject.transform.localScale.y/2-0.1f),gameObject.transform.position.z-1);
				break;
		}
		instProj.ProjectileSpeed = shootSpeed;
		instProj.posIni = instProj.transform.position;
		/*StartCoroutine("shoot");*/
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			_player.isDead = true;
			GameEventManager.TriggerGameOver();
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}

}
