using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	[HideInInspector] public bool paused = false;

	public bool hasFinalKey = false;

	public int angleRotation, nbKey;
	public bool isDead = false, locked = false, isTeleport, standing, unCrouch, killedByBlob, killedByLaser, finishedLevel;
	private bool walkSoundLeft;
	private float moveVelIni;
	//private Camera _mainCam;
	//private LevelManager _lvlManager;
	//private Vector3 _spawnVariation;
	public Transform Cam;
	public Vector3 _camPos;
	private OTTween _crouchTween;
	public int _COEFF_TEMPS, _COEFF_BATTERY;
	public int _scorePlayer;
	public Transform HUDPause;
//	public delegate void TweenDelegate();
//	public TweenDelegate otweenFinish;

	private BoxCollider col;
	// Use this for initialization

	public override void Start () 
	{
		base.Start();

		StartCoroutine ("Wait");
		//_mainCam = GameObject.Find("UI/Main Camera").GetComponent<Camera>();
		OTAnimatingSprite _sprite = GetComponentInChildren<Transform>().GetComponentInChildren<OTAnimatingSprite>();
		//_lvlManager = GameObject.Find("Level").GetComponent<LevelManager>();
		//_spawnVariation = GameObject.Find("playerspawn"+_lvlManager.chosenVariation).transform.position;
		_sprite.alpha = 0f;
		OTTween _tween = new OTTween(_sprite,1f).Tween("alpha",1f);
		_crouchTween = new OTTween(_crouchTween,0f);
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;
		Cam = GameObject.Find("UI").GetComponent<Transform>();
		//HUDPause = GameObject.Find("Pause").GetComponent<Transform>();
		//HUDPause.gameObject.SetActive(false);
		InvokeRepeating("playFootstep",0f,0.4f);
		
//		GetComponent<BoxCollider>().size = new Vector3(1.3f,2f,30f);
//		GetComponent<BoxCollider>().center = new Vector3(0f,0f,0f);
		nbKey = 0;
		spawnPos = thisTransform.position;
		isDead =false;
		col = (BoxCollider)this.collider;
		moveVelIni = moveVel;
		//otweenFinish = test;
		//_crouchTween.onTweenFinish = test;
		//otweenFinish();
		//GameObject.Find("Frameworks/OT/View").GetComponent<OTView>().movementTarget = gameObject;
		
		//_mainCam.transform.position = new Vector3(FETool.Round(_spawnVariation.x,2),FETool.Round(_spawnVariation.y,2),0f);

	}
	//public delegate void truc(OTTween tween);
//	public void onTweenFinish() {
//		print("TESTTETST");
//	}
	// Update is called once per frame
	public void FixedUpdate () 
	{
		if(!paused /*GameEventManager.gamePaused*/) {
		unCrouch = false;
		//_mainCam.transform.position = new Vector3(FETool.Round(thisTransform.position.x,2),FETool.Round(thisTransform.position.y,2),0f);
		if (!Input.GetKey(InputMan.Down) && !Input.GetKey(InputMan.Down2) && !isTeleport && standing && !locked)
		{
			standing = false;
			unCrouch = true;
			_crouchTween = new OTTween(Cam.transform ,.4f, OTEasing.QuadInOut).Tween("position", new Vector3( _camPos.x, _camPos.y, _camPos.z ));
			_crouchTween.OnFinish(unlockCrouch);
			//locked = false;
		}

		if(!locked && !isCrounch) {checkInput();}
			UpdateMovement();
		//		offsetCircles ();
		}
		
	}
	private void unlockCrouch (OTTween tween) {
		isCrounch = false;

	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (6f);

	}
	
	private void checkInput()
	{
		if(finishedLevel && grounded) enabled = false; //bloque le perso quand il touche le sol à la fin du niveau
		// these are false unless one of keys is pressed
		isLeft = false;
		isRight = false;
		isJump = false;
		isGoDown = false;
		isPass = false;
		isCrounch = false;
		raycastWidthRatio = 0f;
		//pushCrate = false;
		
		col.size = new Vector3(1f, col.size.y, col.size.z);
		col.center = new Vector3(0f, 0f, 0f);
		movingDir = moving.None;
		if(Input.GetKey(InputMan.Hold) || Input.GetKey(InputMan.Hold2) || Input.GetKey(InputMan.Hold3) || Input.GetKey(InputMan.PadHold)) {
			col.size = new Vector3(1.75f, col.size.y, col.size.z);
			//collider.bounds.size.Set(1.75f, 1.75f, 10f);
			holdCrate();
		}
		if(Input.GetKeyUp(InputMan.Hold) || Input.GetKeyUp(InputMan.Hold2) || Input.GetKeyUp(InputMan.Hold3) || Input.GetKeyUp(InputMan.PadHold)) pushCrate = grabCrate = false;
		if((Input.GetKey(InputMan.Left) || Input.GetKey(InputMan.Left2) || Input.GetAxisRaw("X axis") > InputMan.X_AxisPos_Sensibility ) && !finishedLevel)
		{ 
			_crouchTween.Stop();
			Cam.transform.position = _camPos;
			isLeft = true;
			raycastWidthRatio = 0.25f;
			shootLeft = true;
			facingDir = facing.Left;
		}
		
		if ((Input.GetKey(InputMan.Right) || Input.GetKey(InputMan.Right2) || Input.GetAxisRaw("X axis") < InputMan.X_AxisNeg_Sensibility) && !finishedLevel && !isLeft) 
		{ 
			_crouchTween.Stop();
			Cam.transform.position = _camPos;
			isRight = true; 
			raycastWidthRatio = 0.25f;
			facingDir = facing.Right;
			shootLeft = false;
			//if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if (/*!jumpLocked &&*/ (Input.GetKey(InputMan.Up)  || Input.GetKey(InputMan.Up2) || Input.GetKey(InputMan.Up3) || Input.GetKey(InputMan.PadJump))) 
		{
			_crouchTween.Stop();
			Cam.transform.position = _camPos;
			isJump = true;
		}
		if (!jumpLocked && (Input.GetKeyDown(InputMan.Up)  || Input.GetKeyDown(InputMan.Up2) || Input.GetKeyDown(InputMan.Up3) || Input.GetKeyDown(InputMan.PadJump)) /* && grounded*/) 
		{ 
			MasterAudio.PlaySound("player_jump");
			jumpLocked=true;
		}
		if (Input.GetKeyUp(InputMan.Up) || Input.GetKeyUp(InputMan.Up2) || Input.GetKeyUp(InputMan.Up3) || Input.GetKeyUp(InputMan.PadJump) ) {jumpLocked = false;chute=true;}
		
		if(Input.GetKeyDown(InputMan.Action) || Input.GetKey(InputMan.Action2) || Input.GetKey(InputMan.Action3) || Input.GetKey(InputMan.PadAction))
		{
			isPass = true;
		}
		if (Input.GetKeyDown(InputMan.Pause) || Input.GetKeyDown(InputMan.PadPause))
		{
			if (GameEventManager.gamePaused == false) HUDPause.gameObject.SetActive(true);
			triggerPause();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameObject.Find("Invasion").GetComponent<InvasionAnims>().invade();
		}
		if (Input.GetKeyDown(InputMan.Reset) || Input.GetKey(InputMan.Reset2) || Input.GetKey(InputMan.PadReset)) 
		{
			resetLevel();
		}
			
		if ((Input.GetKey(InputMan.Down) || Input.GetKey(InputMan.Down2)) && grounded)
		{
			_camPos = Cam.transform.position;
			_crouchTween = new OTTween(Cam.transform ,.4f, OTEasing.QuadInOut).Tween("position", new Vector3( Cam.transform.position.x, Cam.transform.position.y-4, Cam.transform.position.z ));
			isCrounch = standing = true;
			isRight = isLeft = false;
		}

		/*
		 * if((Input.GetKeyUp("left") && !specialCast) || (Input.GetKeyUp("right") && !isLeft && !specialCast)) {
		 * StopCoroutine("footStep");
		 * blockCoroutine = false;
		}*/
		/*if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");*/
		//if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		
		if(!grounded && !(Input.GetKey(InputMan.Hold) || Input.GetKey(InputMan.Hold2) || Input.GetKey(InputMan.Hold3) || Input.GetKey(InputMan.PadHold))) {
			if(facingDir == facing.Right) col.center = new Vector3(0.2f, 0f, 0f);
			else col.center = new Vector3(-0.2f, 0f, 0f);
			col.size = new Vector3(0.4f, col.size.y, col.size.z);
		}
		//if(chute && grounded) {col.center = new Vector3(0f, 0f, 0f);}
	}

	private void holdCrate()
	{
		col.size = new Vector3(1.75f, col.size.y, col.size.z);
		//collider.bounds.size.Set(1.75f, 1.75f, 10f);
	}
	

	public void triggerPause()
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

	public void resetLevel()
	{
		isDead = true;
		GameEventManager.TriggerGameOver();
	}

	private void playFootstep()
	{
		if ((isLeft || isRight) && grounded && !finishedLevel && !isCrounch)
		{
			if(walkSoundLeft) MasterAudio.PlaySound ("player_runL1");
			else MasterAudio.PlaySound ("player_runR1");
			walkSoundLeft = !walkSoundLeft;
		}
	}

	public void teleportTo(Vector3 pos) {
		thisTransform.position = pos;
	}
	public IEnumerator rewind () {
		yield return new WaitForSeconds(0.01f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		thisTransform.Rotate(0f,0f,angleRotation++);
		StartCoroutine("rewind");
	}
	public IEnumerator stopRewind (float duree) {
		yield return new WaitForSeconds(duree);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		StopCoroutine("rewind");
		thisTransform.Rotate(0f,0f,angleRotation--);
		if(angleRotation<5) {StopCoroutine("stopRewind");thisTransform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));}
		StartCoroutine("stopRewind",0.015f);
	}
	
	private void GameStart () 
	{
		if(FindObjectOfType(typeof(Player)) && this != null) {
			onCrate = false;
			transform.localPosition = spawnPos;
			enabled = true;
			moveVel = moveVelIni;
		pushCrate = grabCrate = false;
		col.size = new Vector3(1f, col.size.y, col.size.z);
		col.center = new Vector3(0f, 0f, 0f);
		HUDPause.gameObject.SetActive(false);
			finishedLevel=killedByBlob = killedByLaser = false;
		collider.enabled=true;
		isJump = false;
		chute = true;
		vectorMove.y = 0;
		angleRotation = 0;
		nbKey = 0;
		isDead = false;
		}
	}
	private void FinishLevel() {
		if(FindObjectOfType(typeof(Player)) && this != null) {
			//enabled = false;
			collider.enabled=false;
		}
	}
	
	private void GameOver () 
	{
		if(this != null && gameObject.activeInHierarchy) {
			paused=false;
			StartCoroutine("resetGame");
			isLeft = false;
			isRight = false;
			isJump = false;
			isPass = false;
			isDead = true;
			movingDir = moving.None;
			MasterAudio.PlaySound("lose");
			enabled = false;
		}
	}
	private void GamePause()
	{
		if(this != null && gameObject.activeInHierarchy) {
		//enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		paused = true;
		movingDir = moving.None;
		GameEventManager.gamePaused = true;		
		}
	}
	public void GameUnpause()
	{
		if(this != null && gameObject.activeInHierarchy) {
		paused = false;
		//enabled = true;	
		HUDPause.gameObject.SetActive(false);
		GameEventManager.gamePaused = false;	
		}
	}
	
	IEnumerator resetGame()
	{
		Instantiate(Resources.Load("Objects/Invasion"));
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		yield return new WaitForSeconds(4f);
		GameEventManager.TriggerGameStart();
	}
}
