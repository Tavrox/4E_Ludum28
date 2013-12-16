﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	[SerializeField] private Player player;
	[SerializeField] private Camera myCamera;
	
	public int ID;
	public int nextLvlID;
	public int previousLvlID;

	private int chosenVariation;
	private int _secLeft;
	private TileImporter _tileImporter;

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
		MasterAudio.PlaySound("bg");
		MasterAudio.PlaySound("jam");

		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		DontDestroyOnLoad(GameObject.Find("Frameworks"));
		
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
	}

	public void loadLevel(int _variation, int _lvlID)
	{
//		chosenVariation = _variation;
//		Application.LoadLevel(_lvlID);
//		_tileImporter.buildLevel(chosenVariation);
	}
}
