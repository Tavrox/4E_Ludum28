using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	public bool hasFinalKey = false;
	
	[HideInInspector] public bool paused = false;
	public int angleRotation;
	public bool isDead = false, locked = false;
	private bool walkSoundLeft;
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

		InvokeRepeating("playFootstep",0f,0.4f);
		
//		GetComponent<BoxCollider>().size = new Vector3(1.3f,2f,30f);
//		GetComponent<BoxCollider>().center = new Vector3(0f,0f,0f);
		
		spawnPos = thisTransform.position;
		isDead =false;

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
		
		movingDir = moving.None;

		if(Input.GetKey("left") || Input.GetKey(KeyCode.Q) /*&& !specialCast*/) 
		{ 
			isLeft = true;
			shootLeft = true;
			facingDir = facing.Left;
			//if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");
		}
		/*if((Input.GetKeyUp("left") && !specialCast) || (Input.GetKeyUp("right") && !isLeft && !specialCast)) {
			StopCoroutine("footStep");
			blockCoroutine = false;
		}*/
		if ((Input.GetKey("right") || Input.GetKey(KeyCode.D)) && !isLeft /*&& !specialCast*/) 
		{ 
			isRight = true; 
			facingDir = facing.Right;
			shootLeft = false;
			/*if(!blockCoroutine && grounded) StartCoroutine("waitB4FootStep");*/
		}
//		if (Input.GetKey(KeyCode.DownArrow))
//		{
//			isCrounch = true;
//			facingDir = facing.Down;
//		}
		if (Input.GetKey("up")  || Input.GetKey(KeyCode.Z)/* && grounded*/) 
		{ 
			isJump = true; 
		}
		if (Input.GetKeyDown("up")  || Input.GetKeyDown(KeyCode.Z)/* && grounded*/) 
		{ 
			MasterAudio.PlaySound("player_jump");
		}
		if (Input.GetKeyUp("up") || Input.GetKey(KeyCode.Z)) chute=true;
		
		if(Input.GetKeyDown("space"))
		{
			isPass = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			//skill_axe.useSkill(Skills.SkillList.Axe);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameObject.Find("Invasion").GetComponent<InvasionAnims>().invade();
		}
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
		if (Input.GetKeyDown(KeyCode.Backspace)) {
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
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
		}
		
		isJump = false;
		chute = true;
		vectorMove.y = 0;
		angleRotation = 0;
		enabled = true;
		isDead = false;
	}
	
	private void GameOver () 
	{
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
