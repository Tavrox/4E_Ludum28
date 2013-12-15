using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {


	public Projectile prefabProj;
	public int shootFrequency;
	private Projectile instProj;
	
	public enum shootDir { Right, Left, Up, Down }
	public shootDir myShootDir;

	// Use this for initialization
	void Start () {
		StartCoroutine("shoot");
	}
	private IEnumerator shoot () {
		yield return new WaitForSeconds(shootFrequency);
		instProj = Instantiate (prefabProj) as Projectile;
		switch (myShootDir) {
			case shootDir.Down:
				instProj.direction = Vector3.down;
				break;
			case shootDir.Up:
				instProj.direction = Vector3.up;
				break;
			case shootDir.Left:
				instProj.direction = Vector3.left;
				break;
			case shootDir.Right:
				instProj.direction = Vector3.right;
				break;
		}

		instProj.transform.position = gameObject.transform.position;
		StartCoroutine("shoot");
	}
	// Update is called once per frame
	void Update () {
	
	}
}
