using UnityEngine;
using System.Collections;

public class AnimStop : MonoBehaviour {
	
	public OTAnimatingSprite _animBtn;
	// Use this for initialization
	void Start () {
		if (GetComponentInChildren<OTAnimatingSprite>() != null)
		{
			_animBtn = GetComponentInChildren<OTAnimatingSprite>();
			_animBtn.PlayLoop("static");
			_animBtn.speed = .8f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
