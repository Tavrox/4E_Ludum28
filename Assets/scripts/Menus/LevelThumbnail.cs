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
	public GameSaveLoad _playerDataLoader;

	public void Setup()
	{
		Info = Resources.Load("Tuning/Levels/" +  nameLv.ToString()) as LevelInfo;
		animSpr = GetComponentInChildren<OTAnimatingSprite>();
		animThumb = GetComponentInChildren<ThumbnailAnimations>();
		animThumb.Setup();
		_playerDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
        _playerDataLoader.LoadXMLToList("plr");
		Info.locked = (_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+Info.levelNumName,"locked")=="true")?true:false;
		
		linkedText = GetComponentInChildren<TextUI>();
		if (Info.locked == true)
		{
			animSpr.animation = animThumb.BtnNoLvl;
			//animSpr.PlayLoop("static");
		}
		else
		{
			animSpr.animation = animThumb.BtnLvl;
			//animSpr.PlayLoop("static");
		}
	}
}