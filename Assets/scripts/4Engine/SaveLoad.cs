using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveLoad : MonoBehaviour {
	
	//public static List<Game> savedGames = new List<Game>();
	
	//it's static so we can call it from anywhere
	[ContextMenu ("Yeah Save")]
	public void launchSave () {
		Save(Path.Combine(Application.persistentDataPath, "monsters.xml"));
	}
	public void Save(string path) {
		var serializer = new XmlSerializer(typeof(SaveLoad));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			//serializer.Serialize(stream, "LOLOL");
		}
//		//SaveLoad.savedGames.Add(Game.current);
//		BinaryFormatter bf = new BinaryFormatter();
//		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
//		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.txt"); //you can call it anything you want
//		Debug.Log(Application.persistentDataPath + "/savedGames.gd");
//		bf.Serialize(file, "Lol");
//		file.Close();
	}   
	
	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.txt", FileMode.Open);
			//SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
			file.Close();
		}
	}
}