using UnityEngine;
using System.Collections;

public class BatteryLevels : MonoBehaviour {

	private LevelManager _levelM;
	public OTSprite _batteryColorSprite;
	public bool twinkle;

	// Use this for initialization
	void Start () {
		_levelM = GameObject.FindObjectOfType<LevelManager>();
		_batteryColorSprite = gameObject.GetComponentInChildren<OTSprite>();
		_batteryColorSprite.frameIndex = _levelM.chosenVariation+41;
		if(_levelM.isBoss == true) _batteryColorSprite.frameIndex = 46;
		GameEventManager.NextInstance += NextInstance;
		if(twinkle) StartCoroutine("twinkleLittleStar");
	}
	void NextInstance () {	
		_batteryColorSprite.frameIndex = _levelM.chosenVariation-1;
		if(_levelM.isBoss == true) _batteryColorSprite.frameIndex = 5;
	}
	private IEnumerator twinkleLittleStar() {
		_batteryColorSprite.GetComponent<MeshRenderer>().enabled = !_batteryColorSprite.GetComponent<MeshRenderer>().enabled;
		yield return new WaitForSeconds(0.5f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		if(twinkle) StartCoroutine("twinkleLittleStar");
	}
	public void batteryOK() {
		StopCoroutine("twinkleLittleStar");
		_batteryColorSprite.GetComponent<MeshRenderer>().enabled = true;
		twinkle = false;
	}
}
