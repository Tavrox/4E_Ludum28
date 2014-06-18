using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Linq;

public class LeverLightPath : MonoBehaviour {
	
	public List<Transform> path;
	private bool first;
	public float speed = 5;
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
		//path = path.OrderBy(go=>go.name).ToList();
		path.Sort(CompareListByName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private static int CompareListByName(Transform i1, Transform i2)
	{
	    return i1.name.CompareTo(i2.name); 
	}
}
