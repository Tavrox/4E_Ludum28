using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

	[SerializeField] private Player player;
	[SerializeField] private Camera myCamera;
	
	public int ID;
	private int chosenVariation;

	private int _secLeft;
	private TileImporter _tileImporter;
	private PlayerData _pdata;
	private string _rand;
	private Vector3 spawnPoint;

	// Use this for initialization
	void Start () 
	{
		chosenVariation = 1;//GameObject.Find("Level/TileImporter").GetComponent<TileImporter>().chosenVariation;
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

		//MasterAudio.PlaySound("bg");
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

		spawnPoint = GameObject.Find("playerspawn"+chosenVariation).transform.position;
		player.transform.position = player.spawnPos = new Vector3(spawnPoint.x,spawnPoint.y,-1f);
//		_pdata.displayInput = false;

		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.NextInstance += NextInstance;
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
		if(chosenVariation<5) {
			chosenVariation += 1;
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
			player.transform.position = player.spawnPos = GameObject.Find("playerspawn"+chosenVariation).transform.position;
			GameEventManager.TriggerGameStart();
		}
		else GameEventManager.TriggerNextLevel();
	}
	private void NextLevel ()
	{
		Application.ExternalEval("document.cookie = \""+"Level"+ID+"Unlocked"+"=1; \"");
		Application.LoadLevel(""+(ID+1));
	}
	private void GameOver()
	{
		//playerDies();
	}
	private void GameStart()
	{
	}
	private void GamePause()
	{
	
	}
	private void GameUnpause()
	{

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
