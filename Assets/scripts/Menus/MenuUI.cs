using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {
	
	public enum ListMenu
	{
		Main,
		ChooseLevel,
		HighScores,
		EndLevel
	}
	public ListMenu menu;
	public static bool exists;
	
	// Use this for initialization
	void Start () {
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		
		exists = true;
	
	}
	private void OnMouseDown()
	{
		switch (menu)
		{

		}
	}
	private void GameStart () 
	{
		
	}
	private void GameOver () 
	{
		
	}
	
	private void GamePause()
	{
		if (this == null)
		{
			switch (menu)
			{

			}
		}
		
	}
	private void GameUnpause()
	{
		switch (menu)
		{

		}
	}
	
	private void GameDialog()
	{

	}
}
