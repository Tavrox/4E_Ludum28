using UnityEngine;
using System.Collections;

public class IngameUI : MonoBehaviour {

	public enum ListAction
	{
		PauseGame,
		ResumeGame,
		MuteSound,
		ExplosionTimer,
		GoToMenu
	}
	public ListAction action;
	private OTSprite prefabSprite;
	public bool paused;
	
	void Start () 
	{
		prefabSprite = GetComponentInChildren<OTSprite>();
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
}
