using UnityEngine;
using System.Collections;

public class IngameUI : MonoBehaviour {

	public enum ListAction
	{
		PauseGame,
		ResumeGame,
		MuteSound,
		ExplosionTimer,
		GoToMenu,
		Manager
	}
	public ListAction action;
	private OTSprite prefabSprite;
	
	void Start () 
	{
		prefabSprite = GetComponentInChildren<OTSprite>();
		
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;

		if (action == ListAction.Manager)
		{


		}
	}

	public void fadeOut()
	{
		OTTween _tween = new OTTween(prefabSprite, 1f).Tween("alpha", 1f).PingPong();
	}

	void OnMouseDown()
	{
	
		switch (action)
		{
		case (ListAction.GoToMenu):
			{
				Application.LoadLevel(0);
				break;
			}

		}
	}
	private void GamePause()
	{
		
	}
	private void GameUnpause()
	{

	}
	private void GameOver()
	{


	}
}
