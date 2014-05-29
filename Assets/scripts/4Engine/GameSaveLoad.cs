using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class GameSaveLoad: ScriptableObject { 
	
	// An example where the encoding can be found is at 
	// http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp 
	// We will just use the KISS method and cheat a little and use 
	// the examples from the web page since they are fully described 
	
	// This is our local private members 
	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 
	//public GameObject _Player; 
	UserData myData; 
	LevelDataManager myLvlData,myLvlData2;
	LevelList myLevelList;
	List<LevelDataManager> _ListLevelDataManager;
	string _PlayerName; 
	string _data, _lvlData, _lvlListData; 
	XmlNodeList dialogNodes;
	public string defaultName = "New",defaultInnerTxt="New",defaultAttribute = "name=value";
	//public int nbDatas = 1;
	//public bool stop=false;
	#region Context Menu Calls
	[ContextMenu ("Empty All Nodes")]
	void emptyAllNodes() {
		callEmptyAllNodes(_hehe, true, true, true, true);
	}
	[ContextMenu ("Empty All Nodes Names")]
	void fillAllNodes() {
		callEmptyAllNodes(_hehe, true, false, false, true);
	}
	[ContextMenu ("Empty All Nodes InnerTxts")]
	void emptyInnerTxtNodes() {
		callEmptyAllNodes(_hehe, false, true, false, true);
	}
	[ContextMenu ("Empty All Nodes Attributes")]
	void emptyAttributesNodes() {
		callEmptyAllNodes(_hehe, false, false, true, true);
	}
	[ContextMenu ("Restore All Nodes")]
	void fillNameNodes() {
		callEmptyAllNodes(_hehe, true, true, true, false);
	}
	[ContextMenu ("Restore All Nodes Names")]
	void emptyNameNodes() {
		callEmptyAllNodes(_hehe, true, false, false, false);
	}
	[ContextMenu ("Restore All Nodes InnerTxts")]
	void fillInnerTxtNodes() {
		callEmptyAllNodes(_hehe, false, true, false, false);
	}
	[ContextMenu ("Restore All Nodes Attributes")]
	void fillAttributesNodes() {
		callEmptyAllNodes(_hehe, false, false, true, false);
	}
	#endregion

	/// <summary>
	/// This method allow you to Empty/Fill default values of Datas.
	/// </summary>
	/// <param name="List<ListString>">The List (of list?) to edit.</param>
	/// <param name="bool">If Names are affected.</param>
	/// <param name="bool">If InnerTxts are affected.</param>
	/// <param name="bool">If Attributes are affected.</param>
	/// <param name="bool">If the function must Empty or Fill values.</param>
	void callEmptyAllNodes(List<ListString> listNodes, bool names, bool innerTxts, bool attributes, bool empty) {
		int i=0;
		foreach(ListString lsElt in listNodes) {
			if(names) listNodes[i].name = (empty) ? "" : defaultName;
			if(innerTxts) listNodes[i].innerTxt = (empty) ? "" : defaultInnerTxt;
			if(attributes) {
				int cpt=0;
				foreach(string _prm in listNodes[i].ListTest) {
					listNodes[i].ListTest[cpt] = (empty) ? "" : defaultAttribute;
					cpt++;
				}
			}
			i++;
			callEmptyAllNodes(lsElt.ListChilds, names, innerTxts, attributes, empty);
		}
	}
//	public LevelParam LevelParam;
//	public LevelParam ParamAdd;

//	public List<List<string>> lala;
//	public List<string> testest,testest2;
//
//	[SerializeField] private List<string> _ListLevels;
//	[SerializeField] public List<string> ListLevels
//	{
//		get { return _ListLevels; }
//		set { _ListLevels = value; }
//	}
	public ListString myList, addedList,tmpChild;
	public List<ListString> _hehe = new List<ListString>();
	//public List<ListString> tmpChild = new List<ListString>();
	// When the EGO is instansiated the Start will trigger 
	// so we setup our initial values for our local members 
	void Start () { 
		// We setup our rectangles for our messages 
		_Save=new Rect(10,80,100,20); 
		_Load=new Rect(10,100,100,20); 
		_SaveMSG=new Rect(10,120,400,40); 
		_LoadMSG=new Rect(10,140,400,40); 
		
		// Where we want to save and load to and from 
		_FileLocation=Application.dataPath; 
		_FileName="SaveData.xml"; 
		
		// for now, lets just set the name to Joe Schmoe 
		_PlayerName = "Joe Schmoe"; 
		
		// we need soemthing to store the information into 
		//	myData=new UserData(); 
		myLvlData = new LevelDataManager();
		myLvlData2 = new LevelDataManager();
		myLevelList = new LevelList();
		myLevelList._iLevelParams.lvl1 =  new LevelDataManager();
		myLevelList._iLevelParams.lvl2 =  new LevelDataManager();
		myList = new ListString();
		myList.ListTest.Add("0");myList.ListTest.Add("1");myList.ListTest.Add("2");
		myList.ListChilds.Add(createChildNode());
		myList.ListChilds.Add(createChildNode());
		myList.ListChilds[0].ListTest.Add("c01");myList.ListChilds[0].ListTest.Add("c02");
		myList.ListChilds[1].ListTest.Add("c11");myList.ListChilds[1].ListTest.Add("c12");
		myList.ListChilds[0].ListChilds.Add(createChildNode());
		myList.ListChilds[0].ListChilds[0].ListTest.Add("c001");
		//_hehe.Add(myList as ListString);
//		_hehe.Add(createData());
//		foreach (ListString ll in _hehe) {
//			foreach(string l in ll.ListTest) {
//				Debug.Log(l);
//			}
//		}
//		testest.Add("0");testest.Add("1");testest.Add("2");testest.Add("3");
//		testest2.Add("4");testest2.Add("5");testest2.Add("6");testest2.Add("7");
//		lala.Add(testest);lala.Add(testest2);
//		foreach(List<string> l in TEST) {
//			foreach(string r in l) {
//				Debug.Log(r);
//			}
//		}
//		_ListLevelDataManager.Add(myLvlData);
//		_ListLevelDataManager.Add(myLvlData2);
	} 
	public ListString createChildNode() {
		tmpChild = new ListString();
		return tmpChild;
	}
	public ListString createData() {
		addedList = new ListString();
		//addedList.ListTest.Add("cr");
		return addedList;
	}
	public void displayData() {
		foreach (ListString ll in _hehe) {
			foreach(string l in ll.ListTest) {
				Debug.Log(l);
			}
		}
	}
	void Update () {} 
	
	void OnGUI() 
	{    
		
		//*************************************************** 
		// Loading The Player... 
		// **************************************************       
		if (GUI.Button(_Load,"Load")) { 
			
			GUI.Label(_LoadMSG,"Loading from: "+_FileLocation); 
			// Load our UserData into myData 
			LoadXML(); 
			if(_lvlListData.ToString() != "") 
			{ 
				// notice how I use a reference to type (UserData) here, you need this 
				// so that the returned object is converted into the correct type 
				//	myData = (UserData)DeserializeObject(_data);
				myLevelList = (LevelList)DeserializeObject(_lvlListData);
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(_lvlListData.ToString());

				dialogNodes = xmlDoc.SelectNodes("LevelList/_iLevelParams");

				foreach (XmlNode node in dialogNodes)
				{
					foreach (XmlNode node2 in node.ChildNodes) {
						foreach (XmlNode node3 in node2.ChildNodes) {
							XmlElement newNode = xmlDoc.CreateElement("balise");
							newNode.SetAttribute("testB","lol");
							node3.AppendChild(newNode);
							foreach (XmlNode node4 in node3.ChildNodes) {
								if(node4.Name == "UnlockCondition")
								node4.InnerText = "TEST";
							}
						}
					}
					//node.InnerText = "TEST";
					//print(node.InnerText);
				}
				_lvlListData = SerializeObject(xmlDoc);
				CreateXML(_lvlListData);
//				myLvlData = (LevelDataManager)DeserializeObject(_lvlData);
//				myLvlData2 = (LevelDataManager)DeserializeObject(_lvlData);
				// set the players position to the data we loaded 
				//VPosition=new Vector3(myData._iUser.x,myData._iUser.y,myData._iUser.z);              
				//_Player.transform.position=VPosition; 
				// just a way to show that we loaded in ok 
				//Debug.Log(myLevelList._iLevelParams.lvl1._iLevel.UnlockCondition); 
			} 
			
		} 
		
		//*************************************************** 
		// Saving The Player... 
		// **************************************************    
		if (GUI.Button(_Save,"Save")) { 
			
			GUI.Label(_SaveMSG,"Saving to: "+_FileLocation); 
//			myData._iUser.x=_Player.transform.position.x; 
//			myData._iUser.y=_Player.transform.position.y; 
//			myData._iUser.z=_Player.transform.position.z; 
			//			myData._iUser.name=_PlayerName;
			myLvlData._iLevel.bronze = 5;
			myLvlData._iLevel.gold = 15;
			myLvlData._iLevel.silver = 10;
			myLvlData._iLevel.id = 1;
			myLvlData._iLevel.UnlockCondition = "Release the Kraken";
			myLvlData2._iLevel.bronze = 3;
			myLvlData2._iLevel.gold = 9;
			myLvlData2._iLevel.silver = 6;
			myLvlData2._iLevel.id = 2;
			myLvlData2._iLevel.UnlockCondition = "Release the Blob";
			myLevelList._iLevelParams.lvl1 = myLvlData;
			myLevelList._iLevelParams.lvl2 = myLvlData2;
			
//			_ListLevelDataManager.Add(myLvlData);
//			_ListLevelDataManager.Add(myLvlData2);
//			
//			_ListLevelDataManager[0]=myLvlData;
//			_ListLevelDataManager[1]=myLvlData2;
			//print(_ListLevelDataManager[0]._iLevel.bronze);
			// Time to creat our XML! 
			//			_data = SerializeObject(myData); 
			_lvlListData = SerializeObject(myLevelList);
//			_lvlData = SerializeObject(myLvlData);
//			_lvlData += SerializeObject(myLvlData2); 
			// This is the final resulting XML from the serialization process 
			CreateXML(_lvlListData); 
			Debug.Log(_lvlListData); 
		} 
		
		
	} 
	
	/* The following metods came from the referenced URL */ 
	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 
	
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 
	
	// Here we serialize our UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		//		XmlSerializer xs = new XmlSerializer(typeof(UserData));
//		XmlSerializer xs = new XmlSerializer(typeof(LevelDataManager));  
		XmlSerializer xs = new XmlSerializer(typeof(LevelList));  
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	// Here we deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString) 
	{ 
//		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
//		XmlSerializer xs = new XmlSerializer(typeof(LevelDataManager)); 
		XmlSerializer xs = new XmlSerializer(typeof(LevelList));  
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	} 
	
	// Finally our save and load methods for the file itself 
	void CreateXML(string textToWrite) 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete(); 
			writer = t.CreateText(); 
		} 
		//writer.Write(_data);
		writer.Write(textToWrite); 
		writer.Close(); 
		Debug.Log("File written."); 
	} 
	
	void LoadXML() 
	{ 
		StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		//_data=_info; 
		//_lvlData=_info; 
		_lvlListData = _info;
		Debug.Log("File Read"); 
	} 
} 

// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData 
{ 
	// We have to define a default instance of the structure 
	public DemoData _iUser; 
	// Default constructor doesn't really do anything at the moment 
	public UserData() { } 
	
	// Anything we want to store in the XML file, we define it here 
	public struct DemoData 
	{ 
		public float x; 
		public float y; 
		public float z; 
		public string name; 
	} 
}

public class LevelList {
	public LevelsParams _iLevelParams;
	public LevelList() { } 
	public struct LevelsParams 
	{ 
		public LevelDataManager lvl1;
		public LevelDataManager lvl2;
		//public List<LevelDataManager> _ListLol;
	}
}
public class LevelDataManager
{
	public LevelData _iLevel; 
	// Default constructor doesn't really do anything at the moment 
	public LevelDataManager() { } 
	
	// Anything we want to store in the XML file, we define it here 
	public struct LevelData 
	{ 
		public float id; 
		public float gold; 
		public float silver; 
		public float bronze; 
		public string UnlockCondition; 
	} 
}
public class ListString
{
	public string name = "", innerTxt = "";
	public int niveau;
	public List<string> ListTest = new List<string>();
	public List<ListString> ListChilds = new List<ListString>();
}