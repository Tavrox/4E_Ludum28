﻿using UnityEngine;
using System.Collections;

public class TeleportOUTAnims : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	// Use this for initialization
	void Start ()
	{
		animSprite.Play("teleportOUT");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
