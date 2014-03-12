using UnityEngine;
using System;
using System.Collections;

public enum MyTeam { Team1, Team2, None }

public class Character : MonoBehaviour 
{
	public OTAnimation anim;

	/** ADD **/
	public int res_phys, res_mag;
	
	public int maxHP;
	/*[HideInInspector]*/ public int HP;
	[HideInInspector] public bool isShot;
	[HideInInspector] public bool talking;
	[HideInInspector] public bool isGoDown;
	[HideInInspector] public bool isCrounch;
	[HideInInspector] public bool isLookup;
	[HideInInspector] public bool shootLeft = false;
	protected float runSpeed = 1f;
	/** END **/

	[HideInInspector] public enum facing { Right, Left, Down, Up }
	[HideInInspector] public facing facingDir;
	
	[HideInInspector] public enum moving { Right, Left, None }
	[HideInInspector] public moving movingDir;
	
	[HideInInspector] public bool isLeft; 
	[HideInInspector] public bool isRight;
	[HideInInspector] public bool isJump;
	[HideInInspector] public bool isPass;
	
	[HideInInspector] public bool jumping = false;
	[HideInInspector] public bool falling = false,chute, onCrate;
	[HideInInspector] public bool grounded = false;
	[HideInInspector] public bool passingPlatform;
	[HideInInspector] public bool onPlatform;
	[HideInInspector] public Crate myCrate;
	[HideInInspector] public float myCrateX;
	
	[HideInInspector] public bool blockedRight;
	[HideInInspector] public bool blockedLeft;
	[HideInInspector] public bool blockedUp;
	[HideInInspector] public bool blockedDown;
	
	[HideInInspector] public bool alive = true;
	[HideInInspector] public Vector3 spawnPos;
	
	[HideInInspector] public bool hasShield = false; 
	[HideInInspector] public int shieldDef; 
	
	[HideInInspector] public Transform thisTransform;
	
	[HideInInspector] public Vector3 vectorFixed;
	protected Vector3 vectorMove;
	private Vector3 mypos;
	public InputManager InputMan;

	public float jumpSpeed = 12f, jumpDuration = 0.028f, jumpState; //0.036
	[Range (0,2000)] 	public float 	moveVel = 10f;
	[Range (0,2000)] 	public float 	jumpVel = 80f;
	[Range (0,2000)] 	public float 	jump2Vel = 14f;
	[Range (1,2)] 	public int 		maxJumps = 1;
	[Range (0,2000)] public float 	fallVel = 50f;
	[SerializeField] public float hitUpBounceForce = 5f;
	
	[SerializeField] private int jumps = 0;
	[Range (0,2000)] public float gravityY/* = 16f*/;
	[Range (0,2000)] public float maxVelY = 20f;

	[SerializeField] private RaycastHit hitInfo;
	[SerializeField] private float halfMyX;
	[SerializeField] private float halfMyY;
	
//	[SerializeField] private float absVel2X;
//	[SerializeField] private float absVel2Y;
	
	// layer masks
	protected int groundMask = 1 << 8; // Ground, Block
	protected int platformMask = 1 << 8; //Block
	private float pfPassSpeed = 2.8f;
	private float addForce;
	
	public virtual void Awake()
	{
		thisTransform = transform;
	}
	
	// Use this for initialization
	public virtual void Start () 
	{
		HP = maxHP;
		//maxVelY = fallVel;
		vectorMove.y = 0;
		halfMyX = GetComponentInChildren<Transform>().GetComponentInChildren<OTAnimatingSprite>().size.x * 0.5f;
		halfMyY = GetComponentInChildren<Transform>().GetComponentInChildren<OTAnimatingSprite>().size.y * 0.5f /*+ 0.2f*/;
		StartCoroutine(StartGravity());
		spawnPos = mypos = new Vector3(thisTransform.position.x,thisTransform.position.y,thisTransform.position.z);

		InputMan = Instantiate(Resources.Load("Tuning/InputManager")) as InputManager;
		InputMan.Setup();
	}
	
	IEnumerator StartGravity()
	{
		// wait for things to settle before applying gravity
		yield return new WaitForSeconds(0.1f);
		//gravityY = 25f;
		chute = true;
	}

	// Update is called once per frame
	public virtual void UpdateMovement() 
	{
		if(thisTransform.position.y < mypos.x) falling = true;
		else falling = false;

		mypos = new Vector3(thisTransform.position.x,thisTransform.position.y,thisTransform.position.z);

		if(alive == false) return;
		
		vectorMove.x = 0;
		// pressed right button
		if(isRight == true)
		{
			vectorMove.x = moveVel;
		}
		
		// pressed left button
		if(isLeft == true)
		{			
			vectorMove.x = -moveVel;
		}
		if(!grounded) vectorMove.x=vectorMove.x/1.2f;
		//if (chute) vectorMove.x=vectorMove.x/2.5f;
		// pressed jump button
		if (isJump == true && !chute)
		{
			if (jumps < maxJumps)
		    {
//				jumps += 1;
//				jumping = true;
//				if(jumps == 1)
//				{
//					vectorMove.y = jumpVel;
//				}
//				if(jumps == 2)
//				{
//					vectorMove.y = jump2Vel;
//				}
			}
			jumpSpeed=12f;
			if(jumpState>0.028f) jumpSpeed=jumpSpeed+1f;
			vectorMove.y = jumpSpeed;
			jumpState += 0.001f;
			//print (jumpState);
			if(jumpState > jumpDuration) chute=true;
//			if(vectorMove.y < (3*maxVelY)/4f)	vectorMove.y += jumpSpeed*0.75f;
//			else if(vectorMove.y < maxVelY)	vectorMove.y += jumpSpeed*1.75f;
//			else chute=true;
		}
		addForce = 1;
		if((!grounded && (!Input.GetKey(InputMan.Up)) || Input.GetKey(InputMan.PadJump) ) || blockedUp) chute = true;
		if(blockedUp) {addForce=hitUpBounceForce;/*gravityY += 150f;StartCoroutine("resetGravity");*/}
		if(chute && grounded) {chute = false;MasterAudio.PlaySound("player_fall");}

		// landed from fall/jump
		if(grounded == true && vectorMove.y == 0)
		{
			jumping = false;
			jumps = 0;
			jumpState = 0;
		}
		
		if(onCrate) {
			if(myCrate.transform.position.x < myCrateX) {
				thisTransform.position -= new Vector3(Mathf.Abs(myCrate.transform.position.x - myCrateX),0,0);
			}
			if(myCrate.transform.position.x > myCrateX) {
				thisTransform.position += new Vector3(Mathf.Abs(myCrate.transform.position.x - myCrateX),0,0);
			}
		}
		UpdateRaycasts();
		
		// apply gravity while airborne
		if(grounded == false && chute)
		{
			if(vectorMove.y>0f && vectorMove.y<50f) vectorMove.y -= gravityY * Time.deltaTime * 1.5f * addForce;
			vectorMove.y -= gravityY * Time.deltaTime * 1.5f* addForce;
		}
		// velocity limiter
		if(vectorMove.y < -maxVelY)
		{
			vectorMove.y = -maxVelY;
		}
		
		// apply movement
		vectorMove.x = vectorMove.x * runSpeed; //ADD
		vectorFixed = vectorMove * Time.deltaTime;
		thisTransform.position += new Vector3(vectorFixed.x,vectorFixed.y,0f);
	}
//	private IEnumerator resetGravity() {
//		yield return new WaitForSeconds (1f);
//		gravityY -= 150f;
//	}
	// ============================== RAYCASTS ============================== 
	
	void UpdateRaycasts()
	{
		blockedRight = false;
		blockedLeft = false;
		blockedUp = false;
		blockedDown = false;
		onCrate = false;
		grounded = false;
		myCrate = null;
//		absVel2X = Mathf.Abs(vectorFixed.x);
//		absVel2Y = Mathf.Abs(vectorFixed.y);

		
//		Vector3 tst = new Vector3(mypos.x, mypos.y,0f);
//		Debug.DrawLine( tst , tst+Vector3.down, Color.green);
//		Debug.DrawLine( mypos , Vector3.down, Color.blue);
//		print (mypos);
//
//		print (halfMyY);
		
		//BLOCKED TO DOWN
//		if (Physics.Raycast(mypos, Vector3.down, out hitInfo, halfMyY, platformMask))
//		{
////			print ("entered blocker");
//			Debug.DrawLine(thisTransform.position, hitInfo.point, Color.black);
//			if (isCrounch == true)
//			{
//				passingPlatform = true;
//				ThroughPlatform();
//			}
//			else 
//			{
//				BlockedDown();	
//			}
		//		}
		Debug.DrawRay (new Vector3(mypos.x+0.25f, mypos.y), Vector3.down*halfMyY, Color.yellow);
		Debug.DrawRay (new Vector3(mypos.x-0.25f, mypos.y), Vector3.down*halfMyY, Color.yellow);
		if (Physics.Raycast(mypos, Vector3.down, out hitInfo, halfMyY, groundMask) 
		    || Physics.Raycast(new Vector3(mypos.x+0.25f, mypos.y), Vector3.down, out hitInfo, halfMyY, groundMask)
		    || Physics.Raycast(new Vector3(mypos.x-0.25f, mypos.y), Vector3.down, out hitInfo, halfMyY, groundMask)
		    )
		{
//			print ("blocked down");
			if(hitInfo.collider.CompareTag("Crate")) {
				onCrate = true;
				myCrate=hitInfo.collider.gameObject.GetComponent<Crate>();
				myCrateX = myCrate.transform.position.x;
			}
			BlockedDown();
		}

		// BLOCKED TO UP
		if (Physics.Raycast(mypos, Vector3.up, out hitInfo, halfMyY, groundMask))
		{
			BlockedUp();
			Debug.DrawLine (thisTransform.position, hitInfo.point, Color.red);
		}
		Debug.DrawRay (new Vector3(mypos.x, mypos.y+halfMyY-0.4f), Vector3.right*(halfMyX-0.5f), Color.red);
		//Debug.DrawRay (new Vector3(mypos.x, mypos.y), Vector3.right*(halfMyX-0.5f), Color.red);
		Debug.DrawRay (new Vector3(mypos.x, mypos.y-halfMyY+0.4f), Vector3.right*(halfMyX-0.5f), Color.red);
		// Blocked on right
		if( Physics.Raycast(mypos, Vector3.right, out hitInfo, halfMyX, groundMask) 
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y+halfMyY-0.4f), Vector3.right, out hitInfo, halfMyX, groundMask)
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y-halfMyY+0.4f), Vector3.right, out hitInfo, halfMyX, groundMask))
		{
			if(!hitInfo.collider.CompareTag("Crate")) BlockedRight();
			Debug.DrawRay(mypos, Vector3.right, Color.cyan);
		}
		if( Physics.Raycast(mypos, Vector3.right, out hitInfo, halfMyX-0.6f, groundMask) 
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y+halfMyY-0.4f), Vector3.right, out hitInfo, halfMyX-0.5f, groundMask)
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y-halfMyY+0.4f), Vector3.right, out hitInfo, halfMyX-0.5f, groundMask))
		{
			BlockedRightCrate();
		}
		
		// Blocked on left
		if(	Physics.Raycast(mypos, Vector3.left, out hitInfo, halfMyX, groundMask)
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y+halfMyY-0.4f), Vector3.left, out hitInfo, halfMyX, groundMask)
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y-halfMyY+0.4f), Vector3.left, out hitInfo, halfMyX, groundMask))
		{
			if(!hitInfo.collider.CompareTag("Crate")) BlockedLeft();
			Debug.DrawRay(mypos, Vector3.left, Color.yellow);
		}
//		Debug.DrawRay(new Vector3(1f,0.8f,0), Vector3.right*(halfMyX-0.6f), Color.cyan);
//		Debug.DrawRay(new Vector3(1f,-0.8f,0), Vector3.right*(halfMyX-0.6f), Color.cyan);
		//		Debug.DrawRay(new Vector3(-1f,0.8f,0), Vector3.left*(halfMyX-0.6f), Color.yellow);
		//		Debug.DrawRay(new Vector3(-1f,0.8f,0), Vector3.left*(halfMyX-0.6f), Color.yellow);
		
		Debug.DrawRay (new Vector3(mypos.x, mypos.y+halfMyY-0.4f), Vector3.left*(halfMyX-0.5f), Color.cyan);
		//Debug.DrawRay (new Vector3(mypos.x, mypos.y), Vector3.right*(halfMyX-0.5f), Color.red);
		Debug.DrawRay (new Vector3(mypos.x, mypos.y-halfMyY+0.4f), Vector3.left*(halfMyX-0.5f), Color.cyan);
//		Debug.DrawRay(mypos, Vector3.right*(halfMyX-0.6f), Color.cyan);
//		Debug.DrawRay(mypos, Vector3.left*(halfMyX-0.6f), Color.yellow);
		if(	Physics.Raycast(mypos, Vector3.left, out hitInfo, halfMyX-0.6f, groundMask)
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y+halfMyY-0.4f), Vector3.left, out hitInfo, halfMyX-0.5f, groundMask)
		   || Physics.Raycast(new Vector3(mypos.x, mypos.y-halfMyY+0.4f), Vector3.left, out hitInfo, halfMyX-0.5f, groundMask))
		{
			BlockedLeftCrate();
		}
	}
	
	void BlockedUp()
	{
		if(vectorMove.y > 0)
		{
			vectorMove.y = 0f;
			blockedUp = true;
		}
	}
	void BlockedDown()
	{
		if (vectorMove.y <= 0)
		{
			grounded = true;
			isJump = false;
			vectorMove.y = 0f;
			thisTransform.position = new Vector3(thisTransform.position.x, hitInfo.point.y + halfMyY - 0.1f, thisTransform.position.z);
		}
	}
	void BlockedRight()
	{
		if(facingDir == facing.Right || movingDir == moving.Right)
		{
			blockedRight = true;
			vectorMove.x = 0f;
			thisTransform.position = new Vector3(hitInfo.point.x-(halfMyX-0.01f),thisTransform.position.y, thisTransform.position.z); // .01 less than collision width.
		}
	}
	
	void BlockedLeft()
	{
		if(facingDir == facing.Left || movingDir == moving.Left)
		{
			blockedLeft = true;
			vectorMove.x = 0f;
			thisTransform.position = new Vector3(hitInfo.point.x+(halfMyX-0.01f),thisTransform.position.y, thisTransform.position.z); // .01 less than collision width.
		}
	}
	void BlockedRightCrate()
	{
		if(facingDir == facing.Right || movingDir == moving.Right)
		{
			//blockedRight = true;
			vectorMove.x = 0f;
			//thisTransform.position = new Vector3(hitInfo.point.x-(halfMyX-0.6f),thisTransform.position.y, thisTransform.position.z); // .01 less than collision width.
		}
	}
	
	void BlockedLeftCrate()
	{
		if(facingDir == facing.Left || movingDir == moving.Left)
		{
			//blockedLeft = true;
			vectorMove.x = 0f;
			//thisTransform.position = new Vector3(hitInfo.point.x+(halfMyX-0.6f),thisTransform.position.y, thisTransform.position.z); // .01 less than collision width.
		}
	}
	
	public Vector3 getVectorFixed()
	{
		return vectorFixed;	
	}
	
	void ThroughPlatform()
	{
		vectorMove.y -= pfPassSpeed;
	}
	public void getDamage(int damage) {
		HP -= damage;
		if(HP <=0) gameObject.transform.parent.gameObject.SetActive(false);
	}
}