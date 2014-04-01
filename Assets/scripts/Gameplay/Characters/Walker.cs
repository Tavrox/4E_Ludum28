using UnityEngine;
using System.Collections;

public class Walker : Enemy {
	
//	[HideInInspector] public Vector3 position;
//	[HideInInspector] public Transform trans;
	private Player _player;
	private Transform myspawnpos;
	private bool walkSoundSwitch;
	private bool activated;
	/***** ENNEMI BEGIN *****/

	void Start()
	{
		base.Start();
		InvokeRepeating("sound",0,0.65f);
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		//myspawnpos.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.z,0f);
		GameEventManager.GameStart += GameStart;
		GameEventManager.FinishLevel += FinishLevel;
		StartCoroutine("waitB4Gravity");
	}
	private IEnumerator waitB4Gravity() {
		yield return new WaitForSeconds(0.5f);
		activated = true;
	}
	// Update is called once per frame
	public void Update () 
	{
		if(activated) {
			if(chasingPlayer) {ChasePlayer();}
			detectPlayer();
			detectEndPlatform();
			if(!chasingPlayer) {UpdateMovement();}
		}
	}

	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
		if(this != null && gameObject.activeInHierarchy) {
			transform.position = new Vector3(spawnPos.x,spawnPos.y,0f);
			enabled = true;
			chasingPlayer = activated = false;
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
		if(HP <=0) gameObject.transform.parent.gameObject.SetActive(false);
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
			isLeft = isRight = false;print("STOOOOOOOOOOOOOOOOOOOOP");
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

}