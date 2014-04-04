using UnityEngine;
using System.Collections;

public class Patroler : Character {
	
	// Use this for initialization
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	
	/***** ENNEMI BEGIN *****/
	//protected Transform target; //the enemy's target
	
	protected bool chasingPlayer, endChasingPlayer, patroling, canChangeDir=true;
	protected Vector3 direction;
	
	public float targetDetectionArea = 3f;
	public float blockDetectionArea = 2f;
	//private float spriteScaleX;
	protected RaycastHit hitInfo; //infos de collision
	protected Ray detectTargetLeft, detectTargetRight, detectBlockLeft, detectBlockRight, detectEndPFLeft, detectEndPFRight; //point de départ, direction
	[Range (0,2.25f)] public float myCORRECTSPEED = 10f;
	public bool go = true, touchingCrate;
	public Crate touchedCrate;
	protected int waypointId = 0;
	public Transform[] waypoints;
	private Player _player;
	private bool walkSoundSwitch;
	public bool isVertical,splashed;
	private BoxCollider [] tabCol;
	
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
		
		InvokeRepeating("sound",0,0.65f);
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;

		spawnPos = gameObject.transform.position;
		if (gameObject.name != "PatrolerBoss") {
						GetComponent<BoxCollider> ().size = new Vector3 (1.3f, 0.7f, 30f);
						GetComponent<BoxCollider> ().center = new Vector3 (0, -0.1f, 0f);
				}
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
		
		tabCol = gameObject.GetComponents<BoxCollider>();
		_player = GameObject.FindWithTag("Player").GetComponent<Player>(); //target the player
		patroling = true;
		//spriteScaleX = thisTransform.gameObject.GetComponentInChildren<Transform>().GetComponentInChildren<OTSprite>().transform.localScale.x;
	}
	// Update is called once per frame
	void Update () {
		if(!splashed) Patrol();
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
		if(isVertical) {
			if(Vector3.Distance(new Vector3(0f,transform.position.y,0f), new Vector3(0f,waypoints[waypointId].position.y,0f)) < (spriteScaleX/2f) && canChangeDir)
				revertDirection();
			else if (Vector3.Distance(new Vector3(0f,transform.position.y,0f), new Vector3(0f,waypoints[waypointId].position.y,0f)) > (spriteScaleX/2f)+1 && !canChangeDir)
				canChangeDir = true;
		}
		else {
		if(Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)) < (spriteScaleX/2f) && canChangeDir)
			revertDirection();
		else if (Vector3.Distance(new Vector3(transform.position.x,0f,0f), new Vector3(waypoints[waypointId].position.x,0f,0f)) > (spriteScaleX/2f)+1 && !canChangeDir)
			canChangeDir = true;
		}
		//		
		if(go == true) {
			isRight = false;
			isLeft = true;
			facingDir = facing.Left;
			gameObject.GetComponent<PatrolerAnims>().InvertSprite();
			//UpdateMovement();
			if(isVertical) thisTransform.position -= new Vector3(0f,myCORRECTSPEED,0f);
			else thisTransform.position -= new Vector3(myCORRECTSPEED,0f,0f);
			if(touchingCrate && touchedCrate!=null) moveCrate(go);
		}
		else {
			isLeft = false;
			isRight = true;
			facingDir = facing.Right;
			gameObject.GetComponent<PatrolerAnims>().NormalScaleSprite();
			//UpdateMovement();
			if(isVertical) thisTransform.position += new Vector3(0f,myCORRECTSPEED,0f);
			else thisTransform.position += new Vector3(myCORRECTSPEED,0f,0f);
			if(touchingCrate && touchedCrate!=null) moveCrate(go);
		}
	}
	private void revertDirection() {
		canChangeDir = false;
		go = !go;
		if(go == true) waypointId=0;
		else waypointId=1;
		if(touchingCrate) touchedCrate.gameObject.GetComponent<Crate>().StartCoroutine("SND_moveCrateEnd");
		touchingCrate = false;
		touchedCrate =null;
	}
	protected void OnTriggerExit(Collider _other) 
	{
		if(_other.CompareTag("Crate")) {
			//print (_other.GetInstanceID() +" - other - = - stocked - "+ touchedCrate.GetInstanceID());
//			if(_other.GetInstanceID() == touchedCrate.GetInstanceID()) touchedCrate =null;
			//;
		}
	}
	protected void OnTriggerEnter(Collider _other) 
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			_player.isDead = true;
			_player.killedByBlob = true;
			GameEventManager.TriggerGameOver();
		}
		if(_other.CompareTag("Crate")) {
			if(_other.gameObject.GetComponent<Crate>().grounded) {
				touchingCrate = true;
				if(touchedCrate == null) touchedCrate = _other.gameObject.GetComponent<Crate>();
				if(touchedCrate.transform.position.y>_other.gameObject.transform.position.y) {
					touchedCrate = _other.gameObject.GetComponent<Crate>();
					_other.gameObject.GetComponent<Crate>().StartCoroutine("SND_moveCrate");
				}
			}
			else {getDamage(1);}
		}
	}
//	protected void OnTriggerExit(Collider _other) 
//	{
//		if(_other.CompareTag("Crate")) {
//			touchingCrate = false;
//			touchedCrate = null;
//		}
//	}
	private void moveCrate(bool dirLeft) {
		if(dirLeft) {
			touchedCrate.transform.position -= new Vector3(myCORRECTSPEED,0f,0f);
			touchedCrate.moveCake(-myCORRECTSPEED);
			//if(_player.myCrate == touchedCrate && _player.onCrate) _player.transform.position -= new Vector3(myCORRECTSPEED,0f,0f);
		}
		else {
			touchedCrate.transform.position += new Vector3(myCORRECTSPEED,0f,0f);
			touchedCrate.moveCake(myCORRECTSPEED);
			//if(_player.myCrate == touchedCrate && _player.onCrate) _player.transform.position += new Vector3(myCORRECTSPEED,0f,0f);
		}
	}
	
	public void getDamage(int damage) {
		HP -= damage;
		if(HP <=0) {
			FESound.playDistancedSound("blob_explosion",gameObject.transform, _player.transform,0f);
			splashed=true;
			collider.enabled = false;
			StartCoroutine("hideAfterSplash",0.42f);
		}
	}
	IEnumerator hideAfterSplash(float delay) {
		foreach(BoxCollider box in tabCol) {
			box.enabled = false;
		}
		yield return new WaitForSeconds(delay);
		
		gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
		gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
//		foreach(BoxCollider box in gameObject.GetComponents<BoxCollider>()) {
//			box.enabled = false;
//		}	
//		foreach(MeshRenderer mesh in gameObject.GetComponents<MeshRenderer>()) {
//			mesh.enabled = false;
//		}
		//gameObject.transform.parent.gameObject.SetActive(false);
	}

	protected void GameStart () {
//		if(this != null && !gameObject.activeInHierarchy && splashed) {
//			splashed = false;
//			gameObject.transform.parent.gameObject.SetActive(true);
//		}
		if(this != null && gameObject.activeInHierarchy) {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
			//transform.localPosition = new Vector3(0f,0f,0f);
			foreach(BoxCollider box in tabCol) {
				box.enabled = false;
			}
			
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
			gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
//		foreach(BoxCollider box in gameObject.GetComponents<BoxCollider>()) {
//			box.enabled = true;
//		}	
//		foreach(MeshRenderer mesh in gameObject.GetComponents<MeshRenderer>()) {
//			mesh.enabled = true;
//		}
		transform.position = new Vector3(spawnPos.x,spawnPos.y,0f);
		splashed = false;
		enabled = true;go = true;
		collider.enabled = true;
		touchingCrate = false;waypointId=0;
		touchedCrate =null;
		//}
		}
	}
	
	private void FinishLevel() {
		if(this != null) {
			enabled = false;
			collider.enabled=false;
		}
	}
	protected void GameOver () {
		if(this != null && gameObject.activeInHierarchy) {
		enabled = false;
		touchingCrate = false;
		touchedCrate = null;
//		isLeft = false;
//		isRight = false;
//		isJump = false;
//		isPass = false;
//		movingDir = moving.None;
		}
	}
	protected void GamePause()
	{
		enabled = false;
//		isLeft = false;
//		isRight = false;
//		isJump = false;
//		isPass = false;
//		paused = true;
//		movingDir = moving.None;	
	}
	protected void GameUnpause()
	{
		enabled = true;	
		paused = false;
	}

	private void sound()
	{
		if(walkSoundSwitch) FESound.playDistancedSound("blob_run1",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound ("blob_run1");
		else FESound.playDistancedSound("blob_run2",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound ("blob_run2");
		walkSoundSwitch = !walkSoundSwitch;
	}
}
