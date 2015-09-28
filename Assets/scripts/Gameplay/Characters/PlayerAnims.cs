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
		JumpLeft, JumpRight ,
		FallLeft, FallRight,
		ShootLeft, ShootRight,
		CrounchLeft, CrounchRight,
		unCrounchLeft, unCrounchRight,
		AttackLeft, AttackRight,
		PushCrateLeft, PushCrateRight,
		GrabCrateLeft, GrabCrateRight,
		DeathBlobLeft, DeathBlobRight,
		DeathTimeLeft, DeathTimeRight,
		DeathLaserLeft, DeathLaserRight,
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
	private int fEndTP = 23, fDeathB=82, fMiddleIdle=42, fEndIdle=44, crouched =85,endUnCrouch=83;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player 	= GetComponent<Player>();
		anim = GameObject.Find("playerAnims").GetComponent<OTAnimation>();
		GameEventManager.FinishLevel += FinishLevel;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	void Update() 
	{
		if(!victoryAnim) {
		// Order of action matters, they need to have priorities. //
		//if(!stopped) {
			TeleportIN();
		Run();
		GrabCrate();
		PushCrate();
		Walk();
		Stand();
		Jump();
		Attack();
		Hurt();
		//Fall();
		Paused();
			Death();
			Crounch();
			//	}
			//if(animSprite.frameIndex == crouched) {animPlaying=false;_player.isLeft = _player.isRight = false; _player.locked = true;}
			if(animSprite.frameIndex == fEndTP) {animPlaying=false;anim.fps = 12.5f;_player.locked=false;_player.isTeleport = false;_player.collider.enabled = true;} //stop teleport
			if(animSprite.frameIndex == fDeathB && !stopped) {stopped=true;animSprite.Pauze();StartCoroutine("waitB4Restart",2.5f);} //deathBlob pause
			if(animSprite.frameIndex == fMiddleIdle && !stopped) {stopped=true;animSprite.Pauze();StartCoroutine("waitB4Restart",1f);} //iddle pause 1
			if(animSprite.frameIndex == fEndIdle && !stopped) {stopped=true;animSprite.Pauze();StartCoroutine("waitB4Restart",1f);} //iddle pause 2
			//if((currentAnim == animDef.unCrounchLeft || currentAnim == animDef.unCrounchRight) && animSprite.frameIndex == endUnCrouch) {stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",0.075f);/*animSprite.Play("stand");*/} //stand
			//if(animSprite.frameIndex == 51 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",1f);} //jump
			//if(animSprite.frameIndex == 51 && stopped && _player.chute == true) {stopped=false;animSprite.Resume();} //fall
			//if(animSprite.frameIndex == 43 && !stopped) {stopped=true;animSprite.Stop();StartCoroutine("waitB4Restart",0.09f);} //deathBlob pause
			//print (animSprite.frameIndex);
			//print (currentAnim);
            animSprite.looping = false;
            if (_character.grounded && !_character.isCrounch || (_player.isDead && _player.killedByLaser || victoryAnim)) animSprite.looping = true;
		}
	}
	
	private void GameStart() {
		if(this != null && gameObject.activeInHierarchy) {
            spriteParent.transform.localPosition = new Vector3(0, 0, 0);
			anim.fps = 12.5f;
			_player.locked=false;_player.isTeleport = false;_player.collider.enabled = true;animPlaying=false;
			victoryAnim = false;
		}
	}
	private void FinishLevel() {
		/*if(this != null && gameObject.activeInHierarchy) {
			Victory();
		}*/
	}
	
	private IEnumerator waitB4Restart (float delayRestart) {//print ("attend");
		yield return new WaitForSeconds(delayRestart);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		//print ("attendu");
		stopped = false;//print (animSprite.frameIndex+1);
		if(animSprite.frameIndex == fDeathB) { //Reprend anim mort
			//animSprite.frameIndex=animSprite.frameIndex+1;
			animSprite.PlayOnceBackward("deathBlob");
			//if(animSprite.frameIndex == 28) { //Reprend anim mort
			//if(animSprite.frameIndex != 38) animSprite.frameIndex=animSprite.frameIndex+1;
			animSprite.Resume();
		}
		else if(animSprite.frameIndex == endUnCrouch) {
			animSprite.Play("stand");
			//animSprite.Resume();
		}
		//		else if (animSprite.frameIndex == 33) {
		//			animSprite.frameIndex = 40;
		//		}
		else //Reprend autres anims
			animSprite.Play(animSprite.frameIndex+1);
	}
	public void Victory(string typeVictory)
	{
        if (this != null && gameObject.activeInHierarchy)
        {
            if (typeVictory.Contains("gold"))
                spriteParent.transform.position += new Vector3(0, 0.5f, 0);
            else
                spriteParent.transform.position += new Vector3(0, 0.3f, 0);

            animSprite.looping = false;
            StopCoroutine("waitB4Restart");
            victoryAnim = true;
            _player.isRight = _player.isLeft = false;
            anim.fps = 16f;

            if (currentAnim != animDef.VictoryLeft && _character.facingDir == Character.facing.Left)
            {
                currentAnim = animDef.VictoryLeft;
                animSprite.PlayOnce(typeVictory); // fall left
                InvertSprite();
            }
            if (currentAnim != animDef.VictoryRight && _character.facingDir == Character.facing.Right)
            {
                currentAnim = animDef.VictoryRight;
                animSprite.PlayOnce(typeVictory); // fall right
                NormalScaleSprite();
            }
        }
	}
	private void TeleportIN()
	{
		if(_player.locked && _character.grounded && currentAnim!=animDef.TeleportINRight && _character.facingDir == Character.facing.Right)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.TeleportINRight;
			animSprite.PlayOnce("teleportIN");
			NormalScaleSprite();
			animPlaying = true;
			anim.fps = 16f;
			//StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
		}
		if(_player.locked && _character.grounded && currentAnim!=animDef.TeleportINLeft && _character.facingDir == Character.facing.Left)
		{
			StopCoroutine("waitB4Restart");
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
	
	private void Death()
	{
		if(_character.facingDir == Character.facing.Right && currentAnim!=animDef.DeathBlobRight && _player.isDead && _player.killedByBlob)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.DeathBlobRight;
			animSprite.Play("deathBlob");
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && currentAnim!=animDef.DeathBlobLeft && _player.isDead && _player.killedByBlob)
		{
			StopCoroutine("waitB4Restart");
            currentAnim = animDef.DeathBlobLeft;
			animSprite.Play("deathBlob");
			InvertSprite();
		}
		if(_character.facingDir == Character.facing.Right && currentAnim!=animDef.DeathLaserRight && _player.isDead && _player.killedByLaser)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.DeathLaserRight;
			animSprite.PlayLoop("deathElectric");
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && currentAnim!=animDef.DeathLaserLeft && _player.isDead && _player.killedByLaser)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.DeathLaserLeft;
			animSprite.PlayLoop("deathElectric");
			InvertSprite();
		}
		if(_character.facingDir == Character.facing.Right && currentAnim!=animDef.DeathTimeRight && _player.isDead && !_player.killedByLaser && !_player.killedByBlob)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.DeathTimeRight;
			animSprite.Play("deathBlob");
			NormalScaleSprite();
		}
		if(_character.facingDir == Character.facing.Left && currentAnim!=animDef.DeathTimeLeft && _player.isDead && !_player.killedByLaser && !_player.killedByBlob)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.DeathTimeLeft;
			animSprite.Play("deathBlob");
			InvertSprite();
		}
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight && !_player.pushCrate && !_player.grabCrate)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.WalkRight;
			animSprite.Play("run");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft && !_player.pushCrate && !_player.grabCrate)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.WalkLeft;
			animSprite.Play("run");
			InvertSprite();
		}
	}
	private void PushCrate()
	{
//		if(_character.grounded && currentAnim!=animDef.PushCrateRight && _player.pushCrate)
//		{
//			animSprite.frameIndex = 57;
//			NormalScaleSprite();
//		}
//		if(_character.grounded && currentAnim!=animDef.PushCrateLeft && _player.pushCrate)
//		{
//			animSprite.frameIndex = 57;
//			InvertSprite();
//		}
		if(_character.isRight && _character.grounded && currentAnim!=animDef.PushCrateRight && _player.pushCrate)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.PushCrateRight;
			animSprite.Play("pushCrate");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.PushCrateLeft && _player.pushCrate)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.PushCrateLeft;
			animSprite.Play("pushCrate");
			InvertSprite();
		}
	}
	private void GrabCrate()
	{
//		if(_character.grounded && currentAnim!=animDef.GrabCrateRight && _player.grabCrate)
//		{
//			animSprite.frameIndex = 27;
//			NormalScaleSprite();
//		}
//		if(_character.isLeft && _character.grounded && currentAnim!=animDef.GrabCrateLeft && _player.grabCrate)
//		{
//			animSprite.frameIndex = 27;
//			InvertSprite();
//		}
		if(_character.isRight && _character.grounded && currentAnim!=animDef.GrabCrateRight && _player.grabCrate)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.GrabCrateRight;
			animSprite.Play("grabCrate");
			NormalScaleSprite();
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.GrabCrateLeft && _player.grabCrate)
		{
			StopCoroutine("waitB4Restart");
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
		if(!_player.isDead && !_character.isCrounch && !_character.isLeft && _character.grounded == true && currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.StandLeft;
			animSprite.Play("stand"); // stand left
			InvertSprite();
		}
        if (!_player.isDead && !_character.isCrounch && !_character.isRight && _character.grounded && currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false)
		{
			StopCoroutine("waitB4Restart");
			currentAnim = animDef.StandRight;
			animSprite.Play("stand"); // stand left
			NormalScaleSprite();
		}
	}
	private void Crounch()
	{
		if (_character.isCrounch && _character.grounded && currentAnim != animDef.CrounchLeft && currentAnim != animDef.unCrounchLeft && _character.facingDir == Character.facing.Left)
		{
			StopCoroutine("waitB4Restart");
			animSprite.looping = false;
			currentAnim = animDef.CrounchLeft;
			animSprite.Play("stand");
			InvertSprite();
		}
		if (_character.isCrounch && _character.grounded && currentAnim != animDef.CrounchRight && currentAnim != animDef.unCrounchRight && _character.facingDir == Character.facing.Right)
		{
			StopCoroutine("waitB4Restart");
			animSprite.looping = false;
			currentAnim = animDef.CrounchRight;
			animSprite.Play("stand");
			NormalScaleSprite();
		}
		if (_player.unCrouch && _character.grounded && currentAnim != animDef.unCrounchLeft && _character.facingDir == Character.facing.Left)
		{
			StopCoroutine("waitB4Restart");
			animSprite.looping = false;
			currentAnim = animDef.unCrounchLeft;
			animSprite.Play("stand");
			InvertSprite();
		}
		if (_player.unCrouch && _character.grounded && currentAnim != animDef.unCrounchRight && _character.facingDir == Character.facing.Right)
		{
			StopCoroutine("waitB4Restart");
			animSprite.looping = false;
			currentAnim = animDef.unCrounchRight;
			animSprite.Play("stand");
			//animSprite.PlayOnceBackward("sit");
			NormalScaleSprite();
		}
	}
	private void Jump()
	{
		if(!_player.locked && !_player.isDead && _character.grounded == false && currentAnim != animDef.JumpLeft && _character.facingDir == Character.facing.Left)
		{
			StopCoroutine("waitB4Restart");
			animSprite.looping = false;
			//MasterAudio.StopAllOfSound("player_runL1");
			currentAnim = animDef.JumpLeft;
			animSprite.PlayOnce("jump"); // fall left
			//print (_character.falling);
			InvertSprite();
		}
		if(!_player.locked && !_player.isDead && _character.grounded == false && currentAnim != animDef.JumpRight && _character.facingDir == Character.facing.Right)
		{
			StopCoroutine("waitB4Restart");
			animSprite.looping = false;
			//MasterAudio.StopAllOfSound("player_runL1");
			currentAnim = animDef.JumpRight;
			animSprite.PlayOnce("jump"); // fall right
			//print (_character.falling);
			NormalScaleSprite();
		}
	}
	private void Fall()
	{
//		if(_player.fallApproachGround == true && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
//		//if(!_player.locked && !_player.isDead && _player.chute == true && _character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
//		{
//			animSprite.looping = false;
//			currentAnim = animDef.FallLeft;
//			animSprite.PlayOnce("fall"); // fall left
//			//print (_character.falling);
//			InvertSprite();
//		}
//		if(_player.fallApproachGround == true && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
//		//if(!_player.locked && !_player.isDead && _player.chute == true && _character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
//		{
//			animSprite.looping = false;
//			currentAnim = animDef.FallRight;
//			animSprite.PlayOnce("fall"); // fall right
//			//print (_character.falling);
//			NormalScaleSprite();
//		}
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
			StopCoroutine("waitB4Restart");
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			InvertSprite();
		}
		if (_character.isShot == true && _character.facingDir == Character.facing.Right)
		{
			StopCoroutine("waitB4Restart");
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			NormalScaleSprite();
		}
	}
	private void Paused()
	{
		if (_player.paused == true)
		{
			StopCoroutine("waitB4Restart");
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
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		AnimationFinished();
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
