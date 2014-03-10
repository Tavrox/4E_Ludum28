using UnityEngine;
using System.Collections;
using UnityEditor;

public class FEAssetCreator {

	[MenuItem("Assets/Create/BMTuning")]
	public static void CreateBMTuning ()
	{
		ScriptableObjectUtility.CreateAsset<BMTuning>();
	}
	[MenuItem("Assets/Create/InputManager")]
	public static void CreateInputManager ()
	{
		ScriptableObjectUtility.CreateAsset<InputManager>();
	}
	[MenuItem("Assets/Create/PlayerProfile")]
	public static void CreatePlayerProfile ()
	{
		ScriptableObjectUtility.CreateAsset<PlayerProfile>();
	}
}
