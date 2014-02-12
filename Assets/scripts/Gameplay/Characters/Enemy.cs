using UnityEngine;
using System.Collections;

public class Enemy : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	/***** ENNEMI BEGIN *****/
	protected Transform target; //the enemy's target
	
	protected bool chasingPlayer, endChasingPlayer, patroling,canChangeDir=true;
	protected Vector3 direction;
	
	public float targetDetectionArea = 3f;
	public float blockDetectionArea = 2f;
	protected RaycastHit hitInfo; //infos de collision
	protected Ray detectTargetLeft, detectTargetRight, detectBlockLeft, detectBlockRight, detectEndPFLeft, detectEndPFRight; //point de départ, direction
	
	protected bool go = true;
	protected int waypointId = 0;
	public Transform[] waypoints;

	protected float spriteScaleX, spriteScaleY;
	//	private WaveCreator soundEmitt1, soundEmitt2, soundInstru1, soundInstru2,soundEmitt3;
	//	private int cptWave=1, pebbleDirection = 1;
	//	private bool blockCoroutine, first, toSprint, toWalk, specialCast, playerDirLeft;
	//	private Pebble pebble1;
	//	private float powerPebble;
	//	private GameObject pebbleBar;
	
	[HideInInspector] public bool paused = false;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		
		spawnPos = thisTransform.position;
		
		//		soundEmitt1 = Instantiate(instFootWave) as WaveCreator;
		//		soundEmitt2 = Instantiate(instFootWave) as WaveCreator;
		//		//soundEmitt3 = Instantiate(instFootWave) as WaveCreator;
		//		soundInstru1 = Instantiate(instInstruWave) as WaveCreator;
		//		//soundInstru2 = Instantiate(instInstruWave) as WaveCreator;
		//		soundEmitt1.createCircle(thisTransform);
		//		soundEmitt2.createCircle(thisTransform);
		//		//soundEmitt3.createCircle(thisTransform);
		//		soundInstru1.createCircle(thisTransform);soundInstru1.specialCircle();
		//		//soundInstru2.createCircle(thisTransform);soundInstru2.specialCircle();
		//		
		//		pebbleBar = Instantiate(instPebbleBar) as GameObject;
		
		//enabled = false;
		
		//HP = 150;
		res_mag = 50;
		res_phys = 10;
		runSpeed = 0.5f;
		
		target = GameObject.FindWithTag("Player").transform; //target the player
		patroling = true;
		spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x;
		spriteScaleY = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.y;
	}
	// Update is called once per frame
//	public void Update () 
//	{
//		moveVel = 20f;
//		gravityY = 20f;
//	}
//		//		isLeft = false;
//		//		isRight = false;
//		//		isJump = false;
//		//		isShot = false;
//		//		isPass = false;
//		//		movingDir = moving.None;
//		//
//		Patrol();
//		//		if(!endChasingPlayer) {
//		//			if(chasingPlayer) {ChasePlayer();}
//		//			else if(patroling) {Patrol();}
//		//		}
//		//		else {
//		//			goBackToPatrol();
//		//		}
//		//		checkInput();
//		//		UpdateMovement();
//		//		offsetCircles ();
//		//		detectPlayer();
//		//		detectEndPlatform();
//		//
//		//		if(isLeft || isRight) {
//		//			//print("left&right");
//		//			//if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
//		//		}
//	}
	
	/************************
	 *						*
	 *  DETECTION RAYCASTS 	*
	 *						*
	 ***********************/
//	protected void detectPlayer() {
//		detectTargetLeft = new Ray(thisTransform.position, Vector3.left);
//		detectTargetRight = new Ray(thisTransform.position, Vector3.right);
//		Debug.DrawRay(thisTransform.position, Vector3.left*targetDetectionArea);
//		Debug.DrawRay(thisTransform.position, Vector3.right*targetDetectionArea);
//		
//		if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea) || Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
//			//print ("CA TOUCHE");
//			if(hitInfo.collider.name == "Player" && !endChasingPlayer) {
//				//print ("CHAAAASSSSSEE");
//				chasingPlayer = true;
//			}
//			//print(hitInfo);
//		}
//	}
//	protected void detectEndPlatform() {
//		detectEndPFLeft = new Ray(new Vector3 (thisTransform.position.x-(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
//		detectEndPFRight = new Ray(new Vector3 (thisTransform.position.x+(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down);
//		//print (blockDetectionArea);
//		Debug.DrawRay(new Vector3 (thisTransform.position.x-(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down*blockDetectionArea);
//		Debug.DrawRay(new Vector3 (thisTransform.position.x+(spriteScaleX/2f), thisTransform.position.y, thisTransform.position.z), Vector3.down*blockDetectionArea);
//
//		if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea) || !Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {
//			chasingPlayer = false;
//			isLeft = isRight = false;
//			if (!Physics.Raycast(detectEndPFLeft, out hitInfo, blockDetectionArea)) {
//				if (Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
//					if(hitInfo.collider.name == "Player") {
//						chasingPlayer = true;
//						thisTransform.position += new Vector3(0.1f,0f,0f);
//					}
//				}
//			}
//			if (!Physics.Raycast(detectEndPFRight, out hitInfo, blockDetectionArea)) {			
//				if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea)) {
//					if(hitInfo.collider.name == "Player") {
//						chasingPlayer = true;
//						thisTransform.position -= new Vector3(0.1f,0f,0f);
//					}
//				}
//			}
//		}
//	}
	
	/************************
	 *						*
	 *    IA MANAGEMENT		*
	 *						*
	 ***********************/
	protected void goBackToPatrol () {
		isLeft = false;
		isRight = false;
		
		if(facingDir == facing.Left) {
			StartCoroutine("goToWaypoint",1);
		}
		if(facingDir == facing.Right) {
			StartCoroutine("goToWaypoint",0);
		}
		
	}
//	protected IEnumerator goToWaypoint (int waypointIDToReach) {
//		//print ("goToWaypoint");
//		if(Vector3.Distance(transform.position, waypoints[waypointIDToReach].position) < 1) {
//			endChasingPlayer = false;
//			StopCoroutine("goToWaypoint");
//		}
//		if(waypoints[waypointIDToReach].position.x > transform.position.x) {
//			isRight = true;
//			facingDir = facing.Right;
//		}
//		if(waypoints[waypointIDToReach].position.x < transform.position.x) {
//			isLeft = true;
//			facingDir = facing.Left;
//		}
//		UpdateMovement();
//		yield return new WaitForSeconds(0.1f);
//		StartCoroutine("goToWaypoint",waypointIDToReach);
//	}
//	protected void Patrol () {
//		//print ("patrolllll");
//		//print (waypointId);
//		if(waypoints.Length<=0) print("No Waypoints linked");
//		//print(gameObject.transform.position.x+" + "+waypoints[waypointId].position.x);
//		//		//		print(transform.position+" - "+waypoints[waypointId].position);
//		//		if (transform.position.x < waypoints[waypointId].position.x+10f && transform.position.x > waypoints[waypointId].position.x-10f) {
//		//			//print(gameObject.transform.position.x+" + "+waypoints[waypointId].position.x);
//		//			//print ("********** IN *********");
//		//		}
//		if(Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)) < 30 && canChangeDir) {
//			canChangeDir = false;
//			go = !go;//print ("*-*-*-*-*-****-*--*-*-*-*-*-*-*-*-*-*-*-*-***-*--*-*-*");
//			if(go) waypointId=0;
//			else if (!go) waypointId=1;
//		}
//		else if (Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)) > 40 && !canChangeDir) canChangeDir = true;
//		//		
//		if(go) {
//			isRight = false;
//			isLeft = true;
//			facingDir = facing.Left;//UpdateMovement();
//			gameObject.GetComponent<PatrolerAnims>().InvertSprite();
//			thisTransform.position -= new Vector3(1f,0f,0f);
//		}
//		else {
//			isLeft = false;
//			isRight = true;
//			facingDir = facing.Right;//UpdateMovement();
//			gameObject.GetComponent<PatrolerAnims>().NormalScaleSprite();
//			thisTransform.position += new Vector3(1f,0f,0f);
//		}
//	}
	/************************
	 *						*
	 *  WAVES MANAGEMENT	*
	 *						*
	 ***********************/
	
	//	/* ---- FOOSTEPS ---- */
	//	IEnumerator waitB4FootStep()
	//	{
	//		yield return new WaitForSeconds(0.1f);
	//		if(!blockCoroutine && grounded) StartCoroutine("footStep");
	//	}
	//	IEnumerator footStep()
	//	{
	//		blockCoroutine =true;
	//		
	//		/*if(Input.GetKeyDown("left shift")) {
	////			moveVel = 2 * moveVel;
	////			footStepDelay = footStepDelay / 2;
	//			soundEmitt1.circleWalkToSprint();
	//			soundEmitt2.circleWalkToSprint();
	//		}
	//		if(Input.GetKeyUp("left shift")) {
	////			moveVel = moveVel / 2;
	////			footStepDelay = footStepDelay * 2;
	//			soundEmitt1.circleSprintToWalk();
	//			soundEmitt2.circleSprintToWalk();
	//		}*/
	//		playerDirLeft = (facingDir == facing.Right) ? false : true;
	//		if(cptWave == 1) {cptWave++;soundEmitt1.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);}
	//		else if (cptWave == 2) {cptWave=1;soundEmitt2.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);}
	//		//else if (cptWave == 3) {cptWave=1;soundEmitt3.resetCircle(transform.localScale.x/1.5f,playerDirLeft, true);}
	//		yield return new WaitForSeconds(footStepDelay);
	//		
	//		blockCoroutine = false;
	//	}
	//
	//	/* ---- SPECIAL CIRCLE ---- */
	//	IEnumerator specialCircleCast()
	//	{
	//		specialCast = true;
	//		yield return new WaitForSeconds(1f);
	//		
	//		if(first) {first=first;soundInstru1.resetCircle();}
	//		//else {first=!first;soundInstru2.resetCircle();}
	//		//yield return new WaitForSeconds(soundInstru1.getLifeTime());
	//		specialCast = false;
	//	}
	//	
	//	/* ---- PEBBLE ---- */
	//	private void setPebbleBarPos() {
	//		pebbleBar.transform.position = new Vector3(thisTransform.position.x, thisTransform.position.y,pebbleBar.transform.position.z);
	//	}
	//
	//	/* ---- OTHER ---- */
	//	private void offsetCircles () {
	//		soundEmitt1.setCharacterMoveOffset(vectorFixed.x);
	//		soundEmitt2.setCharacterMoveOffset(vectorFixed.x);
	//		//soundEmitt3.setCharacterMoveOffset(vectorFixed.x);
	//	}
	
	
	
	protected void checkInput()
	{
		// these are false unless one of keys is pressed
		isLeft = false;
		isRight = false;
		isJump = false;
		isGoDown = false;
		isPass = false;
		isCrounch = false;
		
		movingDir = moving.None;
		//facingDir = facing.Left;
		
		// keyboard input
		//		if (Input.GetKeyDown(KeyCode.F))
		//		{
		//			if(!pebble1) {
		//				powerPebble = 0f;
		//				setPebbleBarPos();
		//				pebbleBar.transform.localScale = new Vector3(0f,1f,1f);
		//			}
		//		}
		//		if (Input.GetKey(KeyCode.F))
		//		{
		//			if(powerPebble <=10 && !pebble1) {
		//				powerPebble += 0.2f;
		//				pebbleBar.transform.localScale = new Vector3(powerPebble,1f,1f);
		//				pebbleBar.transform.position = new Vector3((powerPebble-12f)/2,5.5f,-30f);
		//			}
		//		}
		//		if (Input.GetKeyUp(KeyCode.F))
		//		{
		//			if(!pebble1) {
		//				pebble1 = Instantiate(instPebble) as Pebble;
		//				pebble1.setPosition((transform.position.x-transform.localScale.x/2),transform.position.y, -6f);
		//				pebbleDirection = (facingDir == facing.Right) ? 1 : -1;
		//				pebble1.throwPebble(powerPebble, pebbleDirection);
		//				powerPebble = 0f;
		//				pebbleBar.transform.localScale = new Vector3(powerPebble,1f,1f);
		//				pebbleBar.transform.position = new Vector3((powerPebble-12f)/2,5.5f,-30f);
		//			}
		//		}
		//		if (Input.GetKeyDown(KeyCode.Y))
		//		{
		//			//soundInstru2.destroyCircle();
		//		}
		//		if (Input.GetKeyDown(KeyCode.T))
		//		{
		//			soundInstru1.destroyCircle();
		//		}
		//		if (Input.GetKeyDown(KeyCode.R)  && grounded && !specialCast)
		//		{
		//			StartCoroutine("specialCircleCast");
		//		}
		//		if(Input.GetKeyDown("left shift")) {
		//			moveVel = 1.75f * moveVel;
		//			footStepDelay = footStepDelay / 2f;
		//		}
		//		if(Input.GetKeyUp("left shift")) {
		//			moveVel = moveVel / 1.75f;
		//			footStepDelay = footStepDelay * 2f;
		//		}
		//		if(Input.GetKey("left shift")) {
		//			toSprint=true;
		//		}
		//		else if(!Input.GetKey("left shift")) {
		//			toWalk=true;
		//		}
		//		/*if(!blockCoroutine) {*/
		//		if(toSprint) 		{
		//			if(soundEmitt1.getAlpha() <= 0f) soundEmitt1.circleWalkToSprint();
		//			if(soundEmitt2.getAlpha() <= 0f) soundEmitt2.circleWalkToSprint();
		//			if(soundEmitt3.getAlpha() <= 0f) soundEmitt3.circleWalkToSprint();
		//			toSprint=false;
		//		}
		//		else if (toWalk) {
		//			if(soundEmitt1.getAlpha() <= 0f) soundEmitt1.circleSprintToWalk();
		//			if(soundEmitt2.getAlpha() <= 0f) soundEmitt2.circleSprintToWalk();
		//			if(soundEmitt3.getAlpha() <= 0f) soundEmitt3.circleSprintToWalk();
		//			toWalk=false;
		//		}
		//		/*}*/
		//		if(Input.GetKey("left") && !specialCast) 
		//		{ 
		//			isLeft = true;
		//			shootLeft = true;
		//			facingDir = facing.Left;
		//			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		//		}
		//		if((Input.GetKeyUp("left") && !specialCast) || (Input.GetKeyUp("right") && !isLeft && !specialCast)) {
		//			StopCoroutine("footStep");
		//			blockCoroutine = false;
		//		}
		//		if (Input.GetKey("right") && !isLeft && !specialCast) 
		//		{ 
		//			isRight = true; 
		//			facingDir = facing.Right;
		//			shootLeft = false;
		//			if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		//		}
		//		if (Input.GetKey(KeyCode.DownArrow))
		//		{
		//			isCrounch = true;
		//			facingDir = facing.Down;
		//		}
		//		if (Input.GetKeyDown("up")) 
		//		{ 
		//			isJump = true; 
		//		}
		//		if(Input.GetKeyDown("space"))
		//		{
		//			isPass = true;
		//		}
		//		if (Input.GetKeyDown(KeyCode.Alpha1))
		//		{
		//			//skill_axe.useSkill(Skills.SkillList.Axe);
		//		}
		//		if (Input.GetKeyDown(KeyCode.Alpha2))
		//		{
		//			
		//		}
		//		if (Input.GetKeyDown(KeyCode.Alpha3))
		//		{
		//			
		//		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameEventManager.gamePaused == false)
			{
				GameEventManager.TriggerGamePause();
			}
			else if (GameEventManager.gamePaused == true)
			{
				GameEventManager.TriggerGameUnpause();
			}
		}
	}
	
	
	
	
	protected void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player")) 
		{
			GameEventManager.TriggerGameOver();
			chasingPlayer = false;
		}
	}
	
	protected void GameStart () {
		if(FindObjectOfType(typeof(Enemy)) && this != null) {
//			transform.localPosition = spawnPos;
			gameObject.transform.parent.gameObject.SetActive(true);
			enabled = true;
		}
	}
	
	protected void GameOver () {
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		movingDir = moving.None;
	}
	protected void GamePause()
	{
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		paused = true;
		movingDir = moving.None;	
	}
	protected void GameUnpause()
	{
		enabled = true;	
		paused = false;
	}
}


//public class Zombie : Enemy {
//
//		
//		if(chasingPlayer) {ChasePlayer();UpdateMovement();}
//		else {Patrol();UpdateMovement();}
//		//myTransform.position = Vector3.Lerp(myTransform.position, target.position, movevectorMove * Time.deltaTime);
//		
//		detectTargetLeft = new Ray(thisTransform.position, Vector3.left);
//		detectTargetRight = new Ray(thisTransform.position, Vector3.right);
//		Debug.DrawRay(thisTransform.position, Vector3.left*targetDetectionArea);
//		Debug.DrawRay(thisTransform.position, Vector3.right*targetDetectionArea);
//		
//		if (Physics.Raycast(detectTargetLeft, out hitInfo, targetDetectionArea) || Physics.Raycast(detectTargetRight, out hitInfo, targetDetectionArea)) {
//			if(hitInfo.collider.tag == "Player") {
//				chasingPlayer = true;
//			}
//		}
//		
//		detectBlockLeft = new Ray(thisTransform.position, Vector3.left);
//		detectBlockRight = new Ray(thisTransform.position, Vector3.right);
//		Debug.DrawRay(thisTransform.position, Vector3.left*blockDetectionArea);
//		Debug.DrawRay(thisTransform.position, Vector3.right*blockDetectionArea);
//		if (Physics.Raycast(detectBlockLeft, out hitInfo, blockDetectionArea) || Physics.Raycast(detectBlockRight, out hitInfo, blockDetectionArea)) {
//			if(hitInfo.collider.tag == "Boxes") {
//				isJump = true;
//				UpdateMovement();
//			}
//		}
//	}

//	void OnTriggerEnter(Collider other) 
//	{
//		if(other.gameObject.CompareTag("Player")) 
//		{
//			Character ch = other.GetComponent<Character>();
//			if (ch.hasShield == false)
//			{
//				GameEventManager.TriggerGameOver();
//				chasingPlayer = false;
//			}
//		}
//		if(other.gameObject.CompareTag("Bullets")) 
//		{
//			Bullets bull = other.GetComponent<Bullets>();
//			if (bull.bullType != Bullets.bullTopo.Shield)
//			{
//				isShot = true;
//				HP-= bull.Skill.damages;
//			}
//			if(HP <= 0) 
//			{
//				chasingPlayer = false;
//				Destroy(gameObject);
//			}
//		}
//	}

//	private void Patrol () {
//		
//		if(waypoints.Length<=0) print("No Waypoints linked");
//		
//		//		print(Vector3.Distance(waypoints[waypointId].position, transform.position));
//		//		print(go);print(waypointId);
//		//		print(waypoints[waypointId].position+" "+transform.position);
//		
//		if(Vector3.Distance(transform.position, waypoints[waypointId].position) < 1) {
//			go = !go;
//			if(go) waypointId=0;
//			else if (!go) waypointId=1;
//		}
//		
//		//		if(Vector3.Distance(transform.position, waypoints[waypointId].position) < 1) {
//		//			if(go) waypointId++;
//		//			else waypointId--;
//		//			//print(go);
//		//			if(waypointId >= waypoints.Length) {go=false;waypointId--;waypointId--;}
//		//			else if (waypointId <= 0) {go=true;waypointId++;waypointId++;}
//		//		}
//		
//		if(go) {
//			isLeft = true;
//			facingDir = facing.Left;
//		}
//		else {
//			isRight = true;
//			facingDir = facing.Right;
//		}
//	}
//	
//	private void ChasePlayer () {
//		//Debug.Log("Px ="+target.position.x+" / Zx ="+myTransform.position.x);
//		if (target.position.x < thisTransform.position.x) {
//			//direction = Vector3.left;
//			isLeft = true;
//			facingDir = facing.Left;
//		}
//		else if (target.position.x >= thisTransform.position.x && isLeft == false) {
//			//direction = Vector3.right;
//			isRight = true; 
//			facingDir = facing.Right;
//		}
//		//myTransform.Translate(direction * movevectorMove * Time.deltaTime);
//	}
//	private void GameStart () {
//		if(FindObjectOfType(typeof(Zombie)) && this != null) {
//			transform.localPosition = spawnPos;
//			enabled = true;
//		}
//	}
//	
//	private void GameOver () {
//		enabled = false;
//		isLeft = false;
//		isRight = false;
//		isJump = false;
//		isPass = false;
//		movingDir = moving.None;
//	}
//	private void GamePause()
//	{
//		enabled = false;	
//	}
//	private void GameUnpause()
//	{
//		enabled = true;	
//	}
//	//	void OnGUI() {
//	//		Rect rect = new Rect(0,0,250,50);
//	//    	GUI.Box(rect,"This is the text to be displayed");     
//	//    }
//}
