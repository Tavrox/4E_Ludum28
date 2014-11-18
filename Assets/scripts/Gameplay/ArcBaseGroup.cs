using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArcBaseGroup : MonoBehaviour {
	
	public List<ArcElectric> arcs = new List<ArcElectric>();
	public List<BaseElectric> bases = new List<BaseElectric>();
	public int nbCrates = 0;
	private List<Crate> myTouchingCrates = new List<Crate>();
	private int lastEntered=0;
	private bool addingCrate;
	
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		collider.enabled = addingCrate = false;
		lastEntered=0;
		StartCoroutine("waitB4Restart");
	}
	void OnTriggerEnter(Collider _other)
	{//print (_other.tag);
		if (_other.CompareTag("Crate") && lastEntered!=_other.GetInstanceID())
		{
			addingCrate=true;
				print ("++ "+lastEntered+" + "+_other.GetInstanceID());
			foreach(Crate crt in myTouchingCrates) {
			int t = crt.GetInstanceID() + 2;
				if(crt.GetInstanceID() == _other.GetInstanceID() || t == _other.GetInstanceID()) {
					addingCrate=false;
				}
			}
			if(addingCrate || myTouchingCrates.Count==0) {
				myTouchingCrates.Add(_other.GetComponent<Crate>());
				//_other.name(_other.GetInstanceID());
				turnOFF();
				nbCrates++;
			}
				lastEntered = _other.GetInstanceID();
				StartCoroutine("resetLastEntered");
			addingCrate=true;
		}
	}
//	void OnTriggerStay(Collider _other) {//EDIT ARG 31/07?
//		if (_other.CompareTag("Crate"))
//		{
//			turnOFF();
//		}
//	}
	IEnumerator resetLastEntered() {
		yield return new WaitForSeconds(0.25f);
		lastEntered=0;
	}
	void OnTriggerExit(Collider _other)
	{
		if (_other.CompareTag("Crate"))
		{
			bool removeCrate = false;
			foreach(Crate crt in myTouchingCrates) {
				int t = crt.GetInstanceID() + 2;
				if(t == _other.GetInstanceID()) {
					removeCrate = true;
				}
			}
			if(removeCrate) {
					nbCrates--; myTouchingCrates.Remove(_other.GetComponent<Crate>());
			}
			if(nbCrates==0) turnON();
		}
	}
	public void turnOFF() {
			foreach(ArcElectric _arc in arcs) _arc.turnOFF();
			foreach(BaseElectric _base in bases) _base.turnOFF();
	}
	public void turnON() {
				foreach(ArcElectric _arc in arcs) _arc.turnON();
				foreach(BaseElectric _base in bases) _base.turnON();
	}
	void GameStart() {
		if(this != null && gameObject.activeInHierarchy) {
			//active = false;
			nbCrates = 0;
			myTouchingCrates = new List<Crate>();
			addingCrate = false;
			lastEntered=0;
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
		//yield return new WaitForSeconds(1f);EDIT ARG 31/07?
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
