using UnityEngine;
using System.Collections;

public class TeleportOUTAnims : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	[HideInInspector] public int decalage = -20;
	// Use this for initialization
	void Start ()
	{
		animSprite.Play("teleportOUT");
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
