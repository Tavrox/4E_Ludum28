using UnityEngine;
using System.Collections;

public class Walker : Enemy {
	
//	[HideInInspector] public Vector3 position;
//	[HideInInspector] public Transform trans;
	private Player _player;
	private Transform myspawnpos;
	private bool walkSoundSwitch;
	/***** ENNEMI BEGIN *****/

	void Start()
	{
		base.Start();
		InvokeRepeating("sound",0,0.65f);
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
		//myspawnpos.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.z,0f);
		GameEventManager.GameStart += GameStart;
	}

	// Update is called once per frame
	public void Update () 
	{
		if(chasingPlayer) {ChasePlayer();}
		detectPlayer();
		detectEndPlatform();
		UpdateMovement();
	}

	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
			transform.position = new Vector3(spawnPos.x,spawnPos.y,0f);
			enabled = true;
			chasingPlayer = false;
		//}
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
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
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
		}
		else {
			if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea) || Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
				//print ("CA TOUCHE");
				if(hitInfo.collider.name == "ColliBox" || hitInfo.collider.tag=="Blocker") {
					chasingPlayer = false;
					isLeft = isRight = false;
				}
				else if(hitInfo.collider.name == "Player" && !endChasingPlayer) {
					//print ("CHAAAASSSSSEE");
					chasingPlayer = true;
				}
				else if(!chasingPlayer) {isLeft = isRight = false;}
				//print(hitInfo);
			}
			else {
				if(!chasingPlayer) {isLeft = isRight = false;}
			}
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
			isLeft = isRight = false;
			if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea)) {
				if (Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
					if(hitInfo.collider.name == "Player") {
						chasingPlayer = true;
						thisTransform.position += new Vector3((spriteScaleX/2f),0f,0f);
					}
				}
			}
			if (!Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {			
				if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea)) {
					if(hitInfo.collider.name == "Player") {
						chasingPlayer = true;
						thisTransform.position -= new Vector3((spriteScaleX/2f),0f,0f);
					}
				}
			}
		}
	}

}