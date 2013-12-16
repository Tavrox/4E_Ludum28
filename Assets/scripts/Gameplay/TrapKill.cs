using UnityEngine;
using System.Collections;

public class TrapKill : MonoBehaviour {

	private bool triggered = false;

//	// Use this for initialization
//	void Start () {
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.CompareTag("Player") && triggered == false) 
		{
			triggered = true;
			MasterAudio.PlaySound("hole");
			GameEventManager.TriggerGameOver();
		}
	}
}
