using UnityEngine;
using System.Collections;

public class MiscButton : MonoBehaviour {

	public enum buttonList
	{
		None,
		PlayLevel,
		MuteGlobal,
		MuteMusic,
		Facebook,
		Twitter,
		TwitterPublish,
		FacebookPublish,
		Website,
		GoHome,
		CloseGame,
		OpenOptions,
		OpenLevel,
		OpenCredits,
		BackHome
	};
	public GameSetup SETUP;
	public GameSetup.LevelList levelToLoad;
	public BoxCollider _coll;
	public MainTitleUI mainUi;
	public LevelChooser chooser;

	public bool locked = false;
	public buttonList buttonType;
	public OTAnimatingSprite _animBtn;
	public BlobBtnAnimations _animBlob;

	void Start()
	{
		SETUP = Resources.Load ("Tuning/GameSetup") as GameSetup;
		if (GameObject.Find("TitleMenu") != null)
		{
			mainUi = GameObject.Find("TitleMenu").GetComponent<MainTitleUI>();
		}
		if (_coll = GetComponent<BoxCollider>())
		{_coll.isTrigger = true;}

		if (GetComponentInChildren<OTAnimatingSprite>() != null)
		{
			_animBtn = GetComponentInChildren<OTAnimatingSprite>();
		}
		if (GetComponent<BlobBtnAnimations>() != null)
		{
			_animBlob = GetComponent<BlobBtnAnimations>();
			_animBtn.PlayLoop(_animBlob.STATIC);
			_animBtn.speed = 1f;
		}
	}

	void OnMouseDown()
	{
		if (locked == false)
		{
			LockButtons();
			switch (buttonType)
			{
			case buttonList.None :
			{
				print ("this button do nothing bro" + gameObject.name);
				break;
			}
			case buttonList.PlayLevel :
			{
				LevelThumbnail _thumb = GetComponent<LevelThumbnail>();
				int lvl = _thumb.Info.levelID;
				_thumb.Info.locked = false;
				if (_thumb.Info.locked == false)
				{
					Application.LoadLevel(lvl);
				}
				break;
			}
			case buttonList.MuteGlobal :
			{
				mainUi = GameObject.Find("TitleMenu").GetComponent<MainTitleUI>();
				mainUi.PLAYERDAT.MuteGlobal();
				break;
			}
			case buttonList.MuteMusic :
			{
				mainUi = GameObject.Find("TitleMenu").GetComponent<MainTitleUI>();
				mainUi.PLAYERDAT.MuteMusic();
				break;
			}
			case buttonList.Twitter :
			{
				Application.OpenURL(SETUP.twitter_url);
				break;
			}
			case buttonList.Facebook :
			{
				Application.OpenURL(SETUP.facebook_url);
				break;
			}
			case buttonList.TwitterPublish :
			{
				
				break;
			}
			case buttonList.FacebookPublish :
			{
				
				break;
			}
			case buttonList.Website :
			{
				Application.OpenURL(SETUP.website_url);
				break;
			}
			case buttonList.GoHome :
			{
				Application.LoadLevel(0);
				break;
			}
			case buttonList.CloseGame :
			{
				Application.Quit();
				break;
			}
			case buttonList.OpenOptions :
			{
				mainUi.makeTransition( mainUi.Options);
				break;
			}
			case buttonList.OpenLevel :
			{
				if (_animBlob != null)
				{
					MasterAudio.PlaySound("blob_explosion");
					_animBtn.PlayOnce(_animBlob.ACTIVATED);
					_animBtn.speed = 1.5f;
				}
				StartCoroutine(DoTransition(mainUi.LevelChooser));
				break;
			}
			case buttonList.OpenCredits :
			{
				if (_animBlob != null)
				{
					MasterAudio.PlaySound("blob_explosion");
					_animBtn.PlayOnce(_animBlob.ACTIVATED);
					_animBtn.speed = 1.5f;
				}
				StartCoroutine(DoTransition(mainUi.Credits));
				break;
			}
			case buttonList.BackHome :
			{
				if (_animBlob != null)
				{
					MasterAudio.PlaySound("blob_explosion");
					_animBtn.PlayOnce(_animBlob.ACTIVATED);
					_animBtn.speed = 1.5f;
				}
				StartCoroutine(DoTransition(mainUi.Landing));
				break;
			}
			}
//			print ("clicked" + buttonType);
		}
	}

	IEnumerator DoTransition(GameObject _thing)
	{
		yield return new WaitForSeconds(0.75f);
		mainUi.makeTransition(_thing);
		yield return new WaitForSeconds(0.4f);
		StartCoroutine("resetBlob");
	}

	IEnumerator resetBlob()
	{
		yield return new WaitForSeconds(0.5f);
		_animBtn.PlayLoop(_animBlob.STATIC);
		_animBtn.speed = 1f;
	}

	public void LockButtons()
	{
		lockEveryButton();
		StartCoroutine("unlockEveryButton");
	}

	public void lockEveryButton()
	{
		MiscButton[] allBtn = GameObject.FindObjectsOfType(typeof(MiscButton)) as MiscButton[];
		foreach (MiscButton _spr in allBtn)
		{
			_spr.locked = true;
		}
	}

	IEnumerator unlockEveryButton()
	{
		yield return new WaitForSeconds(1.1f);
		MiscButton[] allBtn = GameObject.FindObjectsOfType(typeof(MiscButton)) as MiscButton[];
		foreach (MiscButton _spr in allBtn)
		{
			_spr.locked = false;
		}
	}
}
