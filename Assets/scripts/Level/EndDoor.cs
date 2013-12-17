using UnityEngine;
using System.Collections;

public class EndDoor : MonoBehaviour {

	public OTSprite sprite;
	public bool triggered;
	public int levelToGo = 0;


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
		MasterAudio.PlaySound("win");
		MasterAudio.FadePlaylistToVolume(0f, 2f);
		MasterAudio.FadeAllPlaylistsToVolume(0f, 2f);
		MasterAudio.FadeOutAllOfSound("bg",2f);
		MasterAudio.FadeOutAllOfSound("intro",2f);
		MasterAudio.FadeOutAllOfSound("jam",2f);

		PlayerData _playerdata = GameObject.Find("PlayerData").GetComponent<PlayerData>();
		_playerdata.addLevelUnlocked(levelToGo);

		StartCoroutine("EndGame");
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(5f);		
		Application.LoadLevel(levelToGo);
	}
}
