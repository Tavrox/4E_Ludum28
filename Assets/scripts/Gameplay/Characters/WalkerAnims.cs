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
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
	}
	
	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player 	= GameObject.FindObjectOfType<Player>();
		animSprite.Play("stand");
	}
	void Update() 
	{
		animSprite.looping = true;
		// Order of action matters, they need to have priorities. //
		Run();
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
