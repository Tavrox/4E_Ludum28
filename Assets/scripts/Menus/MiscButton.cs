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
		BackHome,
		PlayOccurence
	};
	public GameSetup SETUP;
	public GameSetup.LevelList levelToLoad;
	public GameSaveLoad _levelDataLoader, _playerDataLoader;
	public LevelThumbnail _thumb;
	private string _levelHovered="-1";
	private OTTween _scoreTween, _occurencesTween;
	public BoxCollider _coll;
	public MainTitleUI mainUi;
	public LevelChooser chooser;
	
	public string _lvlToLoad = "0";
	public bool locked = false,splashed, meLocked;
	public int numOccurence;
	public buttonList buttonType;
	public OTAnimatingSprite _animBtn;
	private OTSprite _spriteBtn;
	public BlobBtnAnimations _animBlob;
	public ThumbnailAnimations _animLvlBlob;
	private PlayerData _playerData;
	private TextMesh txtGoldLvl, txtSilverLvl, txtBronzeLvl, txtScoreLvl, txtNumLvl,
						txtGold1, txtSilver1, txtBronze1, txtScore1,
						txtGold2, txtSilver2, txtBronze2, txtScore2,
						txtGold3, txtSilver3, txtBronze3, txtScore3,
						txtGold4, txtSilver4, txtBronze4, txtScore4,
						txtGold5, txtSilver5, txtBronze5, txtScore5,
						txtGold6, txtSilver6, txtBronze6, txtScore6,
						txtGoldBoss, txtSilverBoss, txtBronzeBoss, txtScoreBoss;
	private Transform _levelDetails, _occurenceSelector;
	private OTSprite _playerMedalLvl, _playerMedal1,_playerMedal2,_playerMedal3,_playerMedal4,_playerMedal5,_playerMedal6,_playerMedalBoss;
//	void Awake() {
//	
//		_levelDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
//		_playerDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
//		_levelDataLoader._FileLocation=Application.dataPath;
//		_playerDataLoader._FileLocation=Application.dataPath;
//	//	print(_levelDataLoader._FileLocation);	
//	}
	void Start()
	{
		txtGoldLvl = GameObject.Find("goldlvl").GetComponent<TextMesh>();txtSilverLvl = GameObject.Find("silverlvl").GetComponent<TextMesh>();
		txtBronzeLvl = GameObject.Find("bronzelvl").GetComponent<TextMesh>();txtScoreLvl = GameObject.Find("scorelvl").GetComponent<TextMesh>();
		txtNumLvl = GameObject.Find("txtNumLvl").GetComponent<TextMesh>();
		txtGold1 = GameObject.Find("gold1").GetComponent<TextMesh>();txtSilver1 = GameObject.Find("silver1").GetComponent<TextMesh>();
		txtBronze1 = GameObject.Find("bronze1").GetComponent<TextMesh>();txtScore1 = GameObject.Find("score1").GetComponent<TextMesh>();
		
		txtGold2 = GameObject.Find("gold2").GetComponent<TextMesh>();txtSilver2 = GameObject.Find("silver2").GetComponent<TextMesh>();
		txtBronze2 = GameObject.Find("bronze2").GetComponent<TextMesh>();txtScore2 = GameObject.Find("score2").GetComponent<TextMesh>();
		
		txtGold3 = GameObject.Find("gold3").GetComponent<TextMesh>();txtSilver3 = GameObject.Find("silver3").GetComponent<TextMesh>();
		txtBronze3 = GameObject.Find("bronze3").GetComponent<TextMesh>();txtScore3 = GameObject.Find("score3").GetComponent<TextMesh>();
		
		txtGold4 = GameObject.Find("gold4").GetComponent<TextMesh>();txtSilver4 = GameObject.Find("silver4").GetComponent<TextMesh>();
		txtBronze4 = GameObject.Find("bronze4").GetComponent<TextMesh>();txtScore4 = GameObject.Find("score4").GetComponent<TextMesh>();
		
		txtGold5 = GameObject.Find("gold5").GetComponent<TextMesh>();txtSilver5 = GameObject.Find("silver5").GetComponent<TextMesh>();
		txtBronze5 = GameObject.Find("bronze5").GetComponent<TextMesh>();txtScore5 = GameObject.Find("score5").GetComponent<TextMesh>();
		
		txtGold6 = GameObject.Find("gold6").GetComponent<TextMesh>();txtSilver6 = GameObject.Find("silver6").GetComponent<TextMesh>();
		txtBronze6 = GameObject.Find("bronze6").GetComponent<TextMesh>();txtScore6 = GameObject.Find("score6").GetComponent<TextMesh>();
		
		txtGoldBoss = GameObject.Find("goldBoss").GetComponent<TextMesh>();txtSilverBoss = GameObject.Find("silverBoss").GetComponent<TextMesh>();
		txtBronzeBoss = GameObject.Find("bronzeBoss").GetComponent<TextMesh>();txtScoreBoss = GameObject.Find("scoreBoss").GetComponent<TextMesh>();
		
		_playerMedalLvl = GameObject.Find("playerMedalLvl").GetComponent<OTSprite>();
		_playerMedal1 = GameObject.Find("playerMedal1").GetComponent<OTSprite>();
		_playerMedal2 = GameObject.Find("playerMedal2").GetComponent<OTSprite>();
		_playerMedal3 = GameObject.Find("playerMedal3").GetComponent<OTSprite>();
		_playerMedal4 = GameObject.Find("playerMedal4").GetComponent<OTSprite>();
		_playerMedal5 = GameObject.Find("playerMedal5").GetComponent<OTSprite>();
		_playerMedal6 = GameObject.Find("playerMedal6").GetComponent<OTSprite>();
		_playerMedalBoss = GameObject.Find("playerMedalBoss").GetComponent<OTSprite>();
		
		_occurenceSelector = GameObject.Find("OccurenceSelector").GetComponent<Transform>();
		_levelDetails = GameObject.Find("LevelDetails").GetComponent<Transform>();
		
		_levelDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
		_playerDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
		_levelDataLoader.LoadXMLToList("blob_minute-levels");
		_playerDataLoader.LoadXMLToList("blob_minute-players");
		
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
			_animBtn.PlayLoop("static");
			_animBtn.speed = .8f;
		}
		if (GetComponentInChildren<OTSprite>() != null)
		{
			_spriteBtn = GetComponentInChildren<OTSprite>();
		}
		if(GameObject.Find("PlayerData") != null) _playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
//		if (GetComponent<BlobBtnAnimations>() != null)
//		{
//			_animBlob = GetComponent<BlobBtnAnimations>();
//			_animBtn.PlayLoop("static");
//			_animBtn.speed = 1f;
//		}
		_thumb = GetComponent<LevelThumbnail>();
		//_thumb.Setup();
		chooser = GameObject.Find("LevelChooser").GetComponent<LevelChooser>();
		if(buttonType == buttonList.PlayOccurence) {
			setOccurenceFrame();
			//print(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_lvlToLoad,"locked"));
			//locked = (_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ"+(numOccurence!=6)?numOccurence:"Boss","locked")=="false")?false:true;
			
			if(!locked) _spriteBtn.alpha = .75f;
		}
		
//		if(_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"locked")=="true") {
//			locked = true;
//		}
//		else locked = false;
//		if (GetComponentInChildren<ThumbnailAnimations>() != null)
//		{
//			_animLvlBlob = GetComponent<ThumbnailAnimations>();
//			print ("olo");
//			_animBtn.PlayLoop("activate");
////			_animBtn.speed = 1f;
//		}
	}
	void Update() {
		if(meLocked) locked=true;
		else locked=false;
	}
	public void setOccurenceFrame() {
		if(locked) _spriteBtn.frameIndex = numOccurence-1;
		else _spriteBtn.frameIndex = numOccurence+6;
	}
	public void alphaBoss(float opacity) {
		_spriteBtn.alpha = opacity;	
	}
	void OnMouseExit()
	{
		if (locked == false)
		{
			if(buttonType == buttonList.PlayLevel && !mainUi.lvlSplashed) {
				_scoreTween = new OTTween(_levelDetails.transform ,.4f, OTEasing.QuadInOut).Tween("localPosition", new Vector3( _levelDetails.transform.localPosition.x, -4f, _levelDetails.transform.localPosition.z ));
			}
			
			if(buttonType == buttonList.PlayOccurence && _spriteBtn.alpha !=0) {
					_spriteBtn.alpha = .75f;
					_spriteBtn.size -= new Vector2(0.3f,0.3f);
			}
		}
	}
	void OnMouseEnter()
	{
		if (locked == false)
		{
			if(buttonType == buttonList.PlayLevel) {
				if(!mainUi.lvlSplashed) _scoreTween = new OTTween(_levelDetails.transform ,.4f, OTEasing.QuadInOut).Tween("localPosition", new Vector3( _levelDetails.transform.localPosition.x, 1.5f, _levelDetails.transform.localPosition.z ));
//				print (_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"id"));
//				print (_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"gold"));
//				print (_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"silver"));
//				print (_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"bronze"));
//				print (_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"locked"));
//				print (_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien","name"));
				txtGoldLvl.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"gold");
				txtSilverLvl.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"silver");
				txtBronzeLvl.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName,"bronze");
				txtScoreLvl.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName,"score");
				txtNumLvl.text = _thumb.Info.levelNumName;
								
				txtGold1.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ1","gold");
				txtSilver1.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ1","silver");
				txtBronze1.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ1","bronze");
				txtScore1.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ1","score");
				
				txtGold2.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ2","gold");
				txtSilver2.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ2","silver");
				txtBronze2.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ2","bronze");
				txtScore2.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ2","score");
				
				txtGold3.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ3","gold");
				txtSilver3.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ3","silver");
				txtBronze3.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ3","bronze");
				txtScore3.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ3","score");
				
				txtGold4.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ4","gold");
				txtSilver4.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ4","silver");
				txtBronze4.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ4","bronze");
				txtScore4.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ4","score");
				
				txtGold5.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ5","gold");
				txtSilver5.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ5","silver");
				txtBronze5.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ5","bronze");
				txtScore5.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ5","score");
				
				txtGold6.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ6","gold");
				txtSilver6.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ6","silver");
				txtBronze6.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occ6","bronze");
				txtScore6.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occ6","score");
				
				txtGoldBoss.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occBoss","gold");
				txtSilverBoss.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occBoss","silver");
				txtBronzeBoss.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_thumb.Info.levelNumName+"/occBoss","bronze");
				txtScoreBoss.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_thumb.Info.levelNumName+"/occBoss","score");
				
				_playerMedalLvl.frameIndex = displayPlayerMedal(txtScoreLvl.text, txtGoldLvl.text, txtSilverLvl.text, txtBronzeLvl.text);
				_playerMedal1.frameIndex = displayPlayerMedal(txtScore1.text, txtGold1.text, txtSilver1.text, txtBronze1.text);
				_playerMedal2.frameIndex = displayPlayerMedal(txtScore2.text, txtGold2.text, txtSilver2.text, txtBronze2.text);
				_playerMedal3.frameIndex = displayPlayerMedal(txtScore3.text, txtGold3.text, txtSilver3.text, txtBronze3.text);
				_playerMedal4.frameIndex = displayPlayerMedal(txtScore4.text, txtGold4.text, txtSilver4.text, txtBronze4.text);
				_playerMedal5.frameIndex = displayPlayerMedal(txtScore5.text, txtGold5.text, txtSilver5.text, txtBronze5.text);
				_playerMedal6.frameIndex = displayPlayerMedal(txtScore6.text, txtGold6.text, txtSilver6.text, txtBronze6.text);
				_playerMedalBoss.frameIndex = displayPlayerMedal(txtScoreBoss.text, txtGoldBoss.text, txtSilverBoss.text, txtBronzeBoss.text);
			}
			
			if(buttonType == buttonList.PlayOccurence && _spriteBtn.alpha !=0) {
					_spriteBtn.alpha = 1f;
					_spriteBtn.size += new Vector2(0.3f,0.3f);
				//	print(_lvlToLoad + " - " + numOccurence);
			}
		}
	}
	int displayPlayerMedal(string playerScore, string goldScore, string silverScore, string bronzeScore) {
		int numFrame = 0;
		if(playerScore=="") playerScore="0";if(goldScore=="") goldScore="0";if(silverScore=="") silverScore="0";if(bronzeScore=="") bronzeScore="0";
		if(System.Convert.ToInt32(playerScore) >= System.Convert.ToInt32(bronzeScore)) numFrame = 1;
		if(System.Convert.ToInt32(playerScore) >= System.Convert.ToInt32(silverScore)) numFrame = 2;
		if(System.Convert.ToInt32(playerScore) >= System.Convert.ToInt32(goldScore)) numFrame = 3;
		return numFrame;
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
				print ("this button does nothing bro" + gameObject.name);
				break;
			}
			case buttonList.PlayOccurence :
			{
					//_spriteBtn.alpha = 1f;
				_playerData.choixOccurence = System.Convert.ToInt32(numOccurence);
				if(numOccurence==6) _lvlToLoad = System.Convert.ToString(System.Convert.ToInt32(numOccurence)+1);
				if(numOccurence==7) _lvlToLoad = System.Convert.ToString(System.Convert.ToInt32(numOccurence)+1);//_lvlToLoad = System.Convert.ToInt32(_lvlToLoad+?); NUM SCENE OCC 6
				print(_lvlToLoad + " - " + numOccurence);
				Application.LoadLevel(System.Convert.ToInt32(_lvlToLoad));
				break;
			}
			case buttonList.PlayLevel :
			{
				if(!splashed) {
					mainUi.lvlSplashed=true;
					_animBtn.PlayOnce("activated");
					_thumb = GetComponent<LevelThumbnail>();
					int lvl = _thumb.Info.levelID;
					//_thumb.Info.locked = false;
					if (_thumb.Info.locked == false)
					{
						_occurencesTween = new OTTween(_occurenceSelector.transform ,.4f, OTEasing.ExpoIn).Tween("localPosition", new Vector3( this.transform.localPosition.x, this.transform.localPosition.y, _occurenceSelector.transform.localPosition.z ));
						chooser.unsplashLvlButtons();
						splashed=true;
						chooser.setLvlOccButtons(lvl);
						chooser.lockOccButtons(_thumb.Info.levelNumName);
						//Application.LoadLevel(lvl);
					}
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
//				if (_animBlob != null)
//				{
					MasterAudio.PlaySound("blob_explosion");
					_animBtn.PlayOnce("activate");
					_animBtn.speed = 1.5f;
//				}
				StartCoroutine(DoTransition(mainUi.LevelChooser));
				break;
			}
			case buttonList.OpenCredits :
			{
//				if (_animBlob != null)
//				{
					MasterAudio.PlaySound("blob_explosion");
					_animBtn.PlayOnce("activate");
					_animBtn.speed = 1.5f;
//				}
				StartCoroutine(DoTransition(mainUi.Credits));
				break;
			}
			case buttonList.BackHome :
			{
//				if (_animBlob != null)
//				{
					_occurencesTween = new OTTween(_occurenceSelector.transform ,.4f, OTEasing.ExpoIn).Tween("localPosition", new Vector3( 1, 7.5f, _occurenceSelector.transform.localPosition.z ));
					_scoreTween = new OTTween(_levelDetails.transform ,.4f, OTEasing.QuadInOut).Tween("localPosition", new Vector3( _levelDetails.transform.localPosition.x, -4f, _levelDetails.transform.localPosition.z ));	
					chooser.unsplashLvlButtons();
					mainUi.lvlSplashed = locked = false;
					MasterAudio.PlaySound("blob_explosion");
					_animBtn.PlayOnce("activate");
					_animBtn.speed = 1.5f;
//				}
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
		_animBtn.PlayLoop("static");
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
			meLocked = true;
		}
	}

	IEnumerator unlockEveryButton()
	{
		yield return new WaitForSeconds(1.1f);
		MiscButton[] allBtn = GameObject.FindObjectsOfType(typeof(MiscButton)) as MiscButton[];
		foreach (MiscButton _spr in allBtn)
		{
			_spr.locked = false;
			meLocked = false;
		}
	}
}
