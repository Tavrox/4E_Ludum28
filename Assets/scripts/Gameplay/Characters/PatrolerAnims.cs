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
		animSprite.Play("run");
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
