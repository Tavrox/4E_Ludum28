using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {
	
	[SerializeField] private Player player;	
	[Range (-10,10)] 	public float speedX = 0,speedY=0;
	private Vector3 scrollVector;
	
	[HideInInspector] public Transform thisTransform;
	private Vector3 posIni;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		thisTransform = transform;
        posIni = gameObject.transform.localPosition;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.isDead && !GameEventManager.gamePaused) {
			scrollVector.x = speedX*0.01f; //Need vectorFixed to be public
			scrollVector.y = speedY*0.01f;
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
