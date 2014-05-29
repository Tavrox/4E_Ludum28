using UnityEngine;
using System.Collections;

[System.Serializable]
public class BrickStepParam : ScriptableObject
{
	[SerializeField] public int stepID = 1;
	[SerializeField] public int ID = 1;
	[SerializeField] private string _WaypointsAttributed = "A";
	[SerializeField] public string WaypointsAttributed
	{
		get { return _WaypointsAttributed; }
		set { _WaypointsAttributed = value; }
	}
	[SerializeField] public bool Enable = true;
	[SerializeField] public bool Disable = false;
	[SerializeField] public string Directions = "UDLR";
	[SerializeField] public int TowerLength = 4;
	[SerializeField] public bool Invert = false;
}
