using UnityEngine;
using System.Collections;

public class ButtonLaunch : MonoBehaviour {
	
	public PlayerData _playerData;
	public string _lvlToLoad;
	public int numOccurence; 
	private int _mynumOccurence;
	private OTSprite _spriteBtn;
	// Use this for initialization
	void Awake () {
		if(GameObject.Find("PlayerData") != null) _playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
		if (GetComponentInChildren<OTSprite>() != null)
		{
			_spriteBtn = GetComponentInChildren<OTSprite>();
			_spriteBtn.enabled = true;
		}
		_mynumOccurence = numOccurence;
		setOccurenceFrame();
	}
	
	// Update is called once per frame
	void Update () {
		setOccurenceFrame();
			_spriteBtn.enabled = true;
	}
	public void setOccurenceFrame() {
		/*if(locked) _spriteBtn.frameIndex = numOccurence-1;
		else*/ _spriteBtn.frameIndex = _mynumOccurence+6;
	}
	void OnMouseDown()
	{
//		PlayerData _playerData = Instantiate(Resources.Load("Presets/PlayerData")) as PlayerData;
		_playerData.choixOccurence = System.Convert.ToInt32(_mynumOccurence);
		if(numOccurence==6) _playerData.choixOccurence = 1;//_lvlToLoad = System.Convert.ToString(System.Convert.ToInt32(numOccurence)+1);
		if(numOccurence==7) _playerData.choixOccurence = 1;//_lvlToLoad = System.Convert.ToString(System.Convert.ToInt32(numOccurence)+1);//_lvlToLoad = System.Convert.ToInt32(_lvlToLoad+?); NUM SCENE OCC 6
		print(_lvlToLoad + " - " + _mynumOccurence);
		
		
		Application.LoadLevel(System.Convert.ToInt32(_lvlToLoad));
	}
	void OnMouseExit()
	{
		_spriteBtn.size -= new Vector2(0.3f,0.3f);
	}
	
	void OnMouseEnter()
	{
		print(_lvlToLoad + " - " + _mynumOccurence);
		_spriteBtn.size += new Vector2(0.3f,0.3f);
	}
}
