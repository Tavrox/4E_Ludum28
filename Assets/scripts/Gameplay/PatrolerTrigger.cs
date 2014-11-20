using UnityEngine;
using System.Collections;

public class PatrolerTrigger : MonoBehaviour {

	public Patroler _patroler;
	public float initSpeed, triggSpeed;
	private bool trigged;
	private Player _player;
	// Use this for initialization
	void Start () {
		//_patroler = gameObject.GetComponent<Walker>();
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(trigged) _patroler.myCORRECTSPEED = triggSpeed;
	}
	void OnTriggerEnter(Collider _other)
	{
		if (_other.CompareTag("Player") == true && _player.isDead == false)
		{
			trigged = true;
		}
	}
	
	protected void GameStart () {
		//if(FindObjectOfType(typeof(Enemy)) && this != null) {
		if(this != null && gameObject.activeInHierarchy) {
			trigged = false;
			_patroler.myCORRECTSPEED = initSpeed;
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
