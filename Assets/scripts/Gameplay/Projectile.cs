using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float ProjectileSpeed;
	//public int dmg;
	private Transform myTransform;

	//[HideInInspector] public Player player;
	
	[HideInInspector] public Vector3 direction;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
//		player = GameObject.FindWithTag("Player").GetComponent<PlayerPop>();
//		if(player.shootLeft == true) direction = Vector3.left;
//		else direction = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(direction * ProjectileSpeed * Time.deltaTime);
		
		if(myTransform.position.x > (myTransform.position.x + 15f) || myTransform.position.x < (myTransform.position.x - 15f)) {
			Destroy(gameObject);
		}
	}
}
