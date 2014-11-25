using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crate : MonoBehaviour {

	public bool grounded;
	[HideInInspector] public Transform thisTransform;
	[SerializeField] public float gravityY, _myGravity, crateMove, playerMoveVel, spriteScaleX, spriteScaleY;
	[SerializeField] public Vector3 vectorMove;
	[SerializeField] public RaycastHit hitInfo;
	public Ray detectEndPFLeft, detectEndPFRight, detectPlayerLeft, detectPlayerRight, detectBlockLeft, detectBlockRight;
	public int blockDetectionArea = 100;
	public Player _player;
	public bool blockCrate, isObjChild, touchingPlayer;
	public float replaceCrate = 3f;
	public Vector3 spawnPos;
	private bool crateSoundPlaying, crateSoundStopping, touchFloor;
	public List<Crate> linkedCrates = new List<Crate>();
	public List<Crate> linkedCratesTMP = new List<Crate>();
	public Crate cakeCrate;
	public InputManager InputMan;
	public OTAnimatingSprite sprite;
	public OTSprite spriteS;
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
		spawnPos = transform.localPosition;
		playerMoveVel = _player.moveVel;
		StartCoroutine("StartGravity");
		spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x*thisTransform.localScale.x;
		spriteScaleY = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.y*thisTransform.localScale.y;
		if(isObjChild) {			
			spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x*thisTransform.localScale.x*gameObject.transform.parent.transform.localScale.x;
			spriteScaleY = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.y*thisTransform.localScale.y*gameObject.transform.parent.transform.localScale.y;
		}
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GameUnpause += GameUnpause;
		InputMan = Instantiate(Resources.Load("Tuning/InputManager")) as InputManager;
		InputMan.Setup();
		if(gameObject.GetComponentInChildren<OTAnimatingSprite>()) {
			sprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
			sprite.Stop();
			sprite.frameIndex = 47;
		}
		else if(gameObject.GetComponentInChildren<OTSprite>()) {
			spriteS = gameObject.GetComponentInChildren<OTSprite>();
			spriteS.frameIndex = 25;
		}
	}
	
	IEnumerator StartGravity()
	{
		// wait for things to settle before applying gravity
		yield return new WaitForSeconds(0.1f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		_myGravity = gravityY; //= 0.7f;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//print(grounded);
		if(!grounded && !isObjChild && !GameEventManager.gamePaused/*&& _player.grounded*/)
		{
			touchFloor = false;
			if(vectorMove.y > -1) vectorMove.y -= _myGravity * Time.deltaTime;
			thisTransform.position += new Vector3(vectorMove.x,vectorMove.y,0f);
			//print (vectorMove.y);
		}
		detectEndPlatform();
		if(isObjChild) moveCakeOnSurfPatroler();
		//blockCrate = false;
		//if(!blockCrate) detectPlayer();

		//landingRay = new Ray(thisTransform.position, Vector3.down);
	}
	void moveCakeOnSurfPatroler() {
		if(gameObject.transform.parent.GetComponent<Patroler>().go)	moveCake(-gameObject.transform.parent.GetComponent<Patroler>().myCORRECTSPEED);
		else moveCake(gameObject.transform.parent.GetComponent<Patroler>().myCORRECTSPEED);
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name=="ColliBox" || other.gameObject.CompareTag("Blocker") || other.gameObject.CompareTag("Crate")) 
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
		if(other.gameObject.name=="ColliBox" || other.gameObject.CompareTag("Blocker") || other.gameObject.CompareTag("Crate")) 
		{/*print("GAUUUUUUCHE");*/blockCrate = true;}
		if(other.gameObject.tag=="Player") {
//		if(other.gameObject.name=="Crate" || other.gameObject.name=="Crate(Clone)") {
//			/*if(detectPlayer())*/ other.gameObject.GetComponent<Crate>().transform.position += new Vector3(crateMove*1.5f/*+0.1f*/,0f,0f);
//		}
			touchingPlayer=true;
			if((Input.GetKey(InputMan.Hold) || Input.GetKey(InputMan.Hold2) || Input.GetKey(InputMan.Hold3)) && (_player.transform.position.x < thisTransform.position.x) /*&& !_player.isRight*/) {
				//print("Je m'accroche à gauche");
				if(!blockCrate && _player.isRight) {//Pousse Droite
					//print("Je pousse à droite");
					_player.moveVel = playerMoveVel*.5f; 
					_player.pushCrate = true;
					_player.grabCrate = false;
					crateMove = _player.moveVel*Time.deltaTime;moveCake(_player.moveVel*Time.deltaTime);
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
				}
				else if(_player.isLeft && !_player.blockedLeft && !_player.pushCrate) {//Tire Gauche
					//print("Je tire à gauche");					
//		detectBlockLeft = new Ray(new Vector3 (thisTransform.position.x, thisTransform.position.y-spriteScaleY*.5f+0.5f, thisTransform.position.z), Vector3.left);
//						if(Physics.Raycast(detectBlockLeft, out hitInfo, spriteScaleX*.5f) && hitInfo.collider.CompareTag("Blocker")) {print (hitInfo.collider.CompareTag("Blocker"));
//						}
//					else {
						_player.grabCrate = true;
						_player.pushCrate = false;
						_player.moveVel = playerMoveVel*.5f; 
						crateMove = -_player.moveVel*Time.deltaTime;moveCake(-_player.moveVel*Time.deltaTime);
						if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
//					}
				}
				else crateMove = 0;
				thisTransform.position += new Vector3(crateMove,0f,0f);
			}
			else if((Input.GetKey(InputMan.Hold) || Input.GetKey(InputMan.Hold2) || Input.GetKey(InputMan.Hold3)) && (_player.transform.position.x > thisTransform.position.x) /*&& !_player.isLeft*/) {
				//print("Je m'accroche à droite");
				if(!blockCrate && _player.isLeft) {//Pousse Gauche
				//	print("Je pousse à gauche");
					_player.moveVel = playerMoveVel*.5f; 
					_player.pushCrate = true;
					_player.grabCrate = false;
					crateMove = -_player.moveVel*Time.deltaTime;moveCake(-_player.moveVel*Time.deltaTime);
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
				}
				else if(_player.isRight && !_player.blockedRight && !_player.pushCrate) {//Tire Droite
				//	print("Je tire à droite");
//		detectBlockRight = new Ray(new Vector3 (thisTransform.position.x, thisTransform.position.y-spriteScaleY*.5f+0.5f, thisTransform.position.z), Vector3.right);
//					if(!Physics.Raycast(detectBlockRight, out hitInfo, spriteScaleY*.5f)) {
					_player.moveVel = playerMoveVel*.5f; 
					_player.grabCrate = true;
					_player.pushCrate = false;
					crateMove = _player.moveVel*Time.deltaTime;moveCake(_player.moveVel*Time.deltaTime);
					if(!crateSoundPlaying) StartCoroutine("SND_moveCrate");
//					}
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
			_player.pushCrate = _player.grabCrate = false;
			_player.moveVel = playerMoveVel;
			touchingPlayer=false;
			if(crateSoundPlaying && !crateSoundStopping) StartCoroutine("SND_moveCrateEnd");
		}
		if(other.gameObject.name=="ColliBox" || other.gameObject.CompareTag("Blocker") || other.gameObject.CompareTag("Crate")) 
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
		detectEndPFLeft = new Ray(new Vector3 (thisTransform.position.x-(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
		detectEndPFRight = new Ray(new Vector3 (thisTransform.position.x+(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
		//print (blockDetectionArea);
		Debug.DrawRay(new Vector3 (thisTransform.position.x-(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.down*spriteScaleY*.5f);
		Debug.DrawRay(new Vector3 (thisTransform.position.x+(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.down*spriteScaleY*.5f);
//		Debug.DrawRay(new Vector3 (thisTransform.position.x, thisTransform.position.y-spriteScaleY*.5f+0.5f, thisTransform.position.z), Vector3.left*spriteScaleY*.5f, Color.red);
//		Debug.DrawRay(new Vector3 (thisTransform.position.x, thisTransform.position.y-spriteScaleY*.5f+0.5f, thisTransform.position.z), Vector3.right*spriteScaleY*.5f, Color.blue);
		
		if (!isObjChild) {
			if (!Physics.Raycast(detectEndPFLeft, out hitInfo, spriteScaleY*.5f) && !Physics.Raycast(detectEndPFRight, out hitInfo, spriteScaleY*.5f)) {
				grounded = false;
				cakeCrate = null;
			}
			else {
				if(hitInfo.collider.CompareTag("Enemy")) {/*hitInfo.collider.gameObject.GetComponent<Character>().getDamage(1);*/} //Kill mob if Crate falls from top
				if(hitInfo.collider.CompareTag("Blocker") || hitInfo.collider.CompareTag("Crate")) {
				grounded = true;
				vectorMove.y = 0;
					if(!touchFloor) {
						thisTransform.position = new Vector3(thisTransform.position.x, (float)((hitInfo.collider.bounds.center.y+(hitInfo.collider.bounds.size.y/2f)+spriteScaleY/2.2f)), thisTransform.position.z);
						touchFloor=true;SND_crateFall();
						if(hitInfo.collider.CompareTag("Crate")) {
							//hitInfo.collider.gameObject.transform.parent = transform;
							//hitInfo.collider.gameObject.GetComponent<Crate>().touchFloor = hitInfo.collider.gameObject.GetComponent<Crate>().grounded = true;
							//linkedCrates.Add(hitInfo.collider.gameObject.GetComponent<Crate>());
						}
					}
				//BoxCollider colliderHit = hitInfo.collider as BoxCollider;
				//print (colliderHit.size);

				}
			}
		}
		Debug.DrawRay(new Vector3(thisTransform.position.x-(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.up*spriteScaleY/1.5f, Color.yellow);
		Debug.DrawRay(new Vector3(thisTransform.position.x+(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.up*spriteScaleY/1.5f, Color.yellow);
		//getUpperCrates();
		createCake();

	}
	public void createCake() {
		if( (Physics.Raycast(new Vector3(thisTransform.position.x-(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.up, out hitInfo, spriteScaleY/1.5f) ||
		     Physics.Raycast(new Vector3(thisTransform.position.x+(spriteScaleX/2.5f), thisTransform.position.y, thisTransform.position.z), Vector3.up, out hitInfo, spriteScaleY/1.5f)) && hitInfo.collider.CompareTag("Crate")) {
			cakeCrate = hitInfo.collider.gameObject.GetComponent<Crate>();
		}
		else cakeCrate = null;
	}
	public void moveCake (float moveValue) {
		if (cakeCrate != null && !cakeCrate.blockCrate) {
			cakeCrate.transform.position += new Vector3(moveValue,0f,0f);
			cakeCrate.moveCake(moveValue);
		}
	}
//	public void moveCakeBlob (float moveValue) {
//		if (cakeCrate != null && !cakeCrate.blockCrate) {
//			cakeCrate.transform.position += new Vector3(moveValue,0f,0f);
//			cakeCrate.moveCake(moveValue);
//		}
//	}
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy)	{
			touchingPlayer=false;	
			collider.enabled = true;
			_myGravity = 0f;StartCoroutine("StartGravity");
			_player.moveVel = playerMoveVel;transform.localPosition = new Vector3(spawnPos.x,spawnPos.y,spawnPos.z);
			
			if(gameObject.GetComponentInChildren<OTAnimatingSprite>()) {
				sprite = gameObject.GetComponentInChildren<OTAnimatingSprite>();
				sprite.Stop();sprite.frameIndex = 47;sprite.alpha=1;
			}
			else if(gameObject.GetComponentInChildren<OTSprite>()) {
				spriteS = gameObject.GetComponentInChildren<OTSprite>();
				spriteS.frameIndex = 25;
			}
			foreach(OTAnimatingSprite spr in gameObject.GetComponentsInChildren<OTAnimatingSprite>()) {
				//print(spr.name);
				spr.alpha=1f;
				//if(spr.transform.parent.transform.parent.GetComponent<Crate>()!=null || spr.transform.parent.GetComponent<Crate>()!=null) {sprite.Stop();sprite.frameIndex = 47;}
			}
		}
	}
	void GameOver() {
		if(this != null && gameObject.activeInHierarchy)
			touchingPlayer=false;	
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
	public IEnumerator destroyOnGrounded() {
		yield return new WaitForSeconds(0.1f);
		if(grounded) {
			collider.enabled = false;
			sprite.PlayOnce("destroy");
			foreach(ArcBaseGroup arcBaseGrp in gameObject.GetComponentsInChildren<ArcBaseGroup>()) {
				arcBaseGrp.turnOFF();
			}
			foreach(OTAnimatingSprite spr in gameObject.GetComponentsInChildren<OTAnimatingSprite>()) {
				new OTTween (spr, 1f, OTEasing.Linear).Tween("alpha",0f);
			}
		}
		else StartCoroutine("destroyOnGrounded");
	}
}
