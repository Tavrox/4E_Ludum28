using UnityEngine;
using System.Collections;

public class EndDoor : MonoBehaviour {

	public OTSprite sprite;
	public bool triggered;
	public int levelToGo = 0;

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
			if (Input.GetKeyDown(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().hasFinalKey == true)
			{
				sprite.frameIndex += 1;
				triggered = true;
				StartCoroutine("lastFrameBuzzer");
				Destroy (GameObject.Find("Player/IngameUI/NeedKey").gameObject);
				finishLevel();
				MasterAudio.PlaySound("key_door");
			}
			else if (Input.GetKeyDown(KeyCode.F) && GameObject.Find("Player").GetComponent<Player>().hasFinalKey == false)
			{
				GameObject.Find("Player/IngameUI/NeedKey").GetComponent<IngameUI>().fadeOut();

			}
		}
	}
	private IEnumerator lastFrameBuzzer () {
		yield return new WaitForSeconds(1f);
		sprite.frameIndex += 1;
	}
	private void finishLevel() {
		Application.LoadLevel(levelToGo);
	}
}
