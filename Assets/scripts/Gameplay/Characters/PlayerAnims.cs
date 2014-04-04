using UnityEngine;
using System.Collections;

public class PlayerAnims : MonoBehaviour 
{
	public enum animDef
	{
		None,
		WalkLeft, WalkRight,
		RopeLeft, RopeRight,
		Climb, ClimbStop,
		StandLeft, StandRight,
		HangLeft, HangRight,
		FallLeft, FallRight ,
		ShootLeft, ShootRight,
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
		PushCrateLeft, PushCrateRight,
		GrabCrateLeft, GrabCrateRight,
		DeathBlobLeft, DeathBlobRight,
		VictoryLeft, VictoryRight,
		TeleportINRight, TeleportINLeft,
		TeleportOUTRight, TeleportOUTLeft
	}
	
	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	private bool stopped, victoryAnim;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player 	= GetComponent<Player>();
		anim = GameObject.Find("playerAnims").GetComponent<OTAnimation>();
		GameEventManager.FinishLevel += FinishLevel;
		GameEventManager.GameStart += GameStart;
	}
	void Update() 
	{
		if(!victoryAnim) {
		// Order of action matters, they need to have priorities. //
		if(animSprite.frameIndex == 23) {animPlaying=false;anim.fps = 12.66667f;_player.locked=false;} //stop teleport
		if(animSprite.frameIndex == 38 && !stopped) {stopped=true;animSprite.Pauze();StartCoroutine("waitB4Restart",2.5f);} //deathBlob pause
		if(animSprite.frameIndex == 46 && !stopped) {stopped=true;animSprite.Pauze();StartCoroutine("waitB4Restart",1f);} //iddle pause 1
		if(animSprite.frameIndex == 48 && !stopped) {stopped=true;animSprite.Pauze();StartCoroutine("waitB4Restart",1f);} //iddle pause 2
		//if(animSprite.frameIndex == 43 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",0.09f);} //deathBlob pause
		TeleportIN();
		Run();
		GrabCrate();
		PushCrate();
		Walk();
		Stand();
		Crounch();
		Jump();
		Attack();
		Hurt();
		Fall();
		Paused();
		DeathBlob();
		//print (currentAnim);
		if(_character.grounded) animSprite.looping = true;
		}
	}
	
	private void GameStart() {
		victoryAnim = false;
	}
	private void FinishLevel() {
		if(this != null && gameObject.activeInHierarchy) {
			Victory();
		}
	}
	
	private void Victory()
	{
		if(currentAnim != animDef.VictoryLeft && _character.facingDir == Character.facing.Left)
		{
			victoryAnim = true;
			currentAnim = animDef.VictoryLeft;
			animSprite.Play("stand"); // fall left
			InvertSprite();
			_player.isRight = _player.isLeft = false;
		}
		if(currentAnim != animDef.VictoryRight && _character.facingDir == Character.facing.Right)
		{
			victoryAnim = true;
			currentAnim = animDef.VictoryRight;
			animSprite.Play("stand"); // fall right
			NormalScaleSprite();
			_player.isRight = _player.isLeft = false;
		}
	}
	private IEnumerator waitB4Restart (float delayRestart) {//print ("attend");
		yield return new WaitForSeconds(delayRestart);
		//print ("attendu");
		stopped = false;//print (animSprite.frameIndex+1);
		if(animSprite.frameIndex == 28) { //Reprend anim mort
			if(animSprite.frameIndex != 38) animSprite.frameIndex=animSprite.frameIndex+1;
			animSprite.Resume();
		}
		else if (animSprite.frameIndex == 33) {
			animSprite.frameIndex = 40;
		}
		else //Reprend autres anims
			animSprite.Play(animSprite.frameIndex+1);
	}
	private void TeleportIN()
	{
		if(_player.locked && _character.grounded && currentAnim!=animDef.TeleportINRight && _character.facingDir == Character.facing.Right)
		{
			currentAnim = animDef.TeleportINRight;
			animSprite.PlayOnce("teleportIN");
			NormalScaleSprite();
			animPlaying = true;
			anim.fps = 16f;
			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
		}
		if(_player.locked && _character.grounded && currentAnim!=animDef.TeleportINLeft && _character.facingDir == Character.facing.Left)
		{
			currentAnim = animDef.TeleportINLeft;
			animSprite.PlayOnce("teleportIN");
			animPlaying = true;
			InvertSprite();
			anim.fps = 16f;
			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
		}
	}
//	private void TeleportOUT()
//	{
//		if(_player.locked && _character.grounded && currentAnim!=animDef.TeleportOUTRight && _character.facingDir == Character.facing.Right)
//		{
//			currentAnim = animDef.TeleportOUTRight;
//			animSprite.PlayOnce("teleportOUT");
//			//animPlaying = true;
//			NormalScaleSprite();
//			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
//		}
//		if(_player.locked && _character.grounded && currentAnim!=animDef.TeleportOUTLeft && _character.facingDir == Character.facing.Left)
//		{
//			currentAnim = animDef.TeleportOUTLeft;
//			animSprite.PlayOnce("teleportOUT");
//			//animPlaying = true;
//			InvertSprite();
//			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
//		}
//	}
	
	private void DeathBlob()
	{
		if(_character.facingDir == Character.facing.Right && currentAnim!=animDef.DeathBlobRight && _player.isDead)
		{
			currentAnim = animDef.DeathBlobRight;
			animSprite.Play("deathBlob");
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && currentAnim!=animDef.DeathBlobLeft && _player.isDead)
		{
			currentAnim = animDef.DeathBlobLeft;
			animSprite.Play("deathBlob");
			InvertSprite();
		}
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight && !_player.pushCrate && !_player.grabCrate)
		{
			currentAnim = animDef.WalkRight;
			animSprite.Play("run");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft && !_player.pushCrate && !_player.grabCrate)
		{
			currentAnim = animDef.WalkLeft;
			animSprite.Play("run");
			InvertSprite();
		}
	}
	private void PushCrate()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.PushCrateRight && _player.pushCrate)
		{
			currentAnim = animDef.PushCrateRight;
			animSprite.Play("pushCrate");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.PushCrateLeft && _player.pushCrate)
		{
			currentAnim = animDef.PushCrateLeft;
			animSprite.Play("pushCrate");
			InvertSprite();
		}
	}
	private void GrabCrate()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.GrabCrateRight && _player.grabCrate)
		{
			currentAnim = animDef.GrabCrateRight;
			animSprite.Play("grabCrate");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.GrabCrateLeft && _player.grabCrate)
		{
			currentAnim = animDef.GrabCrateLeft;
			animSprite.Play("grabCrate");
			InvertSprite();
		}
	}
	private void Walk()
	{
		
	}
	private void Stand()
	{	
		if(!_character.isLeft && _character.grounded == true && currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false)
		{
			currentAnim = animDef.StandLeft;
			animSprite.Play("stand"); // stand left
			InvertSprite();
		}
		if(!_character.isRight && _character.grounded && currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false)
		{
			currentAnim = animDef.StandRight;
			animSprite.Play("stand"); // stand left
			NormalScaleSprite();
		}
	}
	private void Crounch()
	{
		if (_character.isCrounch == true)
		{
			currentAnim = animDef.CrounchLeft;
			animSprite.Play("crounch");
		}
	}
	private void Jump()
	{
		if(!_player.locked && !_player.isDead && _character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
		{
			animSprite.looping = false;
			MasterAudio.StopAllOfSound("player_runL1");
			currentAnim = animDef.FallLeft;
			animSprite.Play("jump"); // fall left
			//print (_character.falling);
			InvertSprite();
		}
		if(!_player.locked && !_player.isDead && _character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
		{
			animSprite.looping = false;
			MasterAudio.StopAllOfSound("player_runL1");
			currentAnim = animDef.FallRight;
			animSprite.Play("jump"); // fall right
			//print (_character.falling);
			NormalScaleSprite();
		}
	}
	private void Attack()
	{
		/*
		if (_player.shootingKnife == true  && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animSprite.Play("throw_knife");
			InvertSprite();
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
		if (_player.shootingKnife == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animSprite.Play("throw_knife");
			NormalScaleSprite();
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
		*/
	}
	private void Hurt()
	{
		//ENEMIES SPECIFIC ANIMS
		if (_character.isShot == true && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			InvertSprite();
		}
		if (_character.isShot == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			NormalScaleSprite();
		}
	}
	private void Fall()
	{
		
	}
	private void Paused()
	{
		if (_player.paused == true)
		{
			currentAnim = animDef.None;
			animSprite.looping = false;
		}
	}
	
	private void AnimationFinished()
	{
		animPlaying = false;
	}
	private void InvertSprite()
	{
		spriteParent.localScale = new Vector3(-1,1,1);
	}
	private void NormalScaleSprite()
	{
		spriteParent.localScale = new Vector3(1,1,1);
	}
	IEnumerator WaitAndCallback(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		AnimationFinished();
	}
}
