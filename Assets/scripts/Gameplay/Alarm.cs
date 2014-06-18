using UnityEngine;
using System.Collections;

public class Alarm : MonoBehaviour {
	
	private OTAnimatingSprite anim;
	private Timer timer;
	private bool activated;
	
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		timer = GameObject.Find("Player/IngameUI/Timer").GetComponent<Timer>();
		anim.Play("off");
		activated = false;
		GameEventManager.GameStart += GameStart;
	}
	
	// Update is called once per frame
	void Update () {
		if(!anim.isPlaying && timer.secLeft<=15) anim.Play("rotate");
		if(timer.secLeft==15 && !activated) {
			anim.PlayOnce("on");
		}
	}
	
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy) {
			anim.Play("off");
			activated = false;
		}
	}
}
