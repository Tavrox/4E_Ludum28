using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crate : MonoBehaviour {

	public bool grounded;
	[HideInInspector] public Transform thisTransform;
	[SerializeField] public float gravityY, crateMove, playerMoveVel, spriteScaleX, spriteScaleY;
	[SerializeField] public Vector3 vectorMove;
	[SerializeField] public RaycastHit hitInfo;
	public Ray detectEndPFLeft, detectEndPFRight, detectPlayerLeft, detectPlayerRight;
	public int blockDetectionArea = 100;
	public Player _player;
	public bool blockCrate, isObjChild;
	public float replaceCrate = 3f;
	private Vector3 spawnPos;
	private bool crateSoundPlaying, crateSoundStopping, touchFloor;
//	private FESound testSon;
	//RaycastHit hitInfo;
	//Ray landingRay;	
	//public float deployHeight;
	
	public virtual void Awake()
	{
		thisTransform = transform;	
	}
	
	public virtual void Start () 
	{
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
//		testSon = gameObject.AddComponent<FESound>();
		spawnPos = transform.position;
		playerMoveVel = _player.moveVel;
		StartCoroutine("StartGravity");
		spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x;
		spriteScaleY = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.y;
		GameEventManager.GameStart += GameStart;
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
		if(!grounded /*&& _player.grounded*/)
		{
			touchFloor = false;
			vectorMove.y -= gravityY * Time.deltaTime;
			thisTransform.position += new Vector3(vectorMove.x,vectorMove.y,0f);
			//print (vectorMove.y);
		}
		detectEndPlatform();
		//if(!blockCrate) detectPlayer();

		//landingRay = new Ray(thisTransform.position, Vector3.down);
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name=="ColliBox" || other.gameObject.CompareTag("Blocker")) 
		{/*print("GAUUUUUUCHE");*/blockCrate = true;}
		if(other.gameObject.tag=="Enemy") {
			//other.gameObject.GetComponent<Patroler>().getDamage(1);
		}
	}
//	void OnTriggerExit(Collider other) {
//		if(other.gameObject.name!="ColliBox" && !other.gameObject.CompareTag("Blocker")) {
//			blockCrate = false;
//		}
//	}
	void OnTriggerStay(Collider other) 
	{
		if(other.gameObject.tag=="Player") {
//		if(other.gameObject.name=="Crate" || other.gameObject.name=="Crate(Clone)") {
//			/*if(detectPlayer())*/ other.gameObject.GetComponent<Crate>().transform.position += new Vector3(crateMove*1.5f/*+0.1f*/,0f,0f);
//		}
			if(Input.GetKey("space") && (_player.transform.position.x < thisTransform.position.x) /*&& !_player.isRight*/) {
				print("Je m'accroche à gauche");
				if(_player.isLeft && !_player.blockedLeft) {//Tire Gauche
					print("Je tire à gauche");
					_player.moveVel = playerMoveVel/2; 
					crateMove = -_player.moveVel*Time.deltaTime;
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
				}
				else if(!blockCrate && _player.isRight) {//Pousse Droite
					print("Je pousse à droite");
					_player.moveVel = playerMoveVel/2; 
					crateMove = _player.moveVel*Time.deltaTime;
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
				}
				else crateMove = 0;
				thisTransform.position += new Vector3(crateMove,0f,0f);
			}
			else if(Input.GetKey("space") && (_player.transform.position.x > thisTransform.position.x) /*&& !_player.isLeft*/) {
				print("Je m'accroche à droite");
				if(_player.isRight && !_player.blockedRight) {//Tire Droite
					print("Je tire à droite");
					_player.moveVel = playerMoveVel/2; 
					crateMove = _player.moveVel*Time.deltaTime;
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
				}
				else if(!blockCrate && _player.isLeft) {//Pousse Gauche
					print("Je pousse à gauche");
					_player.moveVel = playerMoveVel/2; 
					crateMove = -_player.moveVel*Time.deltaTime;
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
				}
				else crateMove = 0;
				thisTransform.position += new Vector3(crateMove,0f,0f);
			}
//			else if(!blockCrate && Input.GetKey("space") && (_player.transform.position.x > thisTransform.position.x) && !_player.isRight) {//Pousse Gauche
//								print("Je m'accroche à droite");
//				if(_player.isLeft) {
//										print("Je pousse à gauche");
//					_player.moveVel = playerMoveVel/2; 
//					crateMove = -_player.moveVel*Time.deltaTime;
//					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
//				}
//				else crateMove = 0;
//				thisTransform.position += new Vector3(crateMove,0f,0f);
//			}
//			else if(!blockCrate && Input.GetKey("space") && (_player.transform.position.x < thisTransform.position.x) && !_player.isLeft) {//Pousse Droite
//								print("Je m'accroche à droite");
//				if(_player.isRight) {
//										print("Je pousse à droite");
//					_player.moveVel = playerMoveVel/2; 
//					crateMove = _player.moveVel*Time.deltaTime;
//					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
//				}
//				else crateMove = 0;
//				thisTransform.position += new Vector3(crateMove,0f,0f);
//			}
//			else if(!blockCrate /*&& _player.grounded */
//			   && !(_player.transform.position.x < thisTransform.position.x && _player.isLeft)
//			   && !(_player.transform.position.x > thisTransform.position.x && _player.isRight)) {
//				_player.moveVel = playerMoveVel/2;
//				if(_player.isRight) {crateMove = _player.moveVel*Time.deltaTime/**2f*/;if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");}
//				else if(_player.isLeft) {crateMove = -_player.moveVel*Time.deltaTime/**2f*/;if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");}
//				else crateMove = 0;
//				thisTransform.position += new Vector3(crateMove,0f,0f);
//				//return true;
//			}
			if((!_player.isRight && !_player.isLeft && crateSoundPlaying && !crateSoundStopping) || blockCrate && crateSoundPlaying && !crateSoundStopping) StartCoroutine("SND_moveCrateEnd");
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
	public IEnumerator SND_moveCrate() {
		crateSoundPlaying = true;
		FESound.playDistancedSound("box_move_start",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("box_move_start");
		yield return new WaitForSeconds(0.885f);
		FESound.playDistancedSound("box_move_idle",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("box_move_idle");
	}
	public IEnumerator SND_moveCrateEnd() {
		crateSoundStopping = true;
		StopCoroutine("SND_moveCrate");
		FESound.playDistancedSound("box_move_stop",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("box_move_stop");
		FESound.playDistancedSound("box_move_start",gameObject.transform, _player.transform,0f,"fade",0.28f);//MasterAudio.FadeOutAllOfSound("box_move_start",0.28f);
		FESound.playDistancedSound("box_move_idle",gameObject.transform, _player.transform,0f,"fade",0.28f);//MasterAudio.FadeOutAllOfSound("box_move_idle",0.28f);
		yield return new WaitForSeconds(0.28f);
		FESound.playDistancedSound("box_move_start",gameObject.transform, _player.transform,0f,"stop");//MasterAudio.StopAllOfSound("box_move_start");
		FESound.playDistancedSound("box_move_idle",gameObject.transform, _player.transform,0f,"stop");//MasterAudio.StopAllOfSound("box_move_idle");
		crateSoundPlaying = crateSoundStopping = false;
	}
	void SND_crateFall() {
		FESound.playDistancedSound("box_fall",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("box_fall");
	}
	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag=="Player") {
			_player.moveVel = playerMoveVel;
			if(crateSoundPlaying && !crateSoundStopping) StartCoroutine("SND_moveCrateEnd");
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
			if(hitInfo.collider.CompareTag("Enemy")) {hitInfo.collider.gameObject.GetComponent<Character>().getDamage(1);} //Kill mob if Crate falls from top
			if(hitInfo.collider.CompareTag("Blocker")) {
			grounded = true;
			vectorMove.y = 0;
				if(!touchFloor) {thisTransform.position = new Vector3(thisTransform.position.x, (float)((hitInfo.collider.bounds.center.y+(hitInfo.collider.bounds.size.y/2f)+spriteScaleY/2f)), thisTransform.position.z);
					touchFloor=true;SND_crateFall();}
			//BoxCollider colliderHit = hitInfo.collider as BoxCollider;
			//print (colliderHit.size);

			}
		}
	}
	void GameStart () {
		if(!isObjChild)	transform.position = new Vector3(spawnPos.x,spawnPos.y,spawnPos.z);
	}
}
