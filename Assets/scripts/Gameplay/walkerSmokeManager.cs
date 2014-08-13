using UnityEngine;
using System.Collections;

public class walkerSmokeManager : MonoBehaviour {
	
	public enum sideSmoke {
		Left,
		Right
	}
	public sideSmoke _mySide;
	public OTAnimatingSprite [] _tabSmoke;
	public OTAnimatingSprite _endSmoke;
	public int nbSpriteToDisplay=0, nbSpriteTotal=0;
	public float positionX, positionSpace;
	private OTTween _rescale, _slideScaled, _replace;
	public float opacity;
	
	// Use this for initialization
	void Start () {
		_tabSmoke = gameObject.GetComponentsInChildren<OTAnimatingSprite>();
		positionSpace = _tabSmoke[0].size.x;
		positionX = (_mySide==sideSmoke.Left)?-_tabSmoke[0].size.x/2:_tabSmoke[0].size.x/2;
//		print(positionX);
		int i = 0;
		foreach(OTAnimatingSprite anim in _tabSmoke)
		{
			if(i%2==0)	anim.Play("flow");
			else anim.Play("flow");
			anim.alpha=opacity;
			anim.transform.parent.transform.localPosition = new Vector3(positionX,anim.transform.parent.transform.localPosition.y,anim.transform.parent.transform.localPosition.z);
			positionX=(_mySide==sideSmoke.Left)?positionX-positionSpace:positionX+positionSpace;
			nbSpriteTotal++;
			i++;
		}
		if(i%2==0) {_endSmoke.Play((_mySide==sideSmoke.Left)?"left":"right");_endSmoke.transform.parent.transform.localScale=new Vector3(1f, _endSmoke.transform.parent.transform.localScale.y,_endSmoke.transform.parent.transform.localScale.z);}
		else {_endSmoke.Play((_mySide==sideSmoke.Left)?"right":"left");_endSmoke.transform.parent.transform.localScale=new Vector3(-1f, _endSmoke.transform.parent.transform.localScale.y,_endSmoke.transform.parent.transform.localScale.z);}
		_endSmoke.transform.parent.transform.localPosition = new Vector3((_mySide==sideSmoke.Left)?positionX:positionX,_tabSmoke[0].transform.parent.transform.localPosition.y,_endSmoke.transform.parent.transform.position.z);
		_endSmoke.alpha=opacity;
	}

	public void showNbSmoke() {
		int i = 0;
//		print (nbSpriteToDisplay);
		positionX = (_mySide==sideSmoke.Left)?-_tabSmoke[0].size.x/2:_tabSmoke[0].size.x/2;
		foreach(OTAnimatingSprite anim in _tabSmoke)
		{
			if(i<nbSpriteToDisplay) {
				//anim.alpha=1;
				if(anim.transform.parent.transform.localScale.x!=1f) {
					/*_rescale =*/ new OTTween(anim.transform.parent.transform, .1f, OTEasing.Linear).Tween("localScale", new Vector3(((i%2==0)?1f:-1f),anim.transform.parent.transform.localScale.y,anim.transform.parent.transform.localScale.z)); //Resize dernier élément
					/*_slideScaled =*/ new OTTween(anim.transform.parent.transform, .1f, OTEasing.Linear).Tween("localPosition", new Vector3(positionX,anim.transform.parent.transform.localPosition.y,anim.transform.parent.transform.localPosition.z)); //Placement du ENDSMOKE
	
				}
				else anim.transform.parent.transform.localPosition = new Vector3(positionX,anim.transform.parent.transform.localPosition.y,anim.transform.parent.transform.localPosition.z);
				positionX=(_mySide==sideSmoke.Left)?positionX-positionSpace:positionX+positionSpace;
			}
			else {
//				if(i==nbSpriteTotal-1) {
				if(anim.transform.parent.transform.localScale.x==1f || anim.transform.parent.transform.localScale.x==-1f) {
					/*_rescale = */new OTTween(anim.transform.parent.transform, .1f, OTEasing.Linear).Tween("localScale", new Vector3(0f,anim.transform.parent.transform.localScale.y,anim.transform.parent.transform.localScale.z)); //Resize dernier élément
					/*_slideScaled = */new OTTween(anim.transform.parent.transform, .1f, OTEasing.Linear).Tween("localPosition", new Vector3(anim.transform.parent.transform.localPosition.x+((_mySide==sideSmoke.Left)?positionSpace/2:-positionSpace/2),anim.transform.parent.transform.localPosition.y,anim.transform.parent.transform.localPosition.z)); //Placement du ENDSMOKE
		//(_mySide==sideSmoke.Left)?
				}
			}
			i++;
		}
		/*_replace =*/ new OTTween(_endSmoke.transform.parent.transform, .1f, OTEasing.Linear).Tween("localPosition", new Vector3(positionX,_tabSmoke[0].transform.parent.transform.localPosition.y,_endSmoke.transform.parent.transform.localPosition.z)); //Placement du ENDSMOKE
		//_endSmoke.transform.parent.transform.localPosition = new Vector2((_mySide==sideSmoke.Left)?positionX:positionX,_tabSmoke[0].transform.parent.transform.localPosition.y);
		//positionEndSprite = positionX=(_mySide==sideSmoke.Left)?positionX-positionSpace:positionX+positionSpace;
		//Via un tween + un tween de décalage vers la droite du dernier sprite avec échelle pour le faire disparaitre en slide. Bonjour la synchro bricolée.
	}
	public void displayAll() {
		foreach(OTAnimatingSprite anim in _tabSmoke)
		{
			//anim.alpha=1;
			anim.gameObject.SetActive(true);
			_endSmoke.gameObject.SetActive(true);
		}
	}
	public void hideAll() {
		foreach(OTAnimatingSprite anim in _tabSmoke)
		{
//			anim.alpha=0;
			anim.gameObject.SetActive(false);
			_endSmoke.gameObject.SetActive(false);
		}
	}
}
//int i = 0;
//		positionX = (_mySide==sideSmoke.Left)?-_tabSmoke[0].size.x/2:_tabSmoke[0].size.x/2;
//		foreach(OTAnimatingSprite anim in _tabSmoke)
//		{
//			
//			if(i<=nbSpriteToDisplay) {
//				anim.transform.parent.transform.localPosition = new Vector2(positionX,anim.transform.parent.transform.localPosition.y);//anim.alpha=1;
//				positionX=(_mySide==sideSmoke.Left)?positionX-positionSpace:positionX+positionSpace;
//			}
//			//else anim.alpha=0;
//			i++;
//		}