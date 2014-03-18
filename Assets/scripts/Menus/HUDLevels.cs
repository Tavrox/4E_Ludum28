using UnityEngine;
using System.Collections;

public class HUDLevels : MonoBehaviour {

	private LevelManager _levelM;
	private OTSprite _HUDLevelsSprite;
	// Use this for initialization
	void Start () {
		_levelM = GameObject.FindObjectOfType<LevelManager>();
		_HUDLevelsSprite = gameObject.GetComponentInChildren<OTSprite>();
		_HUDLevelsSprite.frameIndex = _levelM.chosenVariation-1;
		if(_levelM.isBoss == true) _HUDLevelsSprite.frameIndex = 5;
		GameEventManager.GameStart += GameStart;
	}
	void GameStart () {	
		_HUDLevelsSprite.frameIndex = _levelM.chosenVariation-1;
		if(_levelM.isBoss == true) _HUDLevelsSprite.frameIndex = 5;
	}
}
