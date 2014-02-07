using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {

	public List<int> _levelUnlocked = new List<int>();
	private string _PlayerName = "";
	public bool displayInput = true;

	// Use this for initialization
	void Start ()
	{
		//DontDestroyOnLoad(gameObject);
	}
	
	public bool addLevelUnlocked(int _id)
	{
		_levelUnlocked.Add(_id);
		return true;
	}

	void OnGUI()
	{
		if (displayInput == true)
		{
//			_PlayerName = GUI.TextField(new Rect(10,10,200,20), _PlayerName, 25);
		}
	}
}
