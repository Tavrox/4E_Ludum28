using UnityEngine;
using System.Collections;

public class BlobSplash : MonoBehaviour {

	public int numBlobAnim=1, waitTime=0, delayBegin=0;
	private OTAnimatingSprite anim;
	private bool activated;
	[HideInInspector] public Transform thisTransform;
	private Vector3 posIni;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<OTAnimatingSprite>();
		anim.PlayOnce(numBlobAnim.ToString());
		anim.Stop();
		activated = false;
		thisTransform = transform;
		posIni = gameObject.transform.position;
		anim.onAnimationFinish = OnAnimationFinish;
	}
	
	// Update is called once per frame
	void Update () {
		if(!anim.isPlaying && !activated) {
			activated=true;
			StartCoroutine("playSplash");	
		}
	}
	private IEnumerator playSplash() {
//		anim.PlayLoop(numBlobAnim.ToString()+"Slide");
		yield return new WaitForSeconds(delayBegin);
		if(delayBegin!=0) anim.PlayOnce(numBlobAnim.ToString());
		yield return new WaitForSeconds(waitTime);
		anim.PlayOnce(numBlobAnim.ToString());
		yield return new WaitForSeconds(0.01f);
		transform.position = new Vector3((posIni.x+Random.Range(0,10)*0.1f),posIni.y, posIni.z);
		activated = false;
		delayBegin = 0;
	}
	public void OnAnimationFinish(OTObject owner)
    {
		if(owner == anim)
        	anim.PlayLoop(numBlobAnim.ToString()+"Slide");
    }
}
