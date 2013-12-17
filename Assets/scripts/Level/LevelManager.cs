using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	[SerializeField] private Player player;
	[SerializeField] private Camera myCamera;
	
	public int ID;
	public int chosenVariation;

	private int _secLeft;
	private TileImporter _tileImporter;
	private PlayerData _pdata;

	// Use this for initialization
	void Start () 
	{
		chosenVariation = GameObject.Find("Level/TileImporter").GetComponent<TileImporter>().chosenVariation;
		if (GameObject.FindWithTag("Player") != null)
		{
			player = GameObject.FindWithTag("Player").GetComponent<Player>();
		}
		myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		_secLeft = GameObject.Find("Player/IngameUI/Timer").GetComponent<Timer>().secLeft;
		_tileImporter = GameObject.Find("Level/TileImporter").GetComponent<TileImporter>();
		_pdata = GameObject.Find("PlayerData").GetComponent<PlayerData>();
		Player _Player  = GameObject.Find("Player").GetComponent<Player>();

		MasterAudio.PlaySound("bg");
		MasterAudio.PlaySound("jam");

		string _rand = Random.Range(_tileImporter.minVariation, _tileImporter.maxVariation).ToString();
		print ("Level generated " + _rand);
		foreach (Transform _gameo in GameObject.Find("Level/Gameplay").transform)
		{
			if (_gameo.gameObject.name == _rand )
		    {
				_gameo.gameObject.SetActive(true);
			}
		}


		_Player.transform.position = GameObject.Find("playerspawn"+_rand).transform.position;

		_pdata.displayInput = false;

		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_secLeft <= 0)
		{
//			endLevel();
//			loadLevel();
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			playerDies();
		}
	}

	private void GameOver()
	{
		playerDies();
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
	
	public void playerDies()
	{
		Instantiate(Resources.Load("Objects/Invasion"));
		print ("die bitch");
	}

	public void loadLevel(int _variation, int _lvlID)
	{
//		chosenVariation = _variation;
//		Application.LoadLevel(_lvlID);
//		_tileImporter.buildLevel(chosenVariation);
	}
}
