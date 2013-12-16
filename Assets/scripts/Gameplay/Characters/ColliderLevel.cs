using UnityEngine;
using System.Collections;

public class ColliderLevel : MonoBehaviour {

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetType() == typeof(Projectile))
		{
			print ("fock destroy");
		}
		print ("nazzee");
	}
}
