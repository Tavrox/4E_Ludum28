using UnityEngine;
using System.Collections;

public class RewindSprite : MonoBehaviour {

    private SpriteRenderer _sprite;
    private TextMesh _text;

	// Use this for initialization
	void Start () {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _sprite.color = new Color(1, 1, 1, 0);
        _text = gameObject.GetComponentInChildren<TextMesh>();
        _text.color = new Color(1, 1, 1, 0);
	}

    public void showRewindSprite()
    {
        if (_sprite != null && _text != null)
        {
            _sprite.color = new Color(1, 1, 1, 1);
            _text.color = new Color(1, 1, 1, 1);
            InvokeRepeating("twinkle", 0, 0.35f);
        }
    }
    public void hideRewindSprite()
    {
        if (_sprite != null && _text != null)
        {
            _sprite.color = new Color(1, 1, 1, 0);
            _text.color = new Color(1, 1, 1, 0);
            CancelInvoke("twinkle");
        }
    }
    private void twinkle()
    {
        if (_sprite.color.a == 0)
        {
            _sprite.color = new Color(1, 1, 1, 1);
            _text.color = new Color(1, 1, 1, 1);
        }
        else
        {
            _sprite.color = new Color(1, 1, 1, 0);
            _text.color = new Color(1, 1, 1, 0);
        }
    }
}
