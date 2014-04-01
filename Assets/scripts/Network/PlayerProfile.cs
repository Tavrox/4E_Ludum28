using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile : ScriptableObject {

	public string Player_Name;
	public string Twitter_Name;
	public string Twitter_Pin;
	public GameSetup.languageList Player_Language;
	public List<LevelInfo> ActivatedLevels = new List<LevelInfo>();

}
