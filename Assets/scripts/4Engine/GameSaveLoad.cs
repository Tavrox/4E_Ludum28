using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class GameSaveLoad: ScriptableObject {
	
	public string defaultName = "New",defaultInnerTxt="New",defaultAttribute = "name=value";
	[HideInInspector] public string _FileLocation=Application.dataPath,_FileName="file.xml",_XMLRoot, searchResult; 

	XmlNodeList _xmlNodes, _xmlNodeSearch;
	public XMLNodeCustom myList, addedList, tmpChild;
	public List<XMLNodeCustom> _myXML = new List<XMLNodeCustom>();
	public enum use { Create, Edit }
	public use _myUse;
	public TextAsset fileToEdit;
	int myNiveau;
	public XmlDocument doc, tempDoc;
	
	void Start() {
			_FileLocation=Application.dataPath;
	}
	
	/// <summary>
	/// Save the Structure & Data defined in Inspector after clicking SaveXML button.
	/// </summary>
	/// <param name="List<XMLNodeCustom>">The List of Datas to convert to XML.</param>
	/// <param name="string">Name of the file to be created.</param>
	/// <param name="string">Name of the XML Root Element.</param>
	public void SaveXML(List<XMLNodeCustom> listNodes, string fileName, string XMLRoot) {
		doc = new XmlDocument();
		doc.LoadXml("<"+XMLRoot+">" +
		            "</"+XMLRoot+">"); 

		_xmlNodes = doc.SelectNodes(XMLRoot);
		foreach (XmlNode node in _xmlNodes) //Creating Root node
		{
			ListToXML(doc, node, listNodes); //Creates all XML structure & Datas from EditorInspector values
		}
		CreateXML(SerializeObject(doc,typeof(XmlDocument)),_FileLocation,fileName);
		//Debug.Log(doc.OuterXml); //Show XML result
	}

	/// <summary>
	/// Creates all XML structure & Datas from EditorInspector values ("kind of recursive" -> calls itself to reach all childs).
	/// </summary>
	/// <param name="XmlDocument">The XML Document to edit.</param>
	/// <param name="XmlNode">The First node to which child nodes will be added.</param>
	/// <param name="List<XMLNodeCustom>">The current List of XMLNodeCustom.</param>
	void ListToXML(XmlDocument _doc, XmlNode _parentNode, List<XMLNodeCustom> _listNodes) {
		foreach(XMLNodeCustom _lsElt in _listNodes) { //Run through the node of this level
			XmlElement newElem = _doc.CreateElement(_lsElt.name); //Create a node with XMLNodeCustom's name
			newElem.InnerText = _lsElt.innerTxt; //Set it's InnerText with XMLNodeCustom's innerTxt
			foreach(string _prm in _lsElt.attributes) { //Create the nodes attributes (name=value splited)
				newElem.SetAttributeNode(_prm.Split('=')[0],"");
				newElem.SetAttribute(_prm.Split('=')[0],_prm.Split('=')[1]);
			}
			ListToXML(_doc, newElem, _lsElt.ListChilds); //Do the same for this node's childs
			_parentNode.AppendChild(newElem); //Add the node to it's parent
		}
	}

	/// <summary>
	/// Save the Structure & Data defined in Inspector after clicking SaveXML button.
	/// </summary>
	/// <param name="List<XMLNodeCustom>">The List of Datas to convert to XML.</param>
	/// <param name="string">Name of the file to be created.</param>
	/// <param name="string">Name of the XML Root Element.</param>
	public void LoadXMLToList(string fileName) {
		doc = new XmlDocument();
		doc.LoadXml(ReadXML(_FileLocation,fileName)); 

		_XMLRoot = doc.ChildNodes[1].Name; //On ne prend pas la déclaration du <?xml ?> qui a l'id 0
		_xmlNodes = doc.SelectNodes(_XMLRoot);
		XMLToList(_xmlNodes[0], _myXML);
	}
	/// <summary>
	/// Creates all Lists in EditorInspector XML Structures & Data
	/// </summary>
	/// <param name="XmlDocument">The XML Document to edit.</param>
	/// <param name="XmlNode">The First node to which child nodes will be added.</param>
	/// <param name="List<XMLNodeCustom>">The current List of XMLNodeCustom.</param>
	void XMLToList(XmlNode _parentNode, List<XMLNodeCustom> _listNodes) {
		foreach (XmlNode node in _parentNode)
		{
			if(node.Attributes != null) {
				//Debug.Log(node.Name);
				_listNodes.Add(CreateData());
				myNiveau=0;
				_listNodes[_listNodes.Count-1].niveau = countParentNode(node);
				//Debug.Log(_listNodes[_listNodes.Count-1].niveau);
				_listNodes[_listNodes.Count-1].name = node.Name;
				_listNodes[_listNodes.Count-1].innerTxt = node.InnerText;
				foreach (XmlAttribute attr in node.Attributes)
				{
					//Debug.Log(attr.Name+"="+attr.Value);
					_listNodes[_listNodes.Count-1].attributes.Add(attr.Name+"="+attr.Value);
				}
				if(node.HasChildNodes) {myNiveau++;XMLToList(node, _listNodes[_listNodes.Count-1].ListChilds);}
			}
		}
	}
	int countParentNode (XmlNode _node) {
		if(_node.ParentNode != null && _node.ParentNode.Name != "#document") {
			myNiveau++;
			return countParentNode(_node.ParentNode);
		}
		else return myNiveau;
	}
	/// <summary>
	/// Save textToWrite in _FileName
	/// </summary>
	/// <param name="string" />
	void CreateXML(string textToWrite, string FileLocation, string FileName) 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(FileLocation+"/Resources"+"/"+ FileName+".bg"); 
		Debug.Log(FileLocation+"/Resources"+"/"+ FileName+".bg");
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete(); 
			writer = t.CreateText(); 
		} 
		writer.Write(textToWrite); 
		writer.Close(); 
		Debug.Log("File written."); 
	} 
	
	public string ReadXML(string FileLocation, string FileName) 
	{ 
		StreamReader r = File.OpenText(FileLocation+"/Resources"+"/"+ FileName+".bg"); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		return _info;
		Debug.Log("File Read"); 
	}
	
	/// <summary>
	/// Returns a new XMLNodeCustom
	/// </summary>
	public XMLNodeCustom CreateChildNode() {
		tmpChild = new XMLNodeCustom();
		return tmpChild;
	}
	/// <summary>
	/// Returns a new XMLNodeCustom
	/// </summary>
	public XMLNodeCustom CreateData() {
		addedList = new XMLNodeCustom();
		//addedList.attributes.Add("cr");
		return addedList;
	}

	#region Serialization & Tools

	/// <summary>
	/// Get the value of an attribute from the specified node. Ex : "BlobMinute/Levels/Level0","score".
	/// </summary>
	/// <param name="string" />
	/// <param name="string" />
	public string getValueFromXmlDoc(string xmlPath, string attributeName="") {
		//tempDoc = new XmlDocument();
		searchResult = "-1";
		_xmlNodeSearch = doc.SelectNodes(xmlPath);
		foreach (XmlAttribute attr in _xmlNodeSearch[0].Attributes)
		{
			if(attr.Name == attributeName) {
				searchResult = attr.Value;
				break;
			}
		}
		return searchResult;
	}
	/// <summary>
	/// Get the value of an attribute from the specified node. Ex : "BlobMinute/Levels/Level0","score".
	/// </summary>
	/// <param name="string" />
	/// <param name="string" />
	/// <param name="string" />
	/// <param name="bool" />
	public void setValueInXmlDoc(string xmlPath, string attributeName="", string attributeValue="", bool mustSave=true) {
		//tempDoc = new XmlDocument();
		//searchResult = "-1";
		_xmlNodeSearch = doc.SelectNodes(xmlPath);
//		Debug.Log(_xmlNodeSearch[0].OuterXml);
//		Debug.Log("setValueInXmlDoc");
		foreach (XmlAttribute attr in _xmlNodeSearch[0].Attributes)
		{
			if(attr.Name == attributeName) {
				//_xmlNodeSearch[0].
				
				attr.Value = attributeValue;
				break;
			}
		}
        if (mustSave) CreateXML(SerializeObject(doc, typeof(XmlDocument)), _FileLocation, "plr");
		//return searchResult;
	}
	
	/// <summary>
	/// Show or Hide all ChildNodes with displayState true or false.
	/// </summary>
	/// <param name="bool" />
	public void showChildNodes(List<XMLNodeCustom> listNodes, bool displayState) {
		foreach(XMLNodeCustom lsElt in listNodes) {
			lsElt.displayed=displayState;
			if(lsElt.ListChilds.Count != 0) showChildNodes(lsElt.ListChilds,displayState);
		}
	}
	/// <summary>
	/// The following metods came from the referenced URL.
	/// </summary>
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
	/// <summary>
	/// Serialize pObject object of System.Type _type.
	/// </summary>
	/// <param name="object" />
	/// <param name="System.Type" />
	string SerializeObject(object pObject, System.Type _type) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer(_type); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	/// <summary>
	/// Deserialize pXmlizedString object of System.Type _type.
	/// </summary>
	/// <param name="object" />
	/// <param name="System.Type" />
	object DeserializeObject(string pXmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(List<XMLNodeCustom>));  
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	} 
	#endregion

	#region Context Menu Calls
	[ContextMenu ("Empty All Nodes")]
	void emptyAllNodes() {
		callEmptyAllNodes(_myXML, true, true, true, true);
	}
	[ContextMenu ("Empty All Nodes Names")]
	void fillAllNodes() {
		callEmptyAllNodes(_myXML, true, false, false, true);
	}
	[ContextMenu ("Empty All Nodes InnerTxts")]
	void emptyInnerTxtNodes() {
		callEmptyAllNodes(_myXML, false, true, false, true);
	}
	[ContextMenu ("Empty All Nodes Attributes")]
	void emptyAttributesNodes() {
		callEmptyAllNodes(_myXML, false, false, true, true);
	}
	[ContextMenu ("Restore All Nodes")]
	void fillNameNodes() {
		callEmptyAllNodes(_myXML, true, true, true, false);
	}
	[ContextMenu ("Restore All Nodes Names")]
	void emptyNameNodes() {
		callEmptyAllNodes(_myXML, true, false, false, false);
	}
	[ContextMenu ("Restore All Nodes InnerTxts")]
	void fillInnerTxtNodes() {
		callEmptyAllNodes(_myXML, false, true, false, false);
	}
	[ContextMenu ("Restore All Nodes Attributes")]
	void fillAttributesNodes() {
		callEmptyAllNodes(_myXML, false, false, true, false);
	}
	
	/// <summary>
	/// This method allows you to Empty/Fill default values of Datas.
	/// </summary>
	/// <param name="List<XMLNodeCustom>">The List (of list?) to edit.</param>
	/// <param name="bool">If Names are affected.</param>
	/// <param name="bool">If InnerTxts are affected.</param>
	/// <param name="bool">If Attributes are affected.</param>
	/// <param name="bool">If the function must Empty or Fill values.</param>
	void callEmptyAllNodes(List<XMLNodeCustom> listNodes, bool names, bool innerTxts, bool attributes, bool empty) {
		int i=0;
		foreach(XMLNodeCustom lsElt in listNodes) {
			if(names) listNodes[i].name = (empty) ? "" : defaultName;
			if(innerTxts) listNodes[i].innerTxt = (empty) ? "" : defaultInnerTxt;
			if(attributes) {
				int cpt=0;
				foreach(string _prm in listNodes[i].attributes) {
					listNodes[i].attributes[cpt] = (empty) ? "" : defaultAttribute;
					cpt++;
				}
			}
			i++;
			callEmptyAllNodes(lsElt.ListChilds, names, innerTxts, attributes, empty);
		}
	}
	#endregion
} 

public class XMLNodeCustom
{
	public bool displayed=false;
	public string name = "", innerTxt = "";
	public int niveau;
	public List<string> attributes = new List<string>();
	public List<XMLNodeCustom> ListChilds = new List<XMLNodeCustom>();
}