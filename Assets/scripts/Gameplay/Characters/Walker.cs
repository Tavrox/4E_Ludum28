using UnityEngine;
using System.Collections;

public class Walker : Enemy {
	
//	[HideInInspector] public Vector3 position;
//	[HideInInspector] public Transform trans;

	/***** ENNEMI BEGIN *****/

	void Start()
	{
		InvokeRepeating("sound",0,0.5f);
	}

	// Update is called once per frame
	public void Update () 
	{
		if(chasingPlayer) {ChasePlayer();}
		detectPlayer();
		detectEndPlatform();
	}

	private void sound()
	{
		MasterAudio.PlaySound("blob_run1");
	}
}