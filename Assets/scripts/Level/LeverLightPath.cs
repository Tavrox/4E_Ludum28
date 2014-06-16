using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverLightPath : MonoBehaviour {
	
	public List<Transform> path;
	private bool first;
	// Use this for initialization
	void Start () {
//		print ("hello");
		first = true;
		path = new List<Transform>();
		foreach(Transform _WP in gameObject.GetComponentsInChildren<Transform>())
		{
			if(first) first=!first;
			else path.Add(_WP);	
		}
		//path = gameObject.GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
