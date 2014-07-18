using UnityEngine;
using System.Collections;

public class Walker : Enemy {
	
//	[HideInInspector] public Vector3 position;
//	[HideInInspector] public Transform trans;
	private Player _player;
	private Transform myspawnpos;
	private bool walkSoundSwitch;
	private bool activated;
	private BoxCollider [] tabCol;
	private walkerSmokeManager smokeLeft, smokeRight;
	private float smokePosIni;
	private int distanceToObject;
	/***** ENNEMI BEGIN *****/

	void Start()
	{
		base.Start();
		InvokeRepeating("sound",0,0.65f);
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		//myspawnpos.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.z,0f);
		GameEventManager.GameStart += GameStart;
		GameEventManager.FinishLevel += FinishLevel;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		StartCoroutine("waitB4Gravity");
		smokeLeft = gameObject.GetComponentsInChildren<walkerSmokeManager>()[0];
		smokeRight = gameObject.GetComponentsInChildren<walkerSmokeManager>()[1];
		tabCol = gameObject.GetComponents<BoxCollider>();
	}
	private IEnumerator waitB4Gravity() {
		yield return new WaitForSeconds(0.5f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		activated = true;
	}
	// Update is called once per frame
	public void FixedUpdate () 
	{
		if(activated && !GameEventManager.gamePaused) {
			if(chasingPlayer) {ChasePlayer();}
			detectPlayer();
			detectEndPlatform();
			detectWall();
			if(!chasingPlayer) {UpdateMovement();}
		}
	}

	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
		
//		if(this != null && !gameObject.activeInHierarchy && splashed) {
//			splashed = false;
//			//gameObject.transform.parent.gameObject.SetActive(true);
//			gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
//		}
		if(this != null && gameObject.activeInHierarchy) {
			transform.position = new Vector3(spawnPos.x,spawnPos.y,0f);
			enabled = true;
			foreach(BoxCollider box in tabCol) {
				box.enabled = true;
			}
			HP = maxHP;
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
			gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
			splashed = chasingPlayer = activated = false;
			StartCoroutine("waitB4Gravity");
		//}
		}
	}
	private void FinishLevel() {
		if(this != null) {
			enabled = false;
			collider.enabled=false;
		}
	}

	private void sound()
	{
		if(walkSoundSwitch) FESound.playDistancedSound("blob_run1",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound ("blob_run1");
		else FESound.playDistancedSound("blob_run2",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound ("blob_run2");
		walkSoundSwitch = !walkSoundSwitch;
	}

	
	protected void ChasePlayer () {
		//Debug.Log("Px ="+target.position.x+" / Zx ="+myTransform.position.x);
		if (_player.transform.position.x < thisTransform.position.x-(spriteScaleX/10f)) {
			//direction = Vector3.left;
			isLeft = true;
			isRight = false;
			facingDir = facing.Left;
			UpdateMovement();
		}
		else if (_player.transform.position.x > thisTransform.position.x+(spriteScaleX/10f) /*&& isLeft == false*/) {
			//direction = Vector3.right;
			isRight = true; 
			isLeft = false;
			facingDir = facing.Right;
			UpdateMovement();
		}
		else {
			isLeft = isRight = false;
		}
		//myTransform.Translate(direction * movevectorMove * Time.deltaTime);
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			_player.isDead = true;
			_player.killedByBlob = true;
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
			splashed=true;activated=false;
			StartCoroutine("hideAfterSplash",0.79f);
		}
	}
	IEnumerator hideAfterSplash(float delay) {
		foreach(BoxCollider box in tabCol) {
			box.enabled = false;
		}
		yield return new WaitForSeconds(delay);
		gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
		gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
		//gameObject.transform.parent.gameObject.SetActive(false);
	}
	/************************
	 *						*
	 *  DETECTION RAYCASTS 	*
	 *						*
	 ***********************/
//	void OnTriggerEnter (Collider other) {
//		if (other.gameObject.name == "ColliBox") {
//			chasingPlayer=false;
//		}
//	}
	protected void detectPlayer() {
		detectTargetLeft = new Ray(thisTransform.position, Vector3.left);
		detectTargetRight = new Ray(thisTransform.position, Vector3.right);
		Debug.DrawRay(thisTransform.position, Vector3.left*targetDetectionArea);
		Debug.DrawRay(thisTransform.position, Vector3.right*targetDetectionArea);
		
		if (Physics.Raycast(detectTargetLeft, out hitInfo, 1.2f) || Physics.Raycast(detectTargetRight, out hitInfo, 1.2f)) {
			if(hitInfo.collider.tag == "Crate") {
				chasingPlayer = false;
				if(hitInfo.transform.position.x < thisTransform.position.x) {thisTransform.position += new Vector3((spriteScaleX/8f),0f,0f);/*isLeft = false;isRight = true;*/}
				else if (hitInfo.transform.position.x > thisTransform.position.x) {thisTransform.position -= new Vector3((spriteScaleX/8f),0f,0f);/*isLeft = true;isRight = false;*/}
			}
			else checkPlayerRaycast();
		}
		else checkPlayerRaycast();
	}
	protected void detectWall() {
		detectTargetLeft = new Ray(new Vector3(thisTransform.position.x,thisTransform.position.y,thisTransform.position.z), Vector3.left);
		detectTargetRight = new Ray(new Vector3(thisTransform.position.x,thisTransform.position.y,thisTransform.position.z), Vector3.right);
		Debug.DrawRay(new Vector3(thisTransform.position.x,thisTransform.position.y,thisTransform.position.z), Vector3.left*targetDetectionArea, Color.cyan);
		Debug.DrawRay(new Vector3(thisTransform.position.x,thisTransform.position.y,thisTransform.position.z), Vector3.right*targetDetectionArea, Color.black);
		
		if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea)) {//A GAUCHE
			if(hitInfo.collider.tag == "Crate" || hitInfo.collider.name == "ColliBox" || hitInfo.collider.tag=="Blocker") {
				
				if(hitInfo.collider.bounds.center.x < thisTransform.position.x) smokePosIni=hitInfo.collider.bounds.center.x+hitInfo.collider.bounds.size.x/2;
				else smokePosIni=hitInfo.collider.bounds.center.x-hitInfo.collider.bounds.size.x/2;
				
				distanceToObject = (int) Mathf.Abs(smokePosIni+1f-thisTransform.position.x);
//				print (distanceToObject);
				if(smokeLeft.nbSpriteToDisplay!=distanceToObject) {
//				print("GaucheTouche"+distanceToObject);
					smokeLeft.nbSpriteToDisplay=distanceToObject;
					smokeLeft.showNbSmoke();
				}
			}
		}
		else {
			int distanceToObject = smokeLeft.nbSpriteTotal;
			if(smokeLeft.nbSpriteToDisplay!=distanceToObject) {
//				print("GauchePasTouche"+distanceToObject);
				smokeLeft.nbSpriteToDisplay=distanceToObject;
				smokeLeft.showNbSmoke();
			}
		}
		if(Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) { //A DROITE
			if(hitInfo.collider.tag == "Crate" || hitInfo.collider.name == "ColliBox" || hitInfo.collider.tag=="Blocker") {
				
				if(hitInfo.collider.bounds.center.x < thisTransform.position.x) smokePosIni=hitInfo.collider.bounds.center.x+hitInfo.collider.bounds.size.x/2;
				else smokePosIni=hitInfo.collider.bounds.center.x-hitInfo.collider.bounds.size.x/2;
				
				distanceToObject = (int) Mathf.Abs(smokePosIni-1f-thisTransform.position.x);
//				print (distanceToObject);
				if(smokeRight.nbSpriteToDisplay!=distanceToObject) {
//					print("DroiteTouche"+distanceToObject);
					smokeRight.nbSpriteToDisplay=distanceToObject;
					smokeRight.showNbSmoke();
				}
			}
		}
		else {
			int distanceToObject = smokeRight.nbSpriteTotal;
			if(smokeRight.nbSpriteToDisplay!=distanceToObject) {
//				print("DroitePasTouche"+distanceToObject);
				smokeRight.nbSpriteToDisplay=distanceToObject;
				smokeRight.showNbSmoke();
			}
		}
		//else checkPlayerRaycast();
	}
	
	private void checkPlayerRaycast() {
//		print ("CA CHERCHE");
		if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea, (1 << 8) | (1 << 9)) || Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea, (1 << 8) | (1 << 9))) {
//			print ("CA TOUCHE");
			if(hitInfo.collider.name == "ColliBox" || hitInfo.collider.tag=="Blocker") {
//				print ("CHAAArhrt/r88tr*h7*/7*/*ASSSSColliBoxSEE");
				chasingPlayer = false;
				isLeft = isRight = false;
			}
			else if(hitInfo.collider.name == "Player" /*&& !endChasingPlayer*/) {
//				print ("CHAAAASSSSSEE");
				chasingPlayer = true;
			}
			else if(!chasingPlayer) {isLeft = isRight = false;}
			//print(hitInfo);
		}
		if(Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea, (1 << 8) | (1 << 9))) {
			if(hitInfo.collider.name == "Player") {
//				print ("CHAAAASSSSSEE");
				chasingPlayer = true;
			}
		}
		else {
//			print("OLOLOLOLO-*---*/-/*/-/-/-/-*/*-/-*");
			if(!chasingPlayer) {isLeft = isRight = false;}
		}
	}
	protected void detectEndPlatform() {
		detectEndPFLeft = new Ray(new Vector3 (thisTransform.position.x-(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
		detectEndPFRight = new Ray(new Vector3 (thisTransform.position.x+(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
		//print (blockDetectionArea);
		Debug.DrawRay(new Vector3 (thisTransform.position.x-(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down*blockDetectionArea);
		Debug.DrawRay(new Vector3 (thisTransform.position.x+(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down*blockDetectionArea);
		
		if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea) || !Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {
			chasingPlayer = false;
			isLeft = isRight = false;//print("STOOOOOOOOOOOOOOOOOOOOP");
			if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea)) {
				if (Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
					if(hitInfo.collider.name == "Player") {
						chasingPlayer = true;
						if(grounded) thisTransform.position += new Vector3((spriteScaleX/2f),0f,0f);
					}
				}
			}
			if (!Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {			
				if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea)) {
					if(hitInfo.collider.name == "Player") {
						chasingPlayer = true;
						if(grounded) thisTransform.position -= new Vector3((spriteScaleX/2f),0f,0f);
					}
				}
			}
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}

}