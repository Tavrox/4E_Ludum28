using UnityEngine;
using System.Collections;

public class EndDoor : MonoBehaviour {

	public OTSprite sprite;
	public bool triggered;
	public int levelToGo = 0;
	private Player _player;
	private GameObject _UINeedKey;
	private Timer _lvlTimer;
	//private PlayerData _playerdata;
	
	public InputManager InputMan;

	void Start() {
		_player = GameObject.Find("Player").GetComponent<Player>();
		_UINeedKey = GameObject.Find("Player/IngameUI/NeedKey").gameObject;
		_lvlTimer = GameObject.Find("Player/IngameUI/Timer").GetComponent<Timer>();
		//_playerdata = _player.GetComponent<PlayerData>();
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.NextInstance += NextInstance;
		GameEventManager.FinishLevel += FinishLevel;
		InputMan = Instantiate(Resources.Load("Tuning/InputManager")) as InputManager;
		InputMan.Setup();
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !triggered)
		{
			if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2)) && _player.hasFinalKey == true)
			{
				sprite.frameIndex += 1;
				triggered = _player.finishedLevel = true;
				StartCoroutine("lastFrameBuzzer");
				//Destroy (_UINeedKey);
				GameEventManager.TriggerFinishLevel();
				FinishLevel();
				MasterAudio.PlaySound("key_door");
			}
			else if ((Input.GetKeyDown(InputMan.Action) || Input.GetKeyDown(InputMan.Action2))  && _player.hasFinalKey == false)
			{
				_UINeedKey.GetComponent<IngameUI>().fadeOut();

			}
		}
	}
	private IEnumerator lastFrameBuzzer () {
		yield return new WaitForSeconds(1f);
		sprite.frameIndex += 1;
	}
	private void FinishLevel() {
		if(this != null && gameObject.activeInHierarchy) {
		_lvlTimer.pauseTimer = true;
		MasterAudio.PlaySound("win");
		MasterAudio.FadePlaylistToVolume(0f, 2f);
		MasterAudio.FadeAllPlaylistsToVolume(0f, 2f);
		MasterAudio.FadeOutAllOfSound("bg",2f);
		MasterAudio.FadeOutAllOfSound("intro",2f);
		MasterAudio.FadeOutAllOfSound("jam",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_1",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_2",2f);
			MasterAudio.FadeOutAllOfSound("level_theme_3",2f);
		//	MasterAudio.FadeOutAllOfSound("level_theme_4",2f);
		//	MasterAudio.FadeOutAllOfSound("level_theme_5",2f);
			MasterAudio.FadeOutAllOfSound("boss_theme",2f);

//		_playerdata.addLevelUnlocked(levelToGo);

		StartCoroutine("EndGame");
		}
	}

	IEnumerator EndGame()
	{
		yield return new WaitForSeconds(5f);
		MasterAudio.StopAllOfSound("bg");
		MasterAudio.StopAllOfSound("intro");
		MasterAudio.StopAllOfSound("jam");
		MasterAudio.StopAllOfSound("level_theme_1");
		MasterAudio.StopAllOfSound("level_theme_2");
		MasterAudio.StopAllOfSound("level_theme_3");
		//	MasterAudio.StopAllOfSound("level_theme_4");
		//	MasterAudio.StopAllOfSound("level_theme_5");
		MasterAudio.StopAllOfSound("boss_theme");
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
