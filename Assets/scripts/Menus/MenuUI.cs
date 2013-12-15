using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour {
	
	public enum ListMenu
	{
		Main,
		ChooseLevel,
		HighScores,
		EndLevel,
		Play,
		GoToCredits,
		Title,
		BackToMenuFromCredits,
		BackLevel,
		GoToMainMenu,
		None,
		BackToMenuFromLevelChooser
	};
	public ListMenu menu;
	private bool clickable = false;
	private OTSprite spr;
	private List<GameObject> menuObjects;
	
	// Use this for initialization
	void Start () {

		menuObjects = new List<GameObject>();

		spr = GetComponentInChildren<OTSprite>();

		switch (menu)	
		{
			case (ListMenu.Main):
			{
			
			break;
			}
			case (ListMenu.Play):
			{
			
			break;	
			}
			case (ListMenu.GoToMainMenu):
			{
			
			break;
			}
			case (ListMenu.GoToCredits):
			{
			
				break;
			}
		}
		InvokeRepeating("animateItem", 0f, 2.5f);
	
	}
	private void OnMouseDown()
	{
		menuObjects = new List<GameObject>();
		switch (menu)
		{
			case (ListMenu.Main):
			{
				
				break;
			}
			case (ListMenu.Play):
			{
				translateCamera(796.7685f);
				menuObjects.Add(findObject("Title"));
				menuObjects.Add(findObject("Play"));
				menuObjects.Add(findObject("Credits"));
				fadeOutObjects(menuObjects);
				menuObjects.Clear();
				break;
			}
			case (ListMenu.BackToMenuFromCredits):
			{
				
				menuObjects.Add(findObject("Title"));
				menuObjects.Add(findObject("Play"));
				menuObjects.Add(findObject("Credits"));
				fadeInObjects(menuObjects);
				menuObjects.Clear();
				break;
			}
			case (ListMenu.GoToCredits):
			{
				menuObjects.Add(findObject("Title"));
				menuObjects.Add(findObject("Play"));
				menuObjects.Add(findObject("Credits"));
				fadeOutObjects(menuObjects);
				menuObjects.Clear();
				translateCamera(-804.2063f);
				break;
			}
		}
	}
	
	private void triggerCredits()
	{

	}

	private GameObject findObject(string _str)
	{
		GameObject result = GameObject.Find("Menu/" + _str);
		return result;
	}

	private void fadeOutObjects(List<GameObject> _objects)
	{
		foreach (GameObject _obj in _objects)
		{
			if (_obj != null)
			{
				OTSprite objectSprite = _obj.GetComponentInChildren<OTSprite>();
				_obj.collider.enabled = false;
				OTTween _tween = new OTTween(objectSprite,1f).Tween("alpha",0f);	
				if (objectSprite.alpha == 0)
				{
					_obj.SetActive(false);
				}
			}
		}
	}

	private void fadeInObjects(List<GameObject> _objects)
	{
		foreach (GameObject _obj in _objects)
		{
			if (_obj != null)
			{
				OTSprite objectSprite = _obj.GetComponentInChildren<OTSprite>();
				OTTween _tween = new OTTween(objectSprite,1f).Tween("alpha",1f);	
				if (objectSprite.alpha == 1)
				{
					_obj.SetActive(true);
				}
				_obj.collider.enabled = true;
			}
		}
	}

	private void translateCamera(float posX)
	{
		Transform _cam = GameObject.Find("UI/Main Camera").transform.transform;
		OTTween _tween = new OTTween(_cam,2f).Tween("position", new Vector3(posX, _cam.position.y, _cam.position.z), OTEasing.StrongOut );
	}

	private void animateItem()
	{
		OTTween _tween = new OTTween(spr,2.5f)
		.Tween("size", new Vector2(spr.size.x - Random.Range(-30,30), spr.size.y - Random.Range(-30,30)) )
		.PingPong();
	}
	/*
	private void revealIntro(OTTween _tween)
	{
		OTTween _introOut = new OTTween(_intro, 2.5f).Tween("color", Color.white).Wait(2f);
		OTTween _titleOut = new OTTween(_title, 2.5f).Tween("alpha", 1f).Wait(2f);
		_titleOut.OnFinish(fadeEverything);
	}
	*/
}
