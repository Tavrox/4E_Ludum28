using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public int posX;
	public int posY;
	public int secLeft = 60;
	public int microSecLeft = 99;
	//public GUISkin _skinTimer;
	public Color _colSafe;
	public Color _colorWarning;
	public Color _colCritical;
	public bool pauseTimer = false, lockTimerStart = false;

	public bool triggeredEnd = false;
	private Transform _alertMask;
	private Color alertColor;
	private Player _player;
	private Camera _camera;
	private Vector3 _HUDLevelsPosition;
	private TextMesh _txtTimer;


	// Use this for initialization
	void Start () {
		pauseTimer = true;
		lockTimerStart = true;
		_player = GameObject.Find("Player").GetComponent<Player>();
		InvokeRepeating("updateTimer", 0, 0.01f);
		_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		_txtTimer = gameObject.GetComponentInChildren<TextMesh>();
		_alertMask = GameObject.Find("Player/IngameUI/Timer/timerAlert").GetComponent<Transform>();
		alertColor = _alertMask.renderer.material.color;
		alertColor.a = 0f;
		_txtTimer.color = _colSafe;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.FinishLevel += FinishLevel;
//		if(FETool.Round(((float) Screen.width/(float) Screen.height),1)!=1.3) {
//			posX = posY = 30;
//		}
	}
	void FixedUpdate() {
		_txtTimer.gameObject.transform.position = _camera.ScreenToWorldPoint(new Vector3(Screen.width*0.1f-_txtTimer.text.Length*11.4f, Screen.height - (Screen.height*0.09f), _camera.nearClipPlane));
	}
	//transform.renderer.material.color.a
	private void updateTimer()
	{
		if (pauseTimer != true && !lockTimerStart)
		{
			microSecLeft -= 1;

			if (secLeft < 60)
			{
				//_skinTimer.label.normal.textColor = _colSafe;
				_txtTimer.color = _colSafe;
			}
			if (secLeft < 30)
			{
				//_skinTimer.label.normal.textColor = _colorWarning;
				_txtTimer.color = _colorWarning;
			}
			if (secLeft < 15)
			{
				//_skinTimer.label.normal.textColor = _colCritical;
				_txtTimer.color = _colCritical;
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
			_player._scorePlayer= System.Convert.ToInt32(System.Convert.ToDouble(secLeft.ToString()+"."+microSecLeft.ToString())*_player._COEFF_TEMPS + _player.nbKey*_player._COEFF_BATTERY);
			_txtTimer.text = ((secLeft<10)?"0"+secLeft.ToString():secLeft.ToString()) + " " + ((microSecLeft<10)?"0"+microSecLeft.ToString():microSecLeft.ToString());
			//_txtTimer.gameObject.transform.position = _camera.ScreenToWorldPoint(new Vector3(Screen.width*0.1f, Screen.height - (Screen.height*0.09f), _camera.nearClipPlane));
			//print(_player._scorePlayer);
		}
		else {
			if (_player.isLeft || _player.isRight || _player.isJump || _player.isCrounch)
			lockTimerStart = pauseTimer = false;
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
			lockTimerStart = pauseTimer = true;
			triggeredEnd = false;
			resetTimer();
			_txtTimer.text = "59 99";
			_txtTimer.color = _colSafe;
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
			lockTimerStart = pauseTimer = true;
		}
	}

//	private void OnGUI()
//	{
		//GUI.skin = _skinTimer;
		//gameObject.transform.position = _camera.ScreenToWorldPoint(new Vector3(Screen.width*0.1f, Screen.height - (Screen.height*0.09f), _camera.nearClipPlane));
		
		//GUI.Label(new Rect(posX, posY, 400, 400), secLeft.ToString() + " " + microSecLeft.ToString());
//	}
}
