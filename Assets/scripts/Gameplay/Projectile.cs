using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float ProjectileSpeed;
	public OTAnimatingSprite animSprite;
	//public int dmg;
	private Transform myTransform;
	public Vector3 posIni;
	private bool killedPlay = false;

	//[HideInInspector] public Player player;
	
	[HideInInspector] public Vector3 direction;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		animSprite.Play("default");
//		player = GameObject.FindWithTag("Player").GetComponent<PlayerPop>();
//		if(player.shootLeft == true) direction = Vector3.left;
//		else direction = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(direction * ProjectileSpeed * Time.deltaTime);
		
		if(myTransform.position.x > (posIni.x + 70f) || myTransform.position.x < (posIni.x - 70f)) {
			Destroy(gameObject);
		}
		if(myTransform.position.y > (posIni.y + 70f) || myTransform.position.y < (posIni.y - 70f)) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
//		if (_other.CompareTag("Turret") != true)
//	    {
//			Destroy(gameObject);
//		}
	}
}
