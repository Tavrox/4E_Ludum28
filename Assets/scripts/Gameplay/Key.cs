using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider _coll)
	{
		if (_coll.name == "Player")
		{
			GameObject.Find("Player").GetComponent<Player>().hasFinalKey = true;
			Destroy (gameObject);
		}

	}
}
