using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelParam : ScriptableObject {

	[SerializeField] public int stepID = 1;
	[SerializeField] public int ID = 1;
	[SerializeField] public int occurence = 1;
	[SerializeField] public int goldScore = 15000;
	[SerializeField] public int silverScore = 10000;
	[SerializeField] public int bronzeScore = 5000;
	[SerializeField] public bool Locked = true;
	[SerializeField] public string UnlockCondition = "None";
}
