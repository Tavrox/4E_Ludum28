using UnityEngine;
using System.Collections;

public class EndLvlButton : MonoBehaviour {
	
	public enum buttonList
	{
		None,
		SkipEndLvl,
		RestartLvl
	};
	public buttonList buttonType;
	public OTAnimatingSprite _animBtn;
	private GameObject _EndLvlPanel,_EndLvlContent;
	private TextMesh txtButton;
	private Player _player;
	private Transform pauseDefault, pauseMainMenu, pauseExit, pauseOption;
	public string STATE="Default";
	public bool first=true,trigged=false;
	
	// Use this for initialization
	void Start () {
		
		_player=GameObject.Find("Player").GetComponent<Player>();
		txtButton = GetComponentInChildren<TextMesh>();
		_animBtn = GetComponentInChildren<OTAnimatingSprite>();
		_animBtn.PlayLoop("static");
		_animBtn.speed = .8f;
		_EndLvlPanel = GameObject.Find("Player/IngameUI/EndLVLPanel").gameObject;
		_EndLvlContent = GameObject.Find("Player/IngameUI/EndLVLPanel/content").gameObject;
		trigged=false;
//		GameEventManager.GameStart += GameStart;
	}
	
	void OnMouseExit()
	{
		txtButton.color = new Color(1f,1f,1f,1f);
	}
	void OnMouseEnter()
	{
		txtButton.color = new Color(0f,0.4f,0.75f,1f);	
	}
	void OnMouseDown()
	{
		if(!trigged) {
		trigged=true;
		txtButton.color = new Color(1f,1f,1f,1f);
		//print(buttonType.ToString());
		switch (buttonType)
		{
			case buttonList.None :
			{
				print ("this button does nothing bro" + gameObject.name);
				break;
			}
			case buttonList.SkipEndLvl :
			{
				//OTTween _endLvlPanelTween = new OTTween(transform.parent.GetComponentInChildren<SpriteRenderer>().gameObject.transform ,0.5f, OTEasing.BounceOut).Tween("localScale", new Vector3( 10f, 10f, 1f ));
				//_EndLvlContent.gameObject.SetActive(false);	
				GameEventManager.TriggerNextInstance();
				break;
			}
			case buttonList.RestartLvl :
			{
				_EndLvlPanel.transform.parent = GameObject.Find("Level").GetComponent<Transform>();
				_player.resetLevel();
				break;
			}
		}
//			StartCoroutine ("retriggBtn");
		}
	}
	private IEnumerator hideEndLvlContent () {
		yield return new WaitForSeconds(0.1f);
		_EndLvlContent.gameObject.SetActive(false);
	}
//	private IEnumerator retriggBtn () {
	void OnEnable() {
//		yield return new WaitForSeconds(0.1f);
		//if(this != null && gameObject.activeInHierarchy) {
			trigged=false;
		//}
	}
}
