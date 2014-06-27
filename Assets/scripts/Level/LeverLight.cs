using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeverLight : MonoBehaviour {
	
	private float speed;
	public LeverLightPath _myPath;
	[HideInInspector] public float initSpeed;
	[HideInInspector] public Vector3 direction;
	[HideInInspector] public Vector3 target;
	[HideInInspector] public Vector3 pos;
	[HideInInspector] public Vector3 initPos;
	[HideInInspector] public Player _player;
	private int currentWaypoint;
	private bool finished;
	
	// Use this for initialization
	void Start () {
		initSpeed = speed;
		finished=true;
		//initLight();
	}
	public void initLight() {
		currentWaypoint = 0;
		speed = _myPath.speed;
		gameObject.transform.position =_myPath.path[currentWaypoint].position;
		initPos = gameObject.transform.position;
		finished = false;
		setNextTarget();
	}
	// Update is called once per frame
	void Update () {
		if(!GameEventManager.gamePaused && !finished) { 
			pos = gameObject.transform.position;
			direction = Vector3.Normalize(target - pos);
			if(Vector3.Distance(pos,target)<0.15f) {
				if(currentWaypoint == _myPath.path.Count-1) {
					finished=true;
				}
				else setNextTarget();
			}
			else gameObject.transform.position += new Vector3 ( (speed * direction.x) * Time.deltaTime, (speed * direction.y) * Time.deltaTime, 0f);	
		}
	}
	void setNextTarget() {
		currentWaypoint++;
		target = _myPath.path[currentWaypoint].position;
	}
}
