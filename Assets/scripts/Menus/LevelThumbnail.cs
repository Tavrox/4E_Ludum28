using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelThumbnail : MonoBehaviour
{
	private OTAnimatingSprite animSpr;
	private ThumbnailAnimations animThumb;

	public GameSetup.LevelList nameLv;
	public bool Locked = false;
	public LevelInfo Info;
	private TextUI linkedText;

	public void Setup()
	{
		Info = Resources.Load("Tuning/Levels/" +  nameLv.ToString()) as LevelInfo;
		animSpr = GetComponentInChildren<OTAnimatingSprite>();
		animThumb = GetComponentInChildren<ThumbnailAnimations>();
		animThumb.Setup();
		linkedText = GetComponentInChildren<TextUI>();
	}

	void Update()
	{
		if (Info.locked == true)
		{
			animSpr.animation = animThumb.BtnNoLvl;
			animSpr.PlayLoop(animThumb.ACTIVATED);
		}
		else
		{
			animSpr.animation = animThumb.BtnLvl;
			animSpr.PlayLoop(animThumb.ACTIVATED);
		}
	}
}