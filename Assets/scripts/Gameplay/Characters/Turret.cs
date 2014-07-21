using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {


	public Projectile prefabProj;
	public float shootFrequency, shootSpeed;
	private Projectile instProj;
	private Player _player;
	private bool splashed;
	
	public enum shootDir { Right, Left, Up, Down }
	public shootDir myShootDir;
	public int HP = 1;
	
	private BoxCollider [] tabCol;
	
	// Use this for initialization
	void Start () {
		//StartCoroutine("waitB4Shoot");
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		tabCol = gameObject.GetComponents<BoxCollider>();
	}


//	private IEnumerator waitB4Shoot () {
//		yield return new WaitForSeconds(0.35f);
//		StartCoroutine("shoot");
//	}
	public void shoot () {
		if(!splashed) {
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
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			_player.isDead = true;
			GameEventManager.TriggerGameOver();
		}
		if(_other.CompareTag("Crate")) {
			getDamage(1);
		}
	}
	public void getDamage(int damage) {
		HP -= damage;
		if(HP <=0) {
			FESound.playDistancedSound("blob_explosion",gameObject.transform, _player.transform,0f);
			splashed=true;
			collider.enabled = false;
			StartCoroutine("hideAfterSplash",0.42f);
		}
	}
	IEnumerator hideAfterSplash(float delay) {
		gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
		foreach(BoxCollider box in tabCol) {
			box.enabled = false;
		}
		yield return new WaitForSeconds(delay);
		
		gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
//		foreach(BoxCollider box in gameObject.GetComponents<BoxCollider>()) {
//			box.enabled = false;
//		}	
//		foreach(MeshRenderer mesh in gameObject.GetComponents<MeshRenderer>()) {
//			mesh.enabled = false;
//		}
		//gameObject.transform.parent.gameObject.SetActive(false);
	}
	private void GameStart()
	{
		if(this != null && gameObject.activeInHierarchy) {
			//gameObject.GetComponent<TurretAnims>().animSprite.Play();
			//StartCoroutine("waitB4Shoot");
			collider.enabled = true;
			splashed = false;
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
			gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
		}
	}
	void FinishLevel () {
		if(this != null && gameObject.activeInHierarchy) {
			splashed=true;
			collider.enabled = false;
		}
	}

	private void GameOver()
	{
		//gameObject.GetComponent<TurretAnims>().animSprite.frameIndex=0;
		//gameObject.GetComponent<TurretAnims>().animSprite.Stop();
		//gameObject.GetComponent<TurretAnims>().animSprite.StopCoroutine("waitB4Restart");
		//StopCoroutine("waitB4Shoot");
		//StopCoroutine("shoot");
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}

}
