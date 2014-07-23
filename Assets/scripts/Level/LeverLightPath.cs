﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
//using Linq;

public class LeverLightPath : MonoBehaviour {
	
	public List<Transform> path;
	private bool first;
	public float speed = 5, switchOFFSpeed=1f, duration=2f;
	
	private LineRenderer _myLine;
	private float lineWidth=0.2f, timedDelay, pulseSpeed;	
    public Color cBegin = Color.yellow, cReduced = Color.yellow, cTimed = Color.red, cEnd = Color.yellow,cDisplay;
    private int lengthOfLineRenderer;
	private OTTween _lightTransition;
	// Use this for initialization
	void Start () {
//		print ("hello");
		first = true;
		path = new List<Transform>();
		cEnd = new Color (cEnd.r, cEnd.g, cEnd.b, 0f);
		cDisplay = cEnd;
		foreach(Transform _WP in gameObject.GetComponentsInChildren<Transform>())
		{
			if(first) first=!first;
			else path.Add(_WP);	
		}
		//path = gameObject.GetComponentsInChildren<Transform>();
		//path = path.OrderBy(go=>go.name).ToList();
		if(gameObject.GetComponent<LineRenderer>()==null) _myLine = gameObject.AddComponent<LineRenderer>();
		path.Sort(CompareListByName);
		lengthOfLineRenderer = path.Count;
		//createLine();
	}
	
	// Update is called once per frame
	void Update () {
		updateLight();
		//#if UNITY_EDITOR
		//#endif
	}
	private static int CompareListByName(Transform i1, Transform i2)
	{
	    return i1.name.CompareTo(i2.name); 
	}
	private void updateLight() {
		_myLine = gameObject.GetComponent<LineRenderer>();
        _myLine.material = new Material(Shader.Find("Particles/Additive"));
        _myLine.SetColors(cDisplay, cDisplay);
		
		if(!Application.isPlaying) _myLine.SetColors(cReduced, cReduced);
       // _myLine.SetWidth(0.2F, 0.2F);
		_myLine.SetWidth(lineWidth,lineWidth);
        _myLine.SetVertexCount(lengthOfLineRenderer*2-1);
		
		int j = 0;
		int i = 0;
        while (i < lengthOfLineRenderer) {
            Vector3 pos = new Vector3(path[i].transform.position.x, path[i].transform.position.y, this.transform.position.z);
            _myLine.SetPosition(j, pos);
            i++;
			j++;
        }
		j--;
		while (i > 0) {
            i--;
		 	Vector3 pos = new Vector3(path[i].transform.position.x, path[i].transform.position.y, this.transform.position.z);
            _myLine.SetPosition(j, pos);
			j++;
		}
	}
	public void switchON() {
		StopCoroutine("lightOFFDelay");
		if(_lightTransition != null) _lightTransition.Stop();
		_lightTransition = new OTTween(this, .35f).Tween("cDisplay", cBegin).OnFinish(reduceBrightness);
		//InvokeRepeating("updateLight",0f, 0.05f);
		//cDisplay = cBegin;
//		updateLight();
	}
	public void switchOFF() {
		StartCoroutine("lightOFFDelay",duration);
	}
	IEnumerator lightOFFDelay(float delay) {
		if(gameObject.transform.parent.transform.GetComponent<Lever>().myButtonType.ToString()=="TimedBtn") {
			print ("timed");
//			yield return new WaitForSeconds(delay);
//			_lightTransition = new OTTween(this, switchOFFSpeed).Tween("cDisplay", cEnd).OnFinish(stopUpdateLight);
//			_lightTransition = new OTTween(this, gameObject.transform.parent.transform.GetComponent<Lever>().delay).Tween("cDisplay", cEnd).OnFinish(stopUpdateLight);
			StartCoroutine("timedPulsation",gameObject.transform.parent.transform.GetComponent<Lever>().delay);
		}
		else {
			yield return new WaitForSeconds(delay);
			_lightTransition = new OTTween(this, switchOFFSpeed).Tween("cDisplay", cEnd).OnFinish(stopUpdateLight);
		}
	}
	void reduceBrightness(OTTween _t) {
	    _t = new OTTween(this, .2f).Tween("cDisplay", cReduced).OnFinish(startSwitchOff);
	}
	void startSwitchOff(OTTween _t) {
	    StartCoroutine("lightOFFDelay",duration-(.2f+.35f));
	}
	void stopUpdateLight(OTTween _t) {
		CancelInvoke("updateLight");
		CancelInvoke("pulseColor");	
	}
	IEnumerator timedPulsation (float timedDelay) {
		yield return new WaitForSeconds(.4f); //calé sur le temps suite à la transition
		InvokeRepeating("pulseColor",0.1f, 1f);
		InvokeRepeating("pulseColor",0.2f, 1f);
		yield return new WaitForSeconds(timedDelay-timedDelay*0.3f); //attend 2/3
		CancelInvoke("pulseColor");
		InvokeRepeating("pulseColor",0.1f, .5f);
		InvokeRepeating("pulseColor",0.2f, .5f);
		yield return new WaitForSeconds(timedDelay*0.15f);
		CancelInvoke("pulseColor");
		InvokeRepeating("pulseColor",0.1f, .25f);
		InvokeRepeating("pulseColor",0.2f, .25f);
		yield return new WaitForSeconds(timedDelay*0.1f);
		CancelInvoke("pulseColor");
		_lightTransition = new OTTween(this, switchOFFSpeed).Tween("cDisplay", cEnd).OnFinish(stopUpdateLight);
	}
	void pulseColor() {
//		if(pulseSpeed>.5f)	_lightTransition = new OTTween(this, pulseSpeed).Tween("cDisplay", cTimed).PingPong();
//		else {
			if(cDisplay==cTimed) cDisplay = cReduced;
			else cDisplay = cTimed;
//		}
	}
}
