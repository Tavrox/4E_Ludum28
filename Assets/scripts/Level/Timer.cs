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
	public bool pauseTimer = false;

	public bool triggeredEnd = false;
	private Transform _alertMask;
	private Color alertColor;
	private Player _player;


	// Use this for initialization
	void Start () {
		pauseTimer = true;
		_player = GameObject.Find("Player").GetComponent<Player>();
		InvokeRepeating("updateTimer", 0, 0.01f);
		_alertMask = GameObject.Find("Player/IngameUI/Timer/timerAlert").GetComponent<Transform>();
		alertColor = _alertMask.renderer.material.color;
		alertColor.a = 0f;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;
//		if(FETool.Round(((float) Screen.width/(float) Screen.height),1)!=1.3) {
//			posX = posY = 30;
//		}
	}
	//transform.renderer.material.color.a
	private void updateTimer()
	{
		if (pauseTimer != true)
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
			if (secLeft < 5)
			{
				InvokeRepeating("timerAlert",0,0.2f);
			}

			if (microSecLeft == 0)
			{
				secLeft -= 1 ;
				microSecLeft = 99;
			}

			if (secLeft == 0 && microSecLeft == 1 && triggeredEnd == false)
			{
				CancelInvoke();
				GameEventManager.TriggerGameOver();
				triggeredEnd = true;
				secLeft = 59;
			}
		}
		else {
			if (_player.isLeft || _player.isRight || _player.isJump || _player.isCrounch)
			pauseTimer = false;
		}
	}
	private void timerAlert () {
		//alertColor.a = Mathf.PingPong(Time.time, 0.5f);
		alertColor.a = (Mathf.Sin(Time.time*6.25f)+0.5f ) * 0.2f;
		_alertMask.renderer.material.color = alertColor;
	}
	public void resetTimer()
	{
			secLeft = 59;
			microSecLeft = 99;
	}

	private void GameStart()
	{
		if(this != null) {
			CancelInvoke();
			pauseTimer = true;
			triggeredEnd = false;
			resetTimer();
			alertColor.a = 0f;_alertMask.renderer.material.color = alertColor;
			InvokeRepeating("updateTimer", 0, 0.01f);
		}
	}
	private void FinishLevel() {
		if(this != null && gameObject.activeInHierarchy) {
			CancelInvoke();
			alertColor.a = 0f;_alertMask.renderer.material.color = alertColor;
		}
	}
	private void GamePause()
	{
		pauseTimer = true;
	}
	private void GameUnpause()
	{
		pauseTimer = false;
	}
	
	private void GameOver()
	{
		if(this != null && gameObject.activeInHierarchy) {
			pauseTimer = true;
		}
	}

	private void OnGUI()
	{
		GUI.skin = _skinTimer;
		GUI.Label(new Rect(posX, posY, 400, 400), secLeft.ToString() + " " + microSecLeft.ToString());
	}
}
