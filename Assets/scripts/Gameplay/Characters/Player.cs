using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;

	//	public Pebble instPebble;
	//	public WaveCreator instFootWave,instInstruWave;
	//	public GameObject instPebbleBar;
	public OTSprite menu;
	//	public float footStepDelay = 0.6f;

	//	private WaveCreator soundEmitt1, soundEmitt2, soundInstru1, soundInstru2,soundEmitt3;
	//	private int cptWave=1, pebbleDirection = 1;
	//	private bool blockCoroutine, first, toSprint, toWalk, specialCast, playerDirLeft;
	//	private Pebble pebble1;
	//	private float powerPebble;
	//	private GameObject pebbleBar;
	
	[HideInInspector] public bool paused = false;
	public int angleRotation;
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		
		spawnPos = thisTransform.position;

		GameObject.Find("Frameworks/OT/View").GetComponent<OTView>().movementTarget = gameObject;
	}
	// Update is called once per frame
	public void Update () 
	{
		checkInput();
		UpdateMovement();
		//		offsetCircles ();
	}
	
	private void GameStart () 
	{
		if(FindObjectOfType(typeof(Player)) && this != null) {
			transform.localPosition = spawnPos;
			enabled = true;
		}
	}
	
	private void GameOver () 
	{
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		movingDir = moving.None;
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

		if(Input.GetKey("left") /*&& !specialCast*/) 
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
		if (Input.GetKey("right") && !isLeft /*&& !specialCast*/) 
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
		if (Input.GetKey("up") /* && grounded*/) 
		{ 
			isJump = true; 
		}
		if (Input.GetKeyUp("up")) chute=true;
		
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
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			//thisTransform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
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
	}

	public void teleportTo(Vector3 pos) {
		thisTransform.position = pos;
	}
	public IEnumerator rewind () {
		yield return new WaitForSeconds(0.02f);
		thisTransform.Rotate(0f,0f,angleRotation++);
		StartCoroutine("rewind");
	}
	public IEnumerator stopRewind (float duree) {
		yield return new WaitForSeconds(duree);
		StopCoroutine("rewind");
		thisTransform.Rotate(0f,0f,angleRotation--);
		if(angleRotation<12) {StopCoroutine("stopRewind");thisTransform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));}
		StartCoroutine("stopRewind",0.04f);
	}
}
