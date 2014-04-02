using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelChooser : MonoBehaviour {

	private TextUI levelName;
	private GameSetup SETUP;
	private PlayerData PLAYERDATA;

	private GameObject ThumbGO;
	private List<LevelThumbnail> Thumbs =  new List<LevelThumbnail>();
	private Vector3 gapThumbs = new Vector3(20f, 0f, -10f);
	
	public void Setup () 
	{
		SETUP = MainTitleUI.getSetup();
		PLAYERDATA = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
		LevelThumbnail[] ArrThums = GetComponentsInChildren<LevelThumbnail>();
		foreach (LevelThumbnail _th in ArrThums)
		{
			Thumbs.Add(_th);
			_th.Setup();
		}
		ThumbGO = new GameObject("Thumbnails");
	}
}