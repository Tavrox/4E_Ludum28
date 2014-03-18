using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	[HideInInspector] public bool paused = false;

	public bool hasFinalKey = false;

	public int angleRotation;
	public bool isDead = false, locked = false;
	private bool walkSoundLeft;
	
	private BoxCollider col;
	// Use this for initialization
	public override void Start () 
	{
		base.Start();

		StartCoroutine ("Wait");

		OTAnimatingSprite _sprite = GetComponentInChildren<Transform>().GetComponentInChildren<OTAnimatingSprite>();

		_sprite.alpha = 0f;
		OTTween _tween = new OTTween(_sprite,1f).Tween("alpha",1f);

		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;

		InvokeRepeating("playFootstep",0f,0.4f);
		
//		GetComponent<BoxCollider>().size = new Vector3(1.3f,2f,30f);
//		GetComponent<BoxCollider>().center = new Vector3(0f,0f,0f);
		
		spawnPos = thisTransform.position;
		isDead =false;
		col = (BoxCollider)this.collider;
		GameObject.Find("Frameworks/OT/View").GetComponent<OTView>().movementTarget = gameObject;

	}
	// Update is called once per frame
	public void Update () 
	{
		if(!locked) checkInput();
		UpdateMovement();
		//		offsetCircles ();
	}
	IEnumerator Wait()
	{
		yield return new WaitForSeconds (6f);

	}
	
	private void checkInput()
	{
		// these are false unless one of keys is pressed
		isLeft = false;
		isRight = false;
		isJump = false;
		isGoDown = false;
		isPass = false;
		isCrounch = false;
		
		col.size = new Vector3(1f, col.size.y, col.size.z);
		col.center = new Vector3(0f, 0f, 0f);
		movingDir = moving.None;
		if(Input.GetKey("left shift") || Input.GetKey("right shift") || Input.GetKey(KeyCode.A)) {
			col.size = new Vector3(1.75f, col.size.y, col.size.z);
			//collider.bounds.size.Set(1.75f, 1.75f, 10f);
			holdCrate();
		}
		if(Input.GetKey(InputMan.Left) || Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("X axis") > InputMan.X_AxisPos_Sensibility ) 
		{ 
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
		}
		
		if ((Input.GetKey(InputMan.Right) || Input.GetKey(KeyCode.D) || Input.GetAxisRaw("X axis") < InputMan.X_AxisNeg_Sensibility)
			    && !isLeft) 
		{ 
			isRight = true; 
			facingDir = facing.Right;
			shootLeft = false;
			//if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		if (Input.GetKey(InputMan.Up)  || Input.GetKey(KeyCode.Z) || Input.GetKey(InputMan.PadJump)) 
		{ 
			isJump = true;
		}
		if (Input.GetKeyDown(InputMan.Up)  || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(InputMan.PadJump) /* && grounded*/) 
		{ 
			MasterAudio.PlaySound("player_jump");
		}
		if (Input.GetKeyUp(InputMan.Up) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(InputMan.PadJump) ) chute=true;
		
		if(Input.GetKeyDown(InputMan.Action))
		{
			isPass = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameObject.Find("Invasion").GetComponent<InvasionAnims>().invade();
		}
		if (Input.GetKeyDown(InputMan.Pause))
		{
			triggerPause();
		}
		if (Input.GetKeyDown(InputMan.Reset)) 
		{
			resetLevel();
		}
			
		//		if (Input.GetKey(KeyCode.DownArrow))
		//		{
		//			isCrounch = true;
		//			facingDir = facing.Down;
		//		}

		/*
		 * if((Input.GetKeyUp("left") && !specialCast) || (Input.GetKeyUp("right") && !isLeft && !specialCast)) {
		 * StopCoroutine("footStep");
		 * blockCoroutine = false;
		}*/
		/*if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");*/
		//if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		
		if(!grounded) {if(facingDir == facing.Right) col.center = new Vector3(0.35f, 0f, 0f);
			else col.center = new Vector3(-0.35f, 0f, 0f);
			col.size = new Vector3(0.8f, col.size.y, col.size.z);
		}
		//if(chute && grounded) {col.center = new Vector3(0f, 0f, 0f);}
	}

	private void holdCrate()
	{
		col.size = new Vector3(1.75f, col.size.y, col.size.z);
		//collider.bounds.size.Set(1.75f, 1.75f, 10f);
	}

	private void triggerPause()
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

	private void resetLevel()
	{
		GameObject.Find("Player").GetComponent<Player>().isDead = true;
		GameEventManager.TriggerGameOver();
	}

	private void playFootstep()
	{
		if ((isLeft || isRight) && grounded )
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
		thisTransform.Rotate(0f,0f,angleRotation++);
		StartCoroutine("rewind");
	}
	public IEnumerator stopRewind (float duree) {
		yield return new WaitForSeconds(duree);
		StopCoroutine("rewind");
		thisTransform.Rotate(0f,0f,angleRotation--);
		if(angleRotation<5) {StopCoroutine("stopRewind");thisTransform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));}
		StartCoroutine("stopRewind",0.015f);
	}
	
	private void GameStart () 
	{
		if(FindObjectOfType(typeof(Player)) && this != null) {
			transform.localPosition = spawnPos;
			enabled = true;
		
		
		enabled = true;
		collider.enabled=true;
		isJump = false;
		chute = true;
		vectorMove.y = 0;
		angleRotation = 0;
		enabled = true;
		isDead = false;
		}
	}
	private void FinishLevel() {
		if(FindObjectOfType(typeof(Player)) && this != null) {
			enabled = false;
			collider.enabled=false;
		}
	}
	
	private void GameOver () 
	{
		if(this != null) {
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
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		paused = true;
		movingDir = moving.None;
		
	}
	private void GameUnpause()
	{
		paused = false;
		enabled = true;	
	}
	
	IEnumerator resetGame()
	{
		Instantiate(Resources.Load("Objects/Invasion"));
		yield return new WaitForSeconds(4f);
		GameEventManager.TriggerGameStart();
	}
}
