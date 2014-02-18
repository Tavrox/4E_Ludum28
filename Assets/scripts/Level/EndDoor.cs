using UnityEngine;
using System.Collections;

public class EndDoor : MonoBehaviour {

	public OTSprite sprite;
	public bool triggered;
	public int levelToGo = 0;
	private Player _player;
	private GameObject _UINeedKey;
	//private PlayerData _playerdata;


	void Start() {
		_player = GameObject.Find("Player").GetComponent<Player>();
		_UINeedKey = GameObject.Find("Player/IngameUI/NeedKey").gameObject;
		//_playerdata = _player.GetComponent<PlayerData>();
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.NextInstance += NextInstance;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !triggered)
		{
			if ((Input.GetKeyDown(KeyCode.F) || Input.GetKey("space")) && _player.hasFinalKey == true)
			{
				sprite.frameIndex += 1;
				triggered = true;
				StartCoroutine("lastFrameBuzzer");
				//Destroy (_UINeedKey);
				finishLevel();
				MasterAudio.PlaySound("key_door");
			}
			else if ((Input.GetKeyDown(KeyCode.F) || Input.GetKey("space"))  && _player.hasFinalKey == false)
			{
				_UINeedKey.GetComponent<IngameUI>().fadeOut();

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

//		_playerdata.addLevelUnlocked(levelToGo);

		StartCoroutine("EndGame");
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(5f);
		GameEventManager.TriggerNextInstance();
		//Application.LoadLevel(levelToGo);
	}
	
	private void NextInstance ()
	{
	}
	private void NextLevel ()
	{
	}
}
