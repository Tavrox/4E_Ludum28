using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {


	public Projectile prefabProj;
	public float shootFrequency;
	private Projectile instProj;
	
	public enum shootDir { Right, Left, Up, Down }
	public shootDir myShootDir;

	// Use this for initialization
	void Start () {
		StartCoroutine("waitB4Shoot");
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameStart += GameStart;
	}

	private void GameStart()
	{
		StartCoroutine("waitB4Shoot");
	}

	private void GameOver()
	{
		StopCoroutine("waitB4Shoot");
		StopCoroutine("shoot");
	}

	private IEnumerator waitB4Shoot () {
		yield return new WaitForSeconds(0.35f);
		StartCoroutine("shoot");
	}
	private IEnumerator shoot () {
		yield return new WaitForSeconds(shootFrequency);
		GameObject _proj = Instantiate (Resources.Load("Objects/Projectile")) as GameObject;
		instProj = _proj.GetComponent<Projectile>();
		Player _player = GameObject.Find("Player").GetComponent<Player>();
//		if (transform.position.x < _player.transform.position.x - 800
//		    && transform.position.x > _player.trans.position.x + 800
//		    && transform.position.y < _player.trans.position.x - 600
//		    && transform.position.y > _player.trans.position.x + 600)
//		{
			MasterAudio.PlaySound("turret_shot");
//		}
		switch (myShootDir) {
			case shootDir.Down:
				instProj.direction = Vector3.down;
				break;
			case shootDir.Up:
				instProj.direction = Vector3.up;
				break;
			case shootDir.Left:
				instProj.direction = Vector3.left;
				break;
			case shootDir.Right:
				instProj.direction = Vector3.right;
				break;
		}

		instProj.transform.position = new Vector3(gameObject.transform.position.x,(gameObject.transform.position.y+gameObject.transform.localScale.y/2-0.1f),gameObject.transform.position.z-1);
		instProj.posIni = instProj.transform.position;
		StartCoroutine("shoot");
	}
}
