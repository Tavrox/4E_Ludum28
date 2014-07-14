using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

	[SerializeField] private Player player;
	[SerializeField] private Camera myCamera;
	
	public int ID, _realID;
	public int chosenVariation;
	public bool isBoss;
	public static BMTuning TuningDocument;

	private int _secLeft, _scoreTotalLvl,cptKey;
	private bool bestScore;
	private TileImporter _tileImporter;
	private PlayerData _pdata;
	private string _rand;
	private Vector3 spawnPoint;
	private PartyData _partyData;
	public GameSaveLoad _playerDataLoader,_levelDataLoader;
	private GameObject _EndLvlPanel, _EndLvlSprite, _EndLvlContent;
	public Transform _bulleTuto;
	private Timer _timer;
	private TextMesh lblLevel, lblScoreGold, lblScoreSilver, lblScoreBronze, lblScoreOld, lblScoreTime, lblScoreTotal, lblNBKey, lblScoreKey;
	private Key [] _collectedKeys;
	void Awake()
	{
		TuningDocument  = Instantiate(Resources.Load("Tuning/Global")) as BMTuning;
		
		if(GameObject.Find("PlayerData") != null){_pdata = GameObject.Find("PlayerData").GetComponent<PlayerData>();
			chosenVariation = _pdata.choixOccurence;}
//		if(GameObject.Find("PartyData") != null){_partyData = GameObject.Find("PartyData").GetComponent<PartyData>();
//			chosenVariation = _partyData.choixOccurence;}
	}
//	void Update () {
//		print (chosenVariation);
//	}
	// Use this for initialization
	void Start () 
	{
		
		lblLevel = GameObject.Find("Player/IngameUI/EndLVLPanel/content/LEVEL_ALL").GetComponent<TextMesh>();
		lblScoreGold = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreGold").GetComponent<TextMesh>();
		lblScoreSilver = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreSilver").GetComponent<TextMesh>();
		lblScoreBronze = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreBronze").GetComponent<TextMesh>();
		lblScoreOld = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreOld").GetComponent<TextMesh>();
		lblScoreTime = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreTime").GetComponent<TextMesh>();
		lblScoreTotal = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreTotal").GetComponent<TextMesh>();
		lblNBKey = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblNBKey").GetComponent<TextMesh>();
		lblScoreKey = GameObject.Find("Player/IngameUI/EndLVLPanel/content/lblScoreKey").GetComponent<TextMesh>();
		_EndLvlPanel = GameObject.Find("Player/IngameUI/EndLVLPanel").gameObject;
		_EndLvlSprite = GameObject.Find("Player/IngameUI/EndLVLPanel/panel").gameObject;
		_EndLvlContent = GameObject.Find("Player/IngameUI/EndLVLPanel/content").gameObject;
		if(_EndLvlSprite.transform.localScale.x != 10f) {
			_EndLvlContent.gameObject.SetActive(false);
			OTTween _endLvlSpriteAlphaTween = new OTTween(_EndLvlSprite.GetComponent<SpriteRenderer>() ,0.8f, OTEasing.ExpoIn).Tween("color", new Color(1f,1f,1f,0));
			OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartIn).Tween("localScale", new Vector3( 10f, 10f, 1f )).OnFinish(hideEndLvlPanel);
		}
		else {_EndLvlContent.gameObject.SetActive(false);_EndLvlPanel.gameObject.SetActive(false);}
		
		//DontDestroyOnLoad(this);
		if(chosenVariation==0 /*|| chosenVariation==5*/) chosenVariation = 1;//GameObject.Find("Level/TileImporter").GetComponent<TileImporter>().chosenVariation;
//		if (GameObject.FindWithTag("Player") != null)
		//		{
			player = GameObject.FindWithTag("Player").GetComponent<Player>();
//		}
		myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		_timer = GameObject.Find("Player/IngameUI/Timer").GetComponent<Timer>();
		_secLeft = _timer.secLeft;
		_tileImporter = GameObject.Find("Level/TileImporter").GetComponent<TileImporter>();
//		_pdata = player.GetComponent<PlayerData>();
//		Player _Player  = GameObject.Find("Player").GetComponent<Player>();
		ID = Application.loadedLevel;
		//print("ID : ---*-*-***---"+ID);
		//MasterAudio.PlaySound("jam");

		//if(chosenVariation==0) _rand = Random.Range(_tileImporter.minVariation, _tileImporter.maxVariation).ToString();
		//else _rand = chosenVariation.ToString();

		//print ("Level generated " + _rand);
		foreach (Transform _gameo in GameObject.Find("Level/ObjectImporter").transform)
		{
			if (_gameo.gameObject.name == chosenVariation.ToString() )
		    {
				_gameo.gameObject.SetActive(true);
			}
		}
		TranslateAllInScene();

		spawnPoint = GameObject.Find("playerspawn"+chosenVariation).transform.position;
		player.transform.position = player.spawnPos = new Vector3(spawnPoint.x,spawnPoint.y,-1f);
//		_pdata.displayInput = false;

		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.NextInstance += NextInstance;
		GameEventManager.FinishLevel += FinishLevel;
		
		playLevelMusic();
		_playerDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
		_playerDataLoader.LoadXMLToList("blob_minute-players");
		_levelDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
		_levelDataLoader.LoadXMLToList("blob_minute-levels");
		if(_realID==1 && chosenVariation==1 && _bulleTuto!=null) {
			_bulleTuto.gameObject.SetActive(true);
		}
	}
	
	private void FinishLevel() { //When end lvl panel display after endDoor Input
		_collectedKeys = player.GetComponentsInChildren<Key>();
		int k = 0;
		foreach(Key _k in _collectedKeys) {
			
			_k.transform.position = new Vector3 (_k.transform.position.x,_k.transform.position.y,-15f-0.1f*k);
			k++;
		}
//		print (_EndLvlPanel.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Transform>().gameObject.name);
		_EndLvlPanel.gameObject.SetActive(true);
//		_EndLvlPanel.transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,_EndLvlPanel.transform.position.z);
		OTTween _endLvlSpriteAlphaTween = new OTTween(_EndLvlSprite.GetComponent<SpriteRenderer>() ,0.8f, OTEasing.ExpoOut).Tween("color", new Color(1f,1f,1f,1f));
		OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartOut).Tween("localScale", new Vector3( 1f, 1f, 1f )).OnFinish(displayEndLvlContent);	
		updateScore();
		lblLevel.text = _realID.ToString()+"."+chosenVariation.ToString();
		lblScoreGold.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"gold");
		lblScoreSilver.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"bronze");
		lblScoreBronze.text = _levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"silver");
		lblScoreOld.text = _playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"score");
		//player.nbKey;
		//_player._COEFF_BATTERY;
		//player._scorePlayer;
		//_timer.getScoreTime();
		StartCoroutine("incrementScore");
//		incrementScore();
		/*
		
		int numFrame = 0;
		if(player._scorePlayer >= System.Convert.ToInt32(lblScoreGold)) numFrame = 1;
		if(player._scorePlayer >= System.Convert.ToInt32(lblScoreSilver)) numFrame = 2;
		if(player._scorePlayer >= System.Convert.ToInt32(lblScoreBronze)) numFrame = 3;
		medal.anim.frameIndex = numFrame;
		OTTween medalTween = new OTTween(medalTween.transform ,1f, OTEasing.ExpoIn).Tween("localPosition", new Vector3(0f,0f,0f);
		*/
	}
	int displayPlayerMedal(string playerScore, string goldScore, string silverScore, string bronzeScore) {
		int numFrame = 0;
		if(playerScore=="") playerScore="0";if(goldScore=="") goldScore="0";if(silverScore=="") silverScore="0";if(bronzeScore=="") bronzeScore="0";
		if(System.Convert.ToInt32(playerScore) >= System.Convert.ToInt32(bronzeScore)) numFrame = 1;
		if(System.Convert.ToInt32(playerScore) >= System.Convert.ToInt32(silverScore)) numFrame = 2;
		if(System.Convert.ToInt32(playerScore) >= System.Convert.ToInt32(goldScore)) numFrame = 3;
		return numFrame;
	}
	IEnumerator incrementScore() {
		for(int i=0; i<=_timer.getScoreTime();i++) {
			if(i<_timer.getScoreTime()-10) i=i+3;
			yield return new WaitForSeconds(0.0001f);
			lblScoreTime.text = i.ToString()+" ''";
			lblScoreTotal.text = i.ToString();
			if(i==_timer.getScoreTime()) StartCoroutine("incrementBattery");
		}
	}
	IEnumerator incrementBattery() {
		for(cptKey=1; cptKey<=player.nbKey;cptKey++) {
			new OTTween(_collectedKeys[cptKey-1].transform, 0.2f, OTEasing.BackOut).Tween("localScale", new Vector3(1.2f, 1.2f, 1f)).OnFinish(popKeyScore);
			yield return new WaitForSeconds(1f);
			OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartOut).Tween("localScale", new Vector3( 1f, 1f, 1f )).OnFinish(displayEndLvlContent);
			lblNBKey.text = cptKey.ToString();
			lblScoreKey.text = "= "+ (cptKey * player._COEFF_BATTERY).ToString();
			lblScoreTotal.text = (_timer.getScoreTime()+cptKey * player._COEFF_BATTERY).ToString();
		}
	}
	private void popKeyScore (OTTween tween) {
		new OTTween(_collectedKeys[cptKey-1].transform, 1f, OTEasing.QuadInOut).Tween("position", new Vector3(lblNBKey.transform.position.x+1.25f, lblNBKey.transform.position.y+0.2f,_collectedKeys[cptKey-1].transform.position.z));
		new OTTween(_collectedKeys[cptKey-1].transform, 1f, OTEasing.QuadInOut).Tween("localScale", new Vector3(2.5f, 2.5f, 1f)).PingPong();
	}
	// Update is called once per frame
//	void Update () 
//	{
//		if (_secLeft <= 0)
//		{
////			endLevel();
////			loadLevel();
//		}
//
//		if (Input.GetKeyDown(KeyCode.Alpha3))
//		{
//			player.isDead = true;
//			GameEventManager.TriggerGameOver();
//		}
//	}
	private void displayEndLvlContent (OTTween tween) {
		_EndLvlContent.gameObject.SetActive(true);
	}
	private void hideEndLvlPanel (OTTween tween) {
		//_EndLvlContent.gameObject.SetActive(false); /****** ???? ******/
		_EndLvlPanel.gameObject.SetActive(false);
	}
	public void updateScore ()
	{
		if(this != null) {
		bestScore = (player._scorePlayer>System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"score")))?true:false;
//		print(player._scorePlayer.ToString() + " " + bestScore);
		if(chosenVariation<5 && !isBoss) { //Si occurence 1 à 4
//			print (scoreTotalLevel(_realID));
			if(bestScore) _playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"score",player._scorePlayer.ToString(), false);
//				print (scoreTotalLevel(_realID));
			//chosenVariation += 1; //On affiche l'occurence suivante
			
			if(bestScore) {
				_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString(),"score",scoreTotalLevel(_realID).ToString(), false);
				_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+(chosenVariation+1).ToString(),"locked","false", true);}
//			TranslateAllInScene();
//			GameEventManager.TriggerGameStart();
		}
		else {
			//chosenVariation = 0;
			if(isBoss) { //occurence Boss
					if(bestScore) {
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occBoss","score",player._scorePlayer.ToString(), false);
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString(),"score",scoreTotalLevel(_realID).ToString(), false);
					}
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+(_realID+1).ToString(),"locked","false", false);
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+(_realID+1).ToString()+"/occ1","locked","false", true);
			}
			else { //occurence 5
					if(bestScore) {
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"score",player._scorePlayer.ToString(), false);
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString(),"score",scoreTotalLevel(_realID).ToString(), false);
					}
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occBoss","locked","false", true);
			}
//			GameEventManager.TriggerNextLevel();
//			GameEventManager.NextInstance -= NextInstance;
//			DestroyImmediate(this.gameObject);
		}
		}
	}
	private void NextInstance() {
		if(this != null) {
			chosenVariation += 1; //On affiche l'occurence suivante
			if(chosenVariation<5 && !isBoss) {
				foreach (Transform _gameo in GameObject.Find("Level/ObjectImporter").transform)
				{
					if (_gameo.gameObject.name == chosenVariation.ToString() || _gameo.gameObject.name == "playerspawn"+chosenVariation)
					{
						_gameo.gameObject.SetActive(true);
					}
					else {
						_gameo.gameObject.SetActive(false);
					}
				}
				if(player.gameObject != null) {player.transform.position = player.spawnPos = GameObject.Find("playerspawn"+chosenVariation).transform.position;
				player.enabled=true;}
				TranslateAllInScene();
				GameEventManager.TriggerGameStart();
			}
			else {
				GameEventManager.TriggerNextLevel();
				GameEventManager.NextInstance -= NextInstance;
				DestroyImmediate(this.gameObject);	
			}
		}
	}
	private int scoreTotalLevel(int numLevel) {
		_scoreTotalLvl = System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel.ToString()+"/occ1","score"));
		_scoreTotalLvl += System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel.ToString()+"/occ2","score"));
		_scoreTotalLvl += System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel.ToString()+"/occ3","score"));
		_scoreTotalLvl += System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel.ToString()+"/occ4","score"));
		_scoreTotalLvl += System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel.ToString()+"/occ5","score"));
		_scoreTotalLvl += System.Convert.ToInt32(_playerDataLoader.getValueFromXmlDoc("BlobMinute/players/Bastien/level"+numLevel.ToString()+"/occBoss","score"));
		return _scoreTotalLvl;
	}
	private void NextLevel ()
	{		
		if(this != null) {
		Application.ExternalEval("document.cookie = \""+"Level"+(ID+1)+"Unlocked"+"=1; \"");
		Application.LoadLevel((ID+1));
		}
	}
	private void playLevelMusic () {
		switch(ID) {
		case 0:
			MasterAudio.PlaySound("intro");
			break;
		case 1:
			MasterAudio.PlaySound("tuto");
			break;
		case 2:
			playVariationMusic();
			break;
		case 3:
			MasterAudio.PlaySound("boss_theme");
			break;
		case 4:
			playVariationMusic();
			break;
		case 5:
			MasterAudio.PlaySound("boss_theme");
			break;
		case 6:
			playVariationMusic();
			break;
		case 7:
			MasterAudio.PlaySound("boss_theme");
			break;
		case 8:
			playVariationMusic();
			break;
		case 9:
			MasterAudio.PlaySound("boss_theme");
			break;
		case 10:
			playVariationMusic();
			break;
		case 11:
			MasterAudio.PlaySound("boss_theme");
			break;
		}
	}
	private void playVariationMusic() {
		switch(chosenVariation) {
			case 0:
				MasterAudio.PlaySound("level_theme_1");
				break;
			case 1:
				MasterAudio.PlaySound("level_theme_1");
				break;
			case 2:
				MasterAudio.PlaySound("level_theme_2");
				break;
			case 3:
				MasterAudio.PlaySound("level_theme_3");
				break;
			case 4:
				MasterAudio.PlaySound("level_theme_4");
				break;
			case 5:
				MasterAudio.PlaySound("level_theme_5");
				break;
		}
	}
	private void GameOver()
	{
		if(this != null && gameObject.activeInHierarchy) {
		//playerDies();
		MasterAudio.StopAllOfSound("bg");
		MasterAudio.StopAllOfSound("intro");
		//MasterAudio.StopAllOfSound("tuto");
		MasterAudio.StopAllOfSound("jam");
		MasterAudio.StopAllOfSound("level_theme_1");
		MasterAudio.StopAllOfSound("level_theme_2");
		MasterAudio.StopAllOfSound("level_theme_3");
		MasterAudio.StopAllOfSound("level_theme_4");
		MasterAudio.StopAllOfSound("level_theme_5");
		MasterAudio.StopAllOfSound("boss_theme");
		if(ID != 1) MasterAudio.StopAllPlaylists();
		}
	}
	private void GameStart()
	{
		if(this != null) {
			if(GameObject.Find("EndLVLPanel")!=null) _EndLvlPanel.transform.parent = GameObject.Find("Player/IngameUI").GetComponent<Transform>();
			if(_EndLvlSprite.transform.localScale.x != 10f) {
	//			_EndLvlPanel.transform.position = new Vector3(_player.transform.position.x,_player.transform.position.y,_EndLvlPanel.transform.position.z);
				_EndLvlContent.gameObject.SetActive(false);
			OTTween _endLvlSpriteAlphaTween = new OTTween(_EndLvlSprite.GetComponent<SpriteRenderer>() ,0.8f, OTEasing.ExpoIn).Tween("color", new Color(1f,1f,1f,0));
				OTTween _endLvlPanelTween = new OTTween(_EndLvlSprite.transform ,1f, OTEasing.QuartIn).Tween("localScale", new Vector3( 10f, 10f, 1f )).OnFinish(hideEndLvlPanel);
			}
			if(ID != 1) {
				MasterAudio.StopAllPlaylists();
				playLevelMusic();
			}
		}
	}
	private void GamePause()
	{
	
	}
	private void GameUnpause()
	{

	}
	
	public void TranslateAllInScene()
	{
		print ("translateScene");
		if(_pdata!=null) {
			_pdata.trans();
			_pdata.SETUP.TextSheet.SetupTranslation(_pdata.SETUP.ChosenLanguage);
			TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
			_pdata.SETUP.TextSheet.TranslateAll(ref allTxt);
		}
	}

	private void launchScene()
	{
//		Application.LoadLevel(1);
	}
	public void endLevel()
	{

	}
	
//	public void playerDies()
//	{
//		Instantiate(Resources.Load("Objects/Invasion"));
//		print ("die bitch");
//	}

	public void loadLevel(int _variation, int _lvlID)
	{
//		chosenVariation = _variation;
//		Application.LoadLevel(_lvlID);
//		_tileImporter.buildLevel(chosenVariation);
	}
}
