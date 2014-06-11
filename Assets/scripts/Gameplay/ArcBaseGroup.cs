using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcBaseGroup : MonoBehaviour {
	
	public List<ArcElectric> arcs = new List<ArcElectric>();
	public List<BaseElectric> bases = new List<BaseElectric>();
	public int nbCrates = 0;
	
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		collider.enabled = false;
		StartCoroutine("waitB4Restart");
	}
	void OnTriggerEnter(Collider _other)
	{//print (_other.tag);
		if (_other.CompareTag("Crate"))
		{
			foreach(ArcElectric _arc in arcs) _arc.turnOFF();
			foreach(BaseElectric _base in bases) _base.turnOFF();
			nbCrates++;
		}
	}
	void OnTriggerExit(Collider _other)
	{
		if (_other.CompareTag("Crate"))
		{
			nbCrates--;
			if(nbCrates==0) {
				foreach(ArcElectric _arc in arcs) _arc.turnON();
				foreach(BaseElectric _base in bases) _base.turnON();
			}
		}
	}
	
	void GameStart() {
		if(this != null && gameObject.activeInHierarchy) {
			//active = false;
			StartCoroutine("waitB4Restart");
		}
	}
	void GameOver() {
		if(this != null && gameObject.activeInHierarchy) {
			//active = false;
			collider.enabled = false;
		}
	}
	IEnumerator waitB4Restart() {
		yield return new WaitForSeconds(1f);
		while (GameEventManager.gamePaused) 
		{
			yield return new WaitForFixedUpdate();	
		}
		collider.enabled = true;
		//active = true;
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
