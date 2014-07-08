using UnityEngine;
using System.Collections;

public class MoveObjectMenu : MonoBehaviour {

	[Range (-10,10)] 	public float speedX = 0,speedY=0;
	private Vector3 scrollVector;
	
	[HideInInspector] public Transform thisTransform;
	private Vector3 posIni;
	public Transform loopBackground;
	
	// Use this for initialization
	void Start () {
		thisTransform = transform;
		posIni = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		scrollVector.x = speedX*0.01f; //Need vectorFixed to be public
		scrollVector.y = speedY*0.01f;
		thisTransform.position += new Vector3(scrollVector.x,scrollVector.y,0f);
		if(thisTransform.position.x < loopBackground.position.x) thisTransform.position = new Vector3(loopBackground.position.x+59.5f,thisTransform.position.y,thisTransform.position.z);
	}
}