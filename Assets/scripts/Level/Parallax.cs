﻿using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
	
	[SerializeField] private Player player;	
	[Range (0,10)] 	public int scrollSpeed = 4;
	//private int scrollSpeed;
	private Vector3 scrollVector;
	
	[HideInInspector] public Transform thisTransform, parentTransform;
	private Vector3 posIni;
	private bool active;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		thisTransform = transform;
        parentTransform = transform.parent;
		posIni = gameObject.transform.localPosition;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.isDead && !player.isCrounch && !GameEventManager.gamePaused) {
			scrollVector.x = player.getVectorFixed().x; //Need vectorFixed to be public
			scrollVector.y = player.getVectorFixed().y;
			scrollVector.x = scrollVector.x/scrollSpeed;
			scrollVector.y = scrollVector.y/scrollSpeed;
			thisTransform.position += new Vector3(scrollVector.x,scrollVector.y,0f);
		}
	}
	private void GameStart()
	{
		if(this != null && gameObject.activeInHierarchy) {
            transform.localPosition = posIni;
		}
	}
	void GamePause()
	{
		
	}
	void GameUnpause()
	{
		
	}
}
