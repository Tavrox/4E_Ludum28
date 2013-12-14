using UnityEngine;
using System.Collections;

public class LvManager : MonoBehaviour {

	public enum turnEnum
	{
		Player,
		Enemy
	}
	public turnEnum turn;

	// Use this for initialization
	void Start () {

		turn = turnEnum.Player;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
