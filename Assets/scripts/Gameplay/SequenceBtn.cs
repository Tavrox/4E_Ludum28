using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceBtn : MonoBehaviour {
	
	public List<Lever> seqBtns = new List<Lever>();
	private bool rightCombo = true, allTriggered = true, solved, rightComboChecked, errorDetected,errorLaunched;
	private int cptBtn;
	private Player _player;
	// Use this for initialization
	void Start () {
		//StartCoroutine("myUpdate");
		GameEventManager.GameStart += GameStart;
		_player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!errorDetected) checkRightCombo();
		if(!solved) checkAllBtnTriggered();
	}

	private void checkRightCombo() {
		cptBtn = 0;
		rightCombo = true;
		foreach (Lever btn in seqBtns) {
			if(btn.trigged && cptBtn > 0) {
				if(seqBtns[cptBtn-1].trigged == false) {
					rightCombo = false;errorDetected = true;
					if(errorDetected == true) {
						foreach (Lever btn2 in seqBtns) {
							btn2.StartCoroutine("resetLever");
						}
						if(!errorLaunched) StartCoroutine("resetRightCombo");
					}
				}
			}
			cptBtn++;
			if(cptBtn == seqBtns.Count) {
				cptBtn --;
			}
		}
	}
	private void checkAllBtnTriggered() {
		allTriggered = true;
		foreach (Lever btn in seqBtns) {
			if(btn.seqLocked == false) {
				allTriggered = false;
				}
			}
		if(allTriggered) {
			if(errorDetected==false) {
				seqBtns[1].triggerLever();
				solved = true;
				MasterAudio.PlaySound ("sequence_succeed");
				StartCoroutine("resetSequence");
			}
			else if(errorDetected == true) {
				foreach (Lever btn in seqBtns) {
					btn.StartCoroutine("resetLever");
				}
				if(!errorLaunched) StartCoroutine("resetRightCombo");
			}
		}
	}
	private IEnumerator resetSequence() {
		foreach (Lever btn in seqBtns) {
			btn.StartCoroutine("resetLever");//btn.triggerLever();
		}
		yield return new WaitForSeconds(1f);
		rightCombo = allTriggered = true;
		solved = rightComboChecked = errorDetected = false;
	}
	private IEnumerator resetRightCombo () {
		errorLaunched = true;
		yield return new WaitForSeconds(1.2f);
		MasterAudio.PlaySound("sequence_fail");
		errorDetected = false;
		errorLaunched = false;

	}
	void GameStart () {
		if(this != null && gameObject.activeInHierarchy) {
			StopCoroutine("resetRightCombo");
			StopCoroutine("resetSequence");
			rightCombo = allTriggered = true;
			solved = rightComboChecked = errorDetected = errorLaunched = false;
		}
	}
}
