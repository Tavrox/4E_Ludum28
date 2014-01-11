using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	private bool grounded;
	[HideInInspector] public Transform thisTransform;
	[SerializeField] public float gravityY, crateMove, playerMoveVel, spriteScaleX, spriteScaleY;
	[SerializeField] public Vector3 vectorMove;
	[SerializeField] public RaycastHit hitInfo;
	public Ray detectEndPFLeft, detectEndPFRight, detectPlayerLeft, detectPlayerRight;
	public int blockDetectionArea = 100;
	public Player _player;
	public bool blockCrate;
	public float replaceCrate = 3f;
	//RaycastHit hitInfo;
	//Ray landingRay;	
	//public float deployHeight;
	
	public virtual void Awake()
	{
		thisTransform = transform;	
	}
	
	public virtual void Start () 
	{
		_player = GameObject.Find("Player").GetComponent<Player>();
		playerMoveVel = _player.moveVel;
		StartCoroutine("StartGravity");
		spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x;
		spriteScaleY = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.y;
	}
	
	IEnumerator StartGravity()
	{
		// wait for things to settle before applying gravity
		yield return new WaitForSeconds(0.1f);
		gravityY = 0.2f;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		//print(grounded);
		if(!grounded && _player.grounded)
		{
			vectorMove.y -= gravityY * Time.deltaTime;
			thisTransform.position += new Vector3(vectorMove.x,vectorMove.y,0f);
			//print (vectorMove.y);
		}
		detectEndPlatform();
		//if(!blockCrate) detectPlayer();

		//landingRay = new Ray(thisTransform.position, Vector3.down);
	}
	
	void OnTriggerStay(Collider other) 
	{
		if(other.gameObject.tag=="Player") {
//		if(other.gameObject.name=="Crate" || other.gameObject.name=="Crate(Clone)") {
//			/*if(detectPlayer())*/ other.gameObject.GetComponent<Crate>().transform.position += new Vector3(crateMove*1.5f/*+0.1f*/,0f,0f);
//		}
			if(!blockCrate) {
			_player.moveVel = playerMoveVel/2;
			if(_player.isRight) crateMove = _player.moveVel*Time.deltaTime/**2f*/;
			else if(_player.isLeft) crateMove = -_player.moveVel*Time.deltaTime/**2f*/;
			else crateMove = 0;
			thisTransform.position += new Vector3(crateMove,0f,0f);
			//return true;
		}}
		if(other.gameObject.name=="ColliBox" || other.gameObject.CompareTag("Blocker")) 
		{/*print("GAUUUUUUCHE");*/blockCrate = true;}
		if(other.gameObject.name!="ColliBox" && !other.gameObject.CompareTag("Blocker")) {
			blockCrate = false;
		}
//		if (other.gameObject.tag == "ColliBox") {
//
//		}
//		if(other.gameObject.CompareTag("Player")) {
//
//				if(_player.isRight) crateMove = _player.moveVel*Time.deltaTime;
//				else crateMove = -_player.moveVel*Time.deltaTime;
//				thisTransform.position += new Vector3(crateMove,0f,0f);
//
//			//}
////		}
//		if(other.gameObject.layer == 8)
//		{
//			grounded = true;
//		}
//		if(other.gameObject.CompareTag("Blocker")) 
//		{
//			grounded = true;
//		}
//		if(other.gameObject.CompareTag("Platforms")) 
//		{
//			grounded = true;
//		}
	}
	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag=="Player") {
			_player.moveVel = playerMoveVel;
		}
		if(other.gameObject.name=="ColliBox" || other.gameObject.CompareTag("Blocker")) 
		{/*print("GAUUUUUUCHE");*/blockCrate = false;}
	}
//	private bool detectPlayer() {
//		detectPlayerLeft = new Ray(new Vector3 (thisTransform.position.x, thisTransform.position.y, thisTransform.position.z), Vector3.left);
//		detectPlayerRight = new Ray(new Vector3 (thisTransform.position.x, thisTransform.position.y, thisTransform.position.z), Vector3.right);
//		Debug.DrawRay(new Vector3 (thisTransform.position.x, thisTransform.position.y, thisTransform.position.z), Vector3.left*spriteScaleX/2);
//		Debug.DrawRay(new Vector3 (thisTransform.position.x, thisTransform.position.y, thisTransform.position.z), Vector3.right*spriteScaleX/2);
//		if (Physics.Raycast(detectPlayerLeft, out hitInfo, spriteScaleX)) {
//			//print (hitInfo.collider.gameObject.tag);
//			if(hitInfo.collider.gameObject.tag=="Player") {
//				_player.moveVel = 0.2f*playerMoveVel;
//				if(_player.isRight) crateMove = _player.moveVel*Time.deltaTime*2f;
//				thisTransform.position += new Vector3(crateMove,0f,0f);
//				return true;
//			}
//			if(hitInfo.collider.gameObject.name=="ColliBox" || hitInfo.collider.gameObject.name=="Door") 
//			{print("GAUUUUUUCHE");blockCrate = true;}
//		}
//		if (Physics.Raycast(detectPlayerRight, out hitInfo, spriteScaleX)) {
//			//print (hitInfo.collider.gameObject.tag);
//			if(hitInfo.collider.gameObject.tag=="Player") {
//				_player.moveVel = 0.2f*playerMoveVel;
//				if(_player.isLeft) crateMove = -_player.moveVel*Time.deltaTime*2f;
//				thisTransform.position += new Vector3(crateMove,0f,0f);
//				return true;
//			}
//			if(hitInfo.collider.gameObject.name=="ColliBox" || hitInfo.collider.gameObject.name=="Door") 
//			{print("DROIIIIITE");blockCrate = true;}
//		}
//		return false;
//	}
	private void detectEndPlatform() {
		detectEndPFLeft = new Ray(new Vector3 (thisTransform.position.x-(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
		detectEndPFRight = new Ray(new Vector3 (thisTransform.position.x+(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down);

		//print (blockDetectionArea);
		Debug.DrawRay(new Vector3 (thisTransform.position.x-(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down*spriteScaleY/2f);
		Debug.DrawRay(new Vector3 (thisTransform.position.x+(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down*spriteScaleY/2f);
		
		if (!Physics.Raycast(detectEndPFLeft, out hitInfo, spriteScaleY/2f) && !Physics.Raycast(detectEndPFRight, out hitInfo, spriteScaleY/2f)) {
			grounded = false;
		}
		else {
			grounded = true;
			vectorMove.y = 0;
			//BoxCollider colliderHit = hitInfo.collider as BoxCollider;
			//print (colliderHit.size);
			//thisTransform.position = new Vector3(thisTransform.position.x, (float)((hitInfo.transform.position.y)+replaceCrate), thisTransform.position.z);
		}
	}
}
