﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportAnims : MonoBehaviour 
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
	}
	
	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	public TeleportAnims teleportDestination;
	public int delayB4Telep = 1;
	public bool isOUT;
	
	private bool animPlaying = false, playerCollision, triggable;
	public InputManager InputMan;
	private Vector3 ditanceToTarget;
	private Parallax [] backgroundObjects;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();

		if(isOUT) animSprite.Play("teleportOUT");
		else animSprite.Play("default");
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		
		InputMan = Instantiate(Resources.Load("Tuning/InputManager")) as InputManager;
		InputMan.Setup();
		
		if(!isOUT) ditanceToTarget = new Vector3(teleportDestination.transform.position.x-gameObject.transform.position.x, teleportDestination.transform.position.y-gameObject.transform.position.y,0f); //.Distance(gameObject.transform.position, teleportDestination.transform.position);
		backgroundObjects = FindObjectsOfType<Parallax>();
	}
	void Update() 
	{
		animSprite.looping = true;
		// Order of action matters, they need to have priorities. //
//		Run();
//		Walk();
//		Stand();
//		Crounch();
//		Jump();
//		Attack();
//		Hurt();
//		Fall();
		//		Paused();
		if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2) || Input.GetKeyDown(InputMan.Action3)) && !isOUT && !_player.locked && triggable) {
				inputDetected();
		}
	}
	private void /*OnTriggerStay*/inputDetected(/*Collider other*/) 
	{
		//if(other.gameObject.CompareTag("Player") && !isOUT) 
		//{
//			if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2) || Input.GetKey(InputMan.Action3)) && !_player.locked) {
				_player.isTeleport = true;
				triggable = false;
				FESound.playDistancedSound("teleport_in",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("teleport_in");
				animSprite.Play("teleport");
				//_player.enabled = false;
				StartCoroutine("stopPlayer");
				StartCoroutine("teleportTo",teleportDestination.transform);
				_player.collider.enabled = false;
//			}
		//}
	}
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player"))
		{
			triggable = true;
		}
	}
	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player"))
		{
			triggable = false;
		}
	}
	private IEnumerator stopPlayer () {
		yield return new WaitForSeconds(0.05f);//0.15 ? Edit Bastien
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		//_player.enabled = false;
		_player.locked = true;
		_player.isLeft = _player.isRight = false;
	}
	private IEnumerator teleportTo(Transform destination) {
		yield return new WaitForSeconds(delayB4Telep);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		//_player.position = new Vector3(0f,0f,0f);//destination.position;
		_player.teleportTo( new Vector3(destination.position.x, destination.position.y, _player.transform.position.z));
		print(gameObject.transform.position);
		print(teleportDestination.transform.position);
		print(ditanceToTarget);
		foreach(Parallax _bg in backgroundObjects) {
			_bg.gameObject.transform.position = new Vector3(
				(_bg.gameObject.transform.position.x+ditanceToTarget.x/(/*_player.getVectorFixed().x/*/_bg.scrollSpeed*2.5f)),
				(_bg.gameObject.transform.position.y+ditanceToTarget.y/(/*_player.getVectorFixed().y/*/_bg.scrollSpeed*2.5f))
				,_bg.gameObject.transform.position.z) ;
			//_bg.gameObject.transform.position.y += ditanceToTarget.y/(_player.getVectorFixed().y/_bg.scrollSpeed);
		}
		//_player.enabled = true;
		//_player.locked = false;
		//animSprite.Play("default");
		FESound.playDistancedSound("teleport_out",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("teleport_out");
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight)
		{
			currentAnim = animDef.WalkRight;
			animSprite.Play("run");
			NormalScaleSprite();;
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft)
		{
			currentAnim = animDef.WalkLeft;
			animSprite.Play("run");
			InvertSprite();
		}
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
		if(_character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
		{
			currentAnim = animDef.FallLeft;
			animSprite.Play("jump"); // fall left
			InvertSprite();
		}
		if(_character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
		{
			currentAnim = animDef.FallRight;
			animSprite.Play("jump"); // fall right
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
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		AnimationFinished();
	}
	void GameOver () {		
		if(this != null && gameObject.activeInHierarchy) {
		StopCoroutine("stopPlayer");
		StopCoroutine("teleportTo");
		_player.locked = false;
		if(isOUT) animSprite.Play("teleportOUT");
		else animSprite.Play("default");
		}
	}
	void GameStart () {		
		if(this != null && gameObject.activeInHierarchy) {
		StopCoroutine("stopPlayer");
		StopCoroutine("teleportTo");
		_player.locked = false;
		_player.collider.enabled = true;
		_player.isTeleport = false;
		if(isOUT) animSprite.Play("teleportOUT");
		else animSprite.Play("default");
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
