using UnityEngine;
using System.Collections;

public class HUDLevels : MonoBehaviour {

	private LevelManager _levelM;
	private OTSprite _HUDLevelsSprite;
	private Camera _camera;
	private int _leftPosition;
	// Use this for initialization
	void Start () {
		_levelM = GameObject.FindObjectOfType<LevelManager>();
		_HUDLevelsSprite = gameObject.GetComponentInChildren<OTSprite>();
		_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		_HUDLevelsSprite.frameIndex = _levelM.chosenVariation-1;
		if(_levelM.isBoss == true) _HUDLevelsSprite.frameIndex = 5;
		GameEventManager.GameStart += GameStart;
	}
	void FixedUpdate() {
		if(FETool.Round(((float) Screen.width/(float) Screen.height),1)!=1.3) 
			_leftPosition = 78;
		else _leftPosition = 63;
		gameObject.transform.position = _camera.ScreenToWorldPoint(new Vector3(_leftPosition, Screen.height-Screen.height/10, _camera.nearClipPlane));
	}
	void GameStart () {	
		_HUDLevelsSprite.frameIndex = _levelM.chosenVariation-1;
		if(_levelM.isBoss == true) _HUDLevelsSprite.frameIndex = 5;
	}
}
