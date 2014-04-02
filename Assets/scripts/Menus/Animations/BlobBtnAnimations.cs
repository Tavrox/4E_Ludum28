using UnityEngine;
using System.Collections;

public class BlobBtnAnimations : FEAnims {

	public OTAnimation BtnLvl;
	public OTAnimation BtnNoLvl;
	
	public string ACTIVATED;
	public string STATIC;
	
	// Use this for initialization
	void Start () 
	{
		BtnNoLvl = GameObject.Find("Frameworks/OT/Animations/blobBtn").GetComponent<OTAnimation>();
		ACTIVATED = "activated";
		STATIC = "static";
	}
}
