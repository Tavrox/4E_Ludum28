using UnityEngine;
using System.Collections;

public class EndDoor : MonoBehaviour {

	public OTSprite sprite;
	public bool playerHasKey, triggered;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !triggered)
		{
			if (Input.GetKeyDown(KeyCode.F) && playerHasKey)
			{
				sprite.frameIndex += 1;
				triggered = true;
				StartCoroutine("lastFrameBuzzer");
				finishLevel();
			}
			else if (Input.GetKeyDown(KeyCode.F) && !playerHasKey) print ("GET THE KEY !m!");
		}
	}
	private IEnumerator lastFrameBuzzer () {
		yield return new WaitForSeconds(1f);
		sprite.frameIndex += 1;
	}
	private void finishLevel() {
		print ("finish level function");
	}
}
