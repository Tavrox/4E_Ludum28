using UnityEngine;
using System.Collections;

public class SplashMedalAnimations : MonoBehaviour {

    public OTAnimatingSprite _mySprite;

	// Use this for initialization
	void Start () {
        _mySprite = GetComponentInChildren<OTAnimatingSprite>();
        _mySprite.Stop();
        _mySprite.frameIndex = 0;
        _mySprite.visible = false;
	}
	
    public void playSplash() {
        _mySprite.visible = true;
        _mySprite.PlayOnce();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
