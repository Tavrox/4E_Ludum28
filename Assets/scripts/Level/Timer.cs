using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public int posX;
	public int posY;
	public float secLeft = 30f;
	public GUISkin _skinTimer;


	// Use this for initialization
	void Start () {
		InvokeRepeating("updateTimer",0,1f);
	}
	
	private void updateTimer()
	{
		secLeft -= 1;
	}

	private void OnGUI()
	{
		GUI.skin = _skinTimer;
		GUI.Label(new Rect(posX, posY, 400, 400), secLeft.ToString() + "sec");
	}
}
