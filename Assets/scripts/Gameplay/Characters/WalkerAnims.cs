using UnityEngine;
using System.Collections;

public class WalkerAnims : MonoBehaviour 
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
		SplashLeft, SplashRight,
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
	}
	
	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	public bool lookLeft;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	private Enemy _enemy;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		_player 	= GameObject.FindObjectOfType<Player>();
		_enemy = GetComponent<Enemy>();
		if(lookLeft) {InvertSprite();currentAnim = animDef.StandLeft;_character.facingDir = Character.facing.Left;}
		animSprite.Play("stand");
		animSprite.alpha = 0.8f;
	}
	void Update() 
	{
		// Order of action matters, they need to have priorities. //
		Run();
		Stand();
		Splash();
	}
	private void Run()
	{
		if(_character.isRight && /*_character.grounded &&*/ currentAnim!=animDef.WalkRight && !_enemy.splashed)
		{
			currentAnim = animDef.WalkRight;
			animSprite.Play("run");
			NormalScaleSprite();
		}
		if(_character.isLeft && /*_character.grounded &&*/ currentAnim!=animDef.WalkLeft && !_enemy.splashed)
		{
			currentAnim = animDef.WalkLeft;
			animSprite.Play("run");
			InvertSprite();
		}
	}
	private void Stand()
	{	
		if(!_character.isLeft && /*_character.grounded == true &&*/ currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false && !_enemy.splashed)
		{
			currentAnim = animDef.StandLeft;
			animSprite.Play("stand"); // stand left
			InvertSprite();
		}
		if(!_character.isRight && /*_character.grounded &&*/ currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false && !_enemy.splashed)
		{
			currentAnim = animDef.StandRight;
			animSprite.Play("stand"); // stand left
			NormalScaleSprite();
		}
	}
	private void Splash()
	{	
		if(/*!_character.isLeft && _character.grounded == true &&*/ currentAnim != animDef.SplashLeft && _character.facingDir == Character.facing.Left && _enemy.splashed)
		{
			currentAnim = animDef.SplashLeft;
			animSprite.PlayOnce("splash"); // stand left
			InvertSprite();
		}
		if(/*!_character.isRight && _character.grounded &&*/ currentAnim != animDef.SplashRight && _character.facingDir == Character.facing.Right && _enemy.splashed)
		{
			currentAnim = animDef.SplashRight;
			animSprite.PlayOnce("splash"); // stand left
			NormalScaleSprite();
		}
	}
	
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy) {
		if(lookLeft) InvertSprite();
		animSprite.Play("stand");
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
