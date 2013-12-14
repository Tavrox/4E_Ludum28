using UnityEngine;
using System.Collections;

public class Walker : Enemy {
	
//	[HideInInspector] public Vector3 position;
//	[HideInInspector] public Transform trans;

	/***** ENNEMI BEGIN *****/

	// Update is called once per frame
	public void Update () 
	{
		if(chasingPlayer) {ChasePlayer();}
		detectPlayer();
		detectEndPlatform();
	}
}