using UnityEngine;
using System.Collections;

public class PatrolerAnims : MonoBehaviour 
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
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	private Patroler _enemy;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player 	= GameObject.FindObjectOfType<Player>();
		_enemy = GetComponent<Patroler>();
		GameEventManager.GameStart += GameStart;
		animSprite.Play("run");
		animSprite.alpha = 0.8f;
	}
	void Update() 
	{
		animSprite.looping = true;
		// Order of action matters, they need to have priorities. //
		Run();
		Splash();
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded && currentAnim!=animDef.WalkRight && !_enemy.splashed)
		{
			currentAnim = animDef.WalkRight;
			animSprite.Play("run");
			NormalScaleSprite();;
		}
		if(_character.isLeft && _character.grounded && currentAnim!=animDef.WalkLeft && !_enemy.splashed)
		{
			currentAnim = animDef.WalkLeft;
			animSprite.Play("run");
			InvertSprite();
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
		animSprite.Play("run");
	}
	
	private void AnimationFinished()
	{
		animPlaying = false;
	}
	public void InvertSprite()
	{
		spriteParent.localScale = new Vector3(-1,1,1);
	}
	public void NormalScaleSprite()
	{
		spriteParent.localScale = new Vector3(1,1,1);
	}
	IEnumerator WaitAndCallback(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		AnimationFinished();
	}
}
