using UnityEngine;
using System.Collections;

public class Patroler : Character {
	
	// Use this for initialization
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	
	/***** ENNEMI BEGIN *****/
	protected Transform target; //the enemy's target
	
	protected bool chasingPlayer, endChasingPlayer, patroling, canChangeDir=true;
	protected Vector3 direction;
	
	public float targetDetectionArea = 3f;
	public float blockDetectionArea = 2f;
	private float spriteScaleX;
	protected RaycastHit hitInfo; //infos de collision
	protected Ray detectTargetLeft, detectTargetRight, detectBlockLeft, detectBlockRight, detectEndPFLeft, detectEndPFRight; //point de départ, direction
	public float myCORRECTSPEED = 10f;
	protected bool go = true;
	protected int waypointId = 0;
	public Transform[] waypoints;
	
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
		
		InvokeRepeating("sound",0,0.5f);
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		spawnPos = gameObject.transform.position;

		GetComponent<BoxCollider>().size = new Vector3(1.3f,0.7f,30f);
		GetComponent<BoxCollider>().center = new Vector3(0,-0.1f,0f);
		
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
		
		HP = 150;
		res_mag = 50;
		res_phys = 10;
		runSpeed = 0.5f;
		
		target = GameObject.FindWithTag("Player").transform; //target the player
		patroling = true;
		spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x;
	}
	// Update is called once per frame
	void Update () {
		Patrol();
	}
	protected void Patrol () {
		//print ("patrolllll");
		//print (waypointId);
		if(waypoints.Length<=0) print("No Waypoints linked");
		//print(gameObject.transform.position.x+" + "+waypoints[waypointId].position.x);
		//				print(transform.position+" - "+waypoints[waypointId].position);
		//		if (transform.position.x < waypoints[waypointId].position.x+10f && transform.position.x > waypoints[waypointId].position.x-10f) {
		//			//print(gameObject.transform.position.x+" + "+waypoints[waypointId].position.x);
		//			//print ("********** IN *********");
		//		}
		//print(Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)));
		if(Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)) < (spriteScaleX/2f) && canChangeDir) {
			canChangeDir = false;
			go = !go;//print ("*-*-*-*-*-****-*--*-*-*-*-*-*-*-*-*-*-*-*-***-*--*-*-*");
			if(go == true) waypointId=0;
			else waypointId=1;
		}
		else if (Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)) > (spriteScaleX/2f)+1 && !canChangeDir) canChangeDir = true;
		//		
		if(go == true) {
			isRight = false;
			isLeft = true;
			facingDir = facing.Left;
			gameObject.GetComponent<PatrolerAnims>().InvertSprite();
			//UpdateMovement();
			thisTransform.position -= new Vector3(myCORRECTSPEED,0f,0f);
		}
		else {
			isLeft = false;
			isRight = true;
			facingDir = facing.Right;
			gameObject.GetComponent<PatrolerAnims>().NormalScaleSprite();
			//UpdateMovement();
			thisTransform.position += new Vector3(myCORRECTSPEED,0f,0f);
		}
	}
	
	
	protected void OnTriggerEnter(Collider _other) 
	{
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
	}
	
	protected void GameStart () {
		if(FindObjectOfType(typeof(Enemy)) && this != null) {
			transform.localPosition = new Vector3(0f,0f,0f);
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

	private void sound()
	{
		MasterAudio.PlaySound("Blob_run2");
	}
}
