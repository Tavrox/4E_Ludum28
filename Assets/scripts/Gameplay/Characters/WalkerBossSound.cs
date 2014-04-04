using UnityEngine;
using System.Collections;

public class WalkerBossSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating("bossMoaning",0f,10f);
		GameEventManager.GameStart += GameStart;
	}
	
	private void bossMoaning()
	{
		MasterAudio.PlaySound ("boss_moaning");
		//FESound.playDistancedSound("boss_moaning",gameObject.transform, _player.transform,0f);
	}
	// Update is called once per frame
	void Update () {
	
	}
	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
		if(this != null && gameObject.activeInHierarchy) {
			//InvokeRepeating("bossMoaning",0f,15f);
		}
	}
}
