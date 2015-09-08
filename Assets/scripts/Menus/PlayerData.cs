using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public PlayerProfile PROFILE;
	public GameSetup SETUP;
	public float GlobalVolume = 1f;
	public bool globalVolMuted = false;
	public float musicVolume = 1f;
	public bool musicVolMuted = false;
	private GameSaveLoad _levelDataLoader;
	public int choixOccurence;
	
	// Use this for initialization
	void Awake () 
	{
		PROFILE = Resources.Load("Tuning/PlayerProfile") as PlayerProfile;
		SETUP = Resources.Load("Tuning/GameSetup") as GameSetup;
		DontDestroyOnLoad(gameObject);
		//Screen.SetResolution(800,600, false);
	}
	public void trans () 
	{
		PROFILE = Resources.Load("Tuning/PlayerProfile") as PlayerProfile;
		SETUP = Resources.Load("Tuning/GameSetup") as GameSetup;
	}
	void Start() {
//		_levelDataLoader = ScriptableObject.CreateInstance<GameSaveLoad>();
//		_levelDataLoader.LoadXMLToList("blob_minute-data");
		//_levelDataLoader.getValueFromXmlDoc("BlobMinute/levels/level0","id");
		//print(_levelDataLoader._myXML[0].ListChilds[0].name);
		//print(_dataLoader.doc.OuterXml);
	}
//	public string loadDataFromXML(string _xmlPath, string _attributeName="") {
//		return _levelDataLoader.getValueFromXmlDoc(_xmlPath,_attributeName);
//	}
	public void MuteMusic()
	{
		if (musicVolMuted == true)
		{
			musicVolMuted = false;
			musicVolume = 0f;
		}
		else
		{
			musicVolMuted = true;
			musicVolume = 1f;
		}
		
	}
	
	public void MuteGlobal()
	{
		if (globalVolMuted == true)
		{
			globalVolMuted = false;
			GlobalVolume = 0f;
		}
		else
		{
			globalVolMuted = true;
			GlobalVolume = 1f;
		}
		
		
	}
	
//	void OnLevelWasLoaded(int level) {
//        if (level == 0)
//            DestroyImmediate(gameObject);
//        
//    }
}
