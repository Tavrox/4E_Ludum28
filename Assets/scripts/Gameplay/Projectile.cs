using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float ProjectileSpeed;
	public OTAnimatingSprite animSprite;
	//public int dmg;
	private Transform myTransform;
	public Vector3 posIni;
	private bool killedPlay = false;
	private Player _player;

	//[HideInInspector] public Player player;
	
	[HideInInspector] public Vector3 direction;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		animSprite.Play("default");
		_player = GameObject.Find("Player").GetComponent<Player>();
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameStart += GameStart;
		GameEventManager.FinishLevel += FinishLevel;
//		player = GameObject.FindWithTag("Player").GetComponent<PlayerPop>();
//		if(player.shootLeft == true) direction = Vector3.left;
//		else direction = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (animSprite.frameIndex == 5) Destroy(gameObject);
		if(myTransform.position.x > (posIni.x + 70f) || myTransform.position.x < (posIni.x - 70f)) {
			Destroy(gameObject);
		}
		if(myTransform.position.y > (posIni.y + 70f) || myTransform.position.y < (posIni.y - 70f)) {
			Destroy(gameObject);
		}
		
		if(!killedPlay) myTransform.Translate(direction * ProjectileSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			killedPlay = true;
			animSprite.PlayOnce("splash");
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
		if (_other.CompareTag("Crate") == true) {
			StartCoroutine("delayDestroy",0f);
			this.transform.parent = _other.transform;
			//killedPlay = true;
			animSprite.PlayOnce("splash");
		}
		if(_other.CompareTag("Blocker") == true)  {
			StartCoroutine("delayDestroy",0f);
			animSprite.PlayOnce("splash");
			//killedPlay = true;
		}
//		if (_other.CompareTag("Turret") != true)
//	    {
//			Destroy(gameObject);
//		}
	}
	void GameOver () {
		if(this != null && gameObject.activeInHierarchy) {
			StartCoroutine("delayDestroy",0.1f);
			animSprite.PlayOnce("splash");
		}
	}
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy) {
			Destroy(gameObject);
		}
	}
	void FinishLevel () {
		if(this != null && gameObject.activeInHierarchy) {
			Destroy(gameObject);
		}
	}
	IEnumerator delayDestroy(float delay) {
		yield return new WaitForSeconds(delay);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		killedPlay = true;
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
