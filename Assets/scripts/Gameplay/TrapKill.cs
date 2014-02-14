using UnityEngine;
using System.Collections;

public class TrapKill : MonoBehaviour {

	private bool triggered = false;
	private Player _player;

//	// Use this for initialization
	void Start () {
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	void OnTriggerEnter(Collider _other) 
	{
		if (_other.CompareTag("Player") == true && GameObject.Find("Player").GetComponent<Player>().isDead == false)
		{
			FESound.playDistancedSound("hole",gameObject.transform, _player.transform,0f);//MasterAudio.PlaySound("hole");
			GameObject.Find("Player").GetComponent<Player>().isDead = true;
			GameEventManager.TriggerGameOver();
		}
	}
}
