using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class TileImporter : MonoBehaviour {
	
	private int levelWidth;
	private int levelHeight;
	private int tileWidth;
	private int tileHeight;

	private string url;
	public int levelID = 1;
	private XmlDocument xmlDoc;
	private XmlNodeList mapNodes;
	private XmlNodeList tileNodes;
	private XmlNodeList tilesetNodes;
	private XmlNodeList itemNodes;
	
	public enum tileList
	{
		Metal
	};
	public tileList tileToGet;

	public enum objectList
	{
		Caisses,
		Interrupteur
	}
	public objectList obj;
	
	private void initXML()
	{
		xmlDoc = new XmlDocument();
		string url = "file:///" + Application.dataPath + "/"+"maps/" + levelID.ToString() + ".xml";
		xmlDoc.Load(url);
		Debug.Log("Xml successfully loaded" + xmlDoc);
		initTilesets();
	}

	private void initTilesets()
	{
		mapNodes = xmlDoc.SelectNodes("map");
		foreach(XmlNode node in mapNodes)
		{
			levelWidth = int.Parse(node.Attributes.GetNamedItem("width").Value);
			levelHeight = int.Parse(node.Attributes.GetNamedItem("height").Value);
			tileWidth = int.Parse(node.Attributes.GetNamedItem("tilewidth").Value);
			tileHeight = int.Parse(node.Attributes.GetNamedItem("tileheight").Value);
		}
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		Debug.Log("Setupped Level [LW:"+levelWidth+"][LH:"+levelHeight+"]");
		Debug.Log("Setupped Tiles [TW:"+tileWidth+"][TH:"+tileHeight+"]");
	}

	private void affectTiles()
	{

	}

	private void affectObjects()
	{

	}
	[ContextMenu ("Show Parameters")]
	private void showParameters()
	{
		initXML();
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		XmlNode tilesetParams = tilesetNodes.Item(1);
		print (tilesetParams);
		print (tilesetNodes);
	}
	
	[ContextMenu ("Get Tiles")]
	private void getTiles()
	{
		Debug.Log("Start getting tiles");
		initXML();
		tileList _currTile = tileToGet;
		tileNodes = xmlDoc.SelectNodes("map/layer/data");
		tilesetNodes = xmlDoc.SelectNodes("map/tileset");
		XmlNode tilesetParams = tilesetNodes[1];
		int _currWidth = 0;
		int _currHeight = 0;
		int _width = 0;
		int _height = 0;
		string namePrefab = "";
		string nameCase = "";

		foreach (XmlNode node in tileNodes)
		{
			foreach (XmlNode children in node.ChildNodes)
			{
				foreach (tileList _tile in Enum.GetValues(typeof(tileList)))
				{					
					int modVal = int.Parse(children.Attributes.GetNamedItem("gid").Value);
					string stringVal = "";
					if (modVal < 10)
					{ stringVal = "0" + modVal.ToString(); }
					else
					{ stringVal = modVal.ToString(); }
					namePrefab = "Tiles/" + _tile.ToString() + "/" + stringVal;

					if (Resources.Load(namePrefab) != null)
					{
						GameObject _instance = Instantiate(Resources.Load(namePrefab)) as GameObject;
						_instance.transform.parent = GameObject.Find("Level/TilesLayout").transform;
						_width = 50;
						_height = 50;
						if (_currWidth >= levelWidth)
						{
							_currWidth = 0;
							_currHeight += tileHeight;
						}
						_instance.transform.position = new Vector3 (_currWidth * _width, _currHeight, 0f);
						_currWidth += 1 ;
					}
					else
					{
						Debug.Log("The tile " + "[Tiles/" + _tile.ToString() + "XX] hasn't been found ! Fix that error NOW.PLZ.PLZ.");
					}
				}
				if (children.Attributes.GetNamedItem("gid").Value == "50" || 
				    children.Attributes.GetNamedItem("gid").Value == "100" ||
				    children.Attributes.GetNamedItem("gid").Value == "150")
				{
					StartCoroutine (Wait(3f));
					Debug.Log("Reached tile number 100, let's wait 3 sec plz. You can look at 9gag during this waiting time.");
				}
			}
		}
		Debug.Log("Finish getting tiles");
	}
	
	[ContextMenu ("Get Objects")]
	private void getObjects()
	{
		Debug.Log("Start getting item");
		initXML();
		objectList _currObj = obj;
		itemNodes = xmlDoc.SelectNodes("map/objectgroup");
		foreach (XmlNode node in itemNodes)
		{
			foreach (XmlNode children in node.ChildNodes)
			{
				foreach (objectList _obj in Enum.GetValues(typeof(objectList)))
				{
					Debug.Log(_obj.ToString());
					Debug.Log("Objects/" + children.Attributes.GetNamedItem("type").Value);
					if (Resources.Load("Objects/" + _obj.ToString()) != null)
					{
						if (children.Attributes.GetNamedItem("type").Value == _obj.ToString())
						{
							GameObject _instance = Instantiate(Resources.Load("Objects/" + children.Attributes.GetNamedItem("type").Value)) as GameObject;
							_instance.transform.position = new Vector3 (float.Parse(children.Attributes.GetNamedItem("x").Value), float.Parse(children.Attributes.GetNamedItem("y").Value), 3f);
							_instance.name = children.Attributes.GetNamedItem("name").Value;
							_instance.transform.parent = GameObject.Find("Level/Gameplay").transform;
							Debug.Log("Created a " + children.Attributes.GetNamedItem("type").Value + "at position(X" + _instance.transform.position.x + "/Y" +_instance.transform.position.y+")");
						}
					}
					else 
					{
						Debug.Log("The object " + _obj.ToString() + " hasn't been found ! Fix that error NOW.");
					}
				}
			}

		}
		Debug.Log("Finish getting item");
	}

	IEnumerator Wait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}
}