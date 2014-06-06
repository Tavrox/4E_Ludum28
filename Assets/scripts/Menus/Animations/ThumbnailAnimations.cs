using UnityEngine;
using System.Collections;

public class ThumbnailAnimations : FEAnims {

	public OTAnimation BtnLvl;
	public OTAnimation BtnNoLvl;

	public string ACTIVATED, STATIC;

	// Use this for initialization
	public void Setup () 
	{
		BtnNoLvl = GameObject.Find("Frameworks/OT/Animations/btnNoLvl").GetComponent<OTAnimation>();
		BtnLvl = GameObject.Find("Frameworks/OT/Animations/btnLvl").GetComponent<OTAnimation>();
		ACTIVATED = "activated";
		STATIC = "static";
	}
}
