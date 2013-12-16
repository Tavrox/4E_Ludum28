using UnityEngine;
using System.Collections;

public class Walker : Enemy {
	
//	[HideInInspector] public Vector3 position;
//	[HideInInspector] public Transform trans;
	private Player myTarget;
	/***** ENNEMI BEGIN *****/

	void Start()
	{
		InvokeRepeating("sound",0,0.5f);
		myTarget = GameObject.Find("Player").GetComponent<Player>();
	}

	// Update is called once per frame
	public void Update () 
	{
		if(chasingPlayer) {ChasePlayer();}
		detectPlayer();
		detectEndPlatform();
	}

	private void sound()
	{
		MasterAudio.PlaySound("Blob_run2");
	}

	
	protected void ChasePlayer () {
		//Debug.Log("Px ="+target.position.x+" / Zx ="+myTransform.position.x);
		if (myTarget.transform.position.x < thisTransform.position.x-50f) {
			//direction = Vector3.left;
			isLeft = true;
			isRight = false;
			facingDir = facing.Left;
			UpdateMovement();
		}
		else if (myTarget.transform.position.x > thisTransform.position.x+50f /*&& isLeft == false*/) {
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
		
		if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea) || Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
			//print ("CA TOUCHE");
			if(hitInfo.collider.name == "ColliBox" || hitInfo.collider.tag=="Blocker") {
				chasingPlayer = false;
				isLeft = isRight = false;
			}
			if(hitInfo.collider.name == "Player" && !endChasingPlayer) {
				//print ("CHAAAASSSSSEE");
				chasingPlayer = true;
			}
			//print(hitInfo);
		}
	}
	protected void detectEndPlatform() {
		detectEndPFLeft = new Ray(new Vector3 (thisTransform.position.x-45f, thisTransform.position.y, thisTransform.position.z), Vector3.down);
		detectEndPFRight = new Ray(new Vector3 (thisTransform.position.x+45f, thisTransform.position.y, thisTransform.position.z), Vector3.down);
		//print (blockDetectionArea);
		Debug.DrawRay(new Vector3 (thisTransform.position.x-45f, thisTransform.position.y, thisTransform.position.z), Vector3.down*blockDetectionArea);
		Debug.DrawRay(new Vector3 (thisTransform.position.x+45f, thisTransform.position.y, thisTransform.position.z), Vector3.down*blockDetectionArea);
		
		if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea) || !Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {
			chasingPlayer = false;
			isLeft = isRight = false;
			if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea)) {
				if (Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
					if(hitInfo.collider.name == "Player") {
						chasingPlayer = true;
						thisTransform.position += new Vector3(10f,0f,0f);
					}
				}
			}
			if (!Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {			
				if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea)) {
					if(hitInfo.collider.name == "Player") {
						chasingPlayer = true;
						thisTransform.position -= new Vector3(10f,0f,0f);
					}
				}
			}
		}
	}

}