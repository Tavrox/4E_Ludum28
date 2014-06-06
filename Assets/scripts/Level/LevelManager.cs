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

	private int _secLeft;
	private TileImporter _tileImporter;
	private PlayerData _pdata;
	private string _rand;
	private Vector3 spawnPoint;
	private PartyData _partyData;
	public GameSaveLoad _playerDataLoader;

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
		//DontDestroyOnLoad(this);
		if(chosenVariation==0 /*|| chosenVariation==5*/) chosenVariation = 1;//GameObject.Find("Level/TileImporter").GetComponent<TileImporter>().chosenVariation;
//		if (GameObject.FindWithTag("Player") != null)
		//		{
			player = GameObject.FindWithTag("Player").GetComponent<Player>();
//		}
		myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		_secLeft = GameObject.Find("Player/IngameUI/Timer").GetComponent<Timer>().secLeft;
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
		
		playLevelMusic();
		_playerDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
		_playerDataLoader.LoadXMLToList("blob_minute-players");
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
	
	private void NextInstance ()
	{
		if(this != null) {
		if(chosenVariation<5 && !isBoss) { //Si occurence 1 à 4
			_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"score","123456", false);
			chosenVariation += 1; //On affiche l'occurence suivante
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
			_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"locked","false", true);
			TranslateAllInScene();
			GameEventManager.TriggerGameStart();
		}
		else {
			//chosenVariation = 0;
			if(isBoss) {
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occBoss","score","123456", false);
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+(_realID+1).ToString(),"locked","false", false);
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+(_realID+1).ToString()+"/occ1","locked","false");
			}
			else {
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occ"+chosenVariation.ToString(),"score","123456", false);
					_playerDataLoader.setValueInXmlDoc("BlobMinute/players/Bastien/level"+_realID.ToString()+"/occBoss","locked","false");
			}
			GameEventManager.TriggerNextLevel();
			GameEventManager.NextInstance -= NextInstance;
			DestroyImmediate(this.gameObject);
		}
		}
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
		_pdata.trans();
		_pdata.SETUP.TextSheet.SetupTranslation(_pdata.SETUP.ChosenLanguage);
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		_pdata.SETUP.TextSheet.TranslateAll(ref allTxt);
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
