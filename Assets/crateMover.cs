using UnityEngine;
using System.Collections;

public class crateMover : MonoBehaviour {


	public Crate myCrate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) 
	{
		if(other.gameObject.tag=="Player") {
			//		if(other.gameObject.name=="Crate" || other.gameObject.name=="Crate(Clone)") {
			//			/*if(detectPlayer())*/ other.gameObject.GetComponent<Crate>().transform.position += new Vector3(crateMove*1.5f/*+0.1f*/,0f,0f);
			//		}
			if(!myCrate.blockCrate) {
				myCrate._player.moveVel = 0.2f*myCrate.playerMoveVel;
				if(myCrate._player.isRight) myCrate.crateMove = myCrate._player.moveVel*Time.deltaTime*2f;
				else if(myCrate._player.isLeft) myCrate.crateMove = -myCrate._player.moveVel*Time.deltaTime*2f;
				transform.position += new Vector3(myCrate.crateMove,0f,0f);
				//return true;
			}}
		if(other.gameObject.name=="ColliBox" || other.gameObject.name=="Door") 
		{/*print("GAUUUUUUCHE");*/myCrate.blockCrate = true;}
		
		//		if (other.gameObject.tag == "ColliBox") {
		//
		//		}
		//		if(other.gameObject.CompareTag("Player")) {
		//
		//				if(_player.isRight) crateMove = _player.moveVel*Time.deltaTime;
		//				else crateMove = -_player.moveVel*Time.deltaTime;
		//				thisTransform.position += new Vector3(crateMove,0f,0f);
		//
		//			//}
		//		}
		////		if(other.gameObject.layer == 8)
		////		{
		////			grounded = true;
		////		}
		//////		if(other.gameObject.CompareTag("Ground")) 
		//////		{
		//////			grounded = true;
		//////		}
		//////		if(other.gameObject.CompareTag("Platforms")) 
		//////		{
		//////			grounded = true;
		//////		}
	}
}
