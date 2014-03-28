using UnityEngine;
using System.Collections;

public class InvasionAnims : MonoBehaviour {
	
	public OTAnimatingSprite animSprite;
	private Player _player;
	private bool stopped;
	// Use this for initialization
	void Start ()
	{
		animSprite.frameIndex = 0;
		_player = GameObject.Find("Player").GetComponent<Player>();
		stopped = false;
		invade();
		if(!_player.killedByBlob) animSprite.renderer.enabled = false;
	}
	void Update () {
		if(animSprite.frameIndex == 18 && !stopped) {
			stopped = true;
			animSprite.Stop();
			MasterAudio.PlaySound("rewind");
			_player.angleRotation = 0;
			_player.StartCoroutine("rewind");
			_player.StartCoroutine("stopRewind",1.3f);
			StartCoroutine("reset");
		}
	}
	public IEnumerator reset() {
		//print ("reset");
		yield return new WaitForSeconds(2.25f);//print ("DE-invade");
		animSprite.PlayBackward("invade");
		_player.transform.position = _player.spawnPos;
		transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,-5f);
		StartCoroutine("finalReset");

	}
	public void invade () {
		//print ("invade");
		transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,-5f);
		_player.transform.position = new Vector3 (_player.transform.position.x,_player.transform.position.y,-20f);
		animSprite.Play("invade");
	}
	public IEnumerator finalReset() {
		yield return new WaitForSeconds(1f);
		stopped = false;
		_player.StopCoroutine("rewind");
		_player.StopCoroutine("stopRewind");
		Destroy(gameObject);
	}
}
