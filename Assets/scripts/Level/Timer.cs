using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public int posX;
	public int posY;
	public int secLeft = 60;
	public int microSecLeft = 99;
	public GUISkin _skinTimer;
	public Color _colSafe;
	public Color _colorWarning;
	public Color _colCritical;


	// Use this for initialization
	void Start () {
		InvokeRepeating("updateTimer",0,0.01f);
	}
	
	private void updateTimer()
	{
		microSecLeft -= 1;

		if (secLeft < 60)
		{
			_skinTimer.label.normal.textColor = _colSafe;
		}
		if (secLeft < 30)
		{
			_skinTimer.label.normal.textColor = _colorWarning;
		}
		if (secLeft < 15)
		{
			_skinTimer.label.normal.textColor = _colCritical;
		}

		if (microSecLeft == 0)
		{
			secLeft -= 1 ;
			microSecLeft = 59;
		}
	}

	private void OnGUI()
	{
		GUI.skin = _skinTimer;
		GUI.Label(new Rect(posX, posY, 400, 400), secLeft.ToString() + " " + microSecLeft.ToString());
	}
}
