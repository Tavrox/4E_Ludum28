using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelChooser : MonoBehaviour {

	private TextUI levelName;
	private GameSetup SETUP;
	private PlayerData PLAYERDATA;

	private GameObject ThumbGO;
	private List<LevelThumbnail> Thumbs =  new List<LevelThumbnail>();
	public List<MiscButton> LvlBtns =  new List<MiscButton>();
	private Vector3 gapThumbs = new Vector3(20f, 0f, -10f);
	private MiscButton[] ArrMiscBtns;
	
	public void Setup () 
	{
		SETUP = MainTitleUI.getSetup();
		PLAYERDATA = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
		LevelThumbnail[] ArrThums = GetComponentsInChildren<LevelThumbnail>();
		ArrMiscBtns = GetComponentsInChildren<MiscButton>();
		foreach (LevelThumbnail _th in ArrThums)
		{
			Thumbs.Add(_th);
			_th.Setup();
		}
		ThumbGO = new GameObject("Thumbnails");
	}
	public void unsplashLvlButtons() {
		foreach (MiscButton _mBtn in ArrMiscBtns) {
			if(_mBtn.buttonType == MiscButton.buttonList.PlayLevel) {
				if(_mBtn.splashed) {
					_mBtn.splashed=false;
					_mBtn._animBtn.frameIndex = 6;
					_mBtn._animBtn.PlayOnceBackward("activated");
				}
			}
		}	
	}
	public void setLvlOccButtons(int numLevel) {
		foreach (MiscButton _mBtn in ArrMiscBtns) {
			if(_mBtn.buttonType == MiscButton.buttonList.PlayOccurence) {
					_mBtn._lvlToLoad = numLevel.ToString();
			}
		}	
	}
	public void lockOccButtons(string numLevel) {
		foreach (MiscButton _mBtn in ArrMiscBtns) {
			if(_mBtn.buttonType == MiscButton.buttonList.PlayOccurence) {
				_mBtn.meLocked = _mBtn.locked = (_mBtn._playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel+"/occ"+((_mBtn.numOccurence==6)?"Boss":(_mBtn.numOccurence==7)?"6":_mBtn.numOccurence.ToString()) as string,"locked")=="true")?true:false;
				_mBtn.setOccurenceFrame();
				if(numLevel=="0" && (_mBtn.numOccurence==6 || _mBtn.numOccurence==7)) _mBtn.alphaBoss(0);
				else _mBtn.alphaBoss(.75f);
			}
		}	
	}
}