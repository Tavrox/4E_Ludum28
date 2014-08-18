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
		Manager,
		NeedKey
	}
	public ListAction action;
	private OTSprite prefabSprite;
	private TextMesh _moreKeyText;
	
	void Start () 
	{
		prefabSprite = GetComponentInChildren<OTSprite>();
		if(GetComponentInChildren<TextMesh>()!=null) _moreKeyText = GetComponentInChildren<TextMesh>();
		
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameStart += GameStart;

		if (action == ListAction.Manager)
		{


		}
	}
//	void Update () {
//		if(action == ListAction.PauseGame) {
//			if (Input.GetKeyDown(InputMan.Pause))
//			{
//				triggerEscape();
//			}
//		}
//	}
	public void fadeOut()
	{
		if(prefabSprite.alpha == 0f) {
			OTTween _tween = new OTTween(prefabSprite, 1f).Tween("alpha", 1f).PingPong();
			OTTween _txtAlphaTween = new OTTween(_moreKeyText, 1f).Tween("color", new Color (_moreKeyText.color.r, _moreKeyText.color.g, _moreKeyText.color.b, 1f)).PingPong();
			StartCoroutine("hideItem");
		}
	}
	private IEnumerator hideItem() {
		yield return new WaitForSeconds(1f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		prefabSprite.alpha = 0f;
		_moreKeyText.color = new Color (_moreKeyText.color.r, _moreKeyText.color.g, _moreKeyText.color.b, 0f);
	}
	private void OnMouseDown()
	{
		if (action == ListAction.GoToMenu)
		{
			Application.LoadLevel(0);
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
		if(this != null) {
			gameObject.SetActive(false);
		}
	}
	void GameStart () {		
		if(this != null && action != ListAction.PauseGame) {
			gameObject.SetActive(true);
		}
	}
}
