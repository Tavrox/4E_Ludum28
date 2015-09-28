using UnityEngine;
using System.Collections;

public class ChangeLanguage : MonoBehaviour {

    private PlayerData _pdata;
    private SpriteRenderer _sprite;
    private ChangeLanguage[] _allFlags;
    public bool active;
    public GameSetup.languageList _lang;

	// Use this for initialization
	void Start () {
        _pdata = GameObject.FindObjectOfType<PlayerData>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _allFlags = GameObject.FindObjectsOfType<ChangeLanguage>();
        if (active)
        {
            _sprite.color = new Color(1, 1, 1, 1);
        }
        else
        {
            _sprite.color = new Color(1, 1, 1, 0.25f);
        }
	}

    void OnMouseExit()
	{
        //txtButton.color = new Color(1f,1f,1f,1f);
        if(active) _sprite.color = new Color(1, 1, 1, 1);
        else _sprite.color = new Color(1, 1, 1, 0.25f);
	}
	void OnMouseEnter()
	{
        //txtButton.color = new Color(0f,0.4f,0.75f,1f);	
        _sprite.color = new Color(1, 1, 1, 0.5f);
	}
    void OnMouseDown()
    {
        for (int i = 0; i < _allFlags.Length; i++)
        {
            _allFlags[i].active = false;
            _allFlags[i]._sprite.color = new Color(1, 1, 1, 0.25f);
        }
        this.active = true;
        this._sprite.color = new Color(1, 1, 1, 1);
        if (_lang == GameSetup.languageList.english)
        {
            _pdata.ToEnglish();
        }
        else if (_lang == GameSetup.languageList.french)
        {
            _pdata.ToFrench();
        }
    }
}
