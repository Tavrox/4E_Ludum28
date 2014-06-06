using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(GameSaveLoad))]
public class GameSaveLoadEditor : Editor {

	private GameSaveLoad step;
	private float maxSize = 550f;
	private float boxSize;
	private GUIStyle style;
	private int cpt,i, trueNiveau;
	[SerializeField] private LevelParam brpm;
	private Vector2 scrollPos;
	private bool xmlLoadClicked;
	public EditorWindow _Panels;
	private List<bool> listOpen;
	
	public override void OnInspectorGUI()
	{
		step = (GameSaveLoad)target;
		boxSize = maxSize / 10 ;
		brpm = LevelParam.CreateInstance("LevelParam") as LevelParam;
		step._FileLocation=Application.dataPath;

		base.OnInspectorGUI();
		#region File Creation
		if(step._myUse == GameSaveLoad.use.Create) {
			xmlLoadClicked = false;
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			if (GUILayout.Button("SaveXML", GUILayout.Width(150)))
			{
				step.SaveXML(step._myXML, step._FileName, step._XMLRoot);
			}
			GUILayout.Space(50f);
			if (GUILayout.Button("Show All Nodes",GUILayout.Width(100)))
			{
				step.showChildNodes(step._myXML, true);
			}
			GUILayout.Space(50f);
			if (GUILayout.Button("Hide All Nodes",GUILayout.Width(100)))
			{
				step.showChildNodes(step._myXML, false);
			}
			GUILayout.Space(50f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			GUILayout.Label("FileName",GUILayout.Width(boxSize * 1.2f));
			step._FileName = EditorGUILayout.TextField("",step._FileName,GUILayout.Width(boxSize));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			GUILayout.Label("XMLRoot",GUILayout.Width(boxSize * 1.2f));
			step._XMLRoot = EditorGUILayout.TextField("",step._XMLRoot,GUILayout.Width(boxSize));
			EditorGUILayout.EndHorizontal();
			if (GUILayout.Button("Add Data", GUILayout.ExpandWidth(true)))
			{
				//step.nbDatas++;
				step._myXML.Add(step.CreateData());
				step._myXML[step._myXML.Count-1].niveau = 0;
				step._myXML[step._myXML.Count-1].name = step.defaultName;
				step._myXML[step._myXML.Count-1].innerTxt = step.defaultInnerTxt;
				step._myXML[step._myXML.Count-1].attributes.Add(step.defaultAttribute);
			}
			if(step._myXML.Count>0) {
				trueNiveau=0;
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
				displayChildNodes(step._myXML);
				EditorGUILayout.EndScrollView();
			}
			EditorUtility.SetDirty(step);
		}
		#endregion
		
		#region File Edition
		else if(step._myUse == GameSaveLoad.use.Edit && step.fileToEdit != null) {
			step._FileName = step.fileToEdit.name;
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			if (GUILayout.Button("SaveXML",GUILayout.Width(100)))
			{
				step.SaveXML(step._myXML, step._FileName, step._XMLRoot);
			}
			GUILayout.Space(30f);
			if (GUILayout.Button("LoadXMLToList",GUILayout.Width(100)))
			{
				//step.SaveXML(step._myXML, step._FileName, step._XMLRoot);
				step._myXML.Clear();
				step.LoadXMLToList(step._FileName);
				//step._myXML = null;
				xmlLoadClicked = true;
			}
			GUILayout.Space(30f);
			if (GUILayout.Button("Show All Nodes",GUILayout.Width(100)))
			{
				step.showChildNodes(step._myXML, true);
			}
			GUILayout.Space(30f);
			if (GUILayout.Button("Hide All Nodes",GUILayout.Width(100)))
			{
				step.showChildNodes(step._myXML, false);
			}
			EditorGUILayout.EndHorizontal();
			
			if(xmlLoadClicked) {
				trueNiveau=0;
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
				displayChildNodes(step._myXML);
				EditorGUILayout.EndScrollView();
			}
		}
		#endregion
	}
	void displayChildNodes(List<XMLNodeCustom> listNodes) {
		int i=0;
		foreach(XMLNodeCustom lsElt in listNodes) {
			GUILayout.Label("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯",GUILayout.ExpandWidth(true),GUILayout.Height(5));
			//Debug.Log("Niveau : "+trueNiveau + "."+ i +" - "+listNodes[i].name +" - "+listNodes[i].innerTxt);
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			for(int cptNiveau = 0; cptNiveau<trueNiveau;cptNiveau++) {
				//GUILayout.Box("",currentStyle,GUILayout.Width(boxSize / 2));
				//GUILayout.Space(20f);
				GUILayout.Label("|----------",GUILayout.Width(boxSize / 1.5f));
			}
			if (GUILayout.Button((listNodes[i].displayed)?"-":"+",GUILayout.Width(boxSize / 2.65f)))
			{
				listNodes[i].displayed=!listNodes[i].displayed;
			}
			GUILayout.Label("DataName",GUILayout.Width(boxSize * 1.2f));
			listNodes[i].name = EditorGUILayout.TextField("",listNodes[i].name,GUILayout.Width(boxSize));
			if(listNodes[i].displayed) {
				GUILayout.Label("InnerText",GUILayout.Width(boxSize * 1.15f));
				listNodes[i].innerTxt = EditorGUILayout.TextField("",listNodes[i].innerTxt,GUILayout.Width(boxSize));
				GUILayout.Space(20f);
				if (GUILayout.Button(trueNiveau+"Add Child",GUILayout.Width(boxSize * 1.5f)))
				{
					//Debug.Log(listNodes[i].ListChilds.Count-1);
					listNodes[i].ListChilds.Add(step.CreateChildNode());
					//Debug.Log(trueNiveau);

					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].niveau = trueNiveau+1;
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].name = step.defaultName;
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].innerTxt = step.defaultInnerTxt;
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].attributes.Add(step.defaultAttribute);
				}
				if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
				{
					listNodes.Remove(listNodes[i]);
					//step.nbDatas--;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				for(int cptNiveau = 0; cptNiveau<trueNiveau;cptNiveau++) {
					GUILayout.Label("|----------",GUILayout.Width(boxSize / 1.5f));
				}
				GUILayout.Space(25);
				GUILayout.Label("Attributes",GUILayout.Width(boxSize * 1.2f));
				
				//GUILayout.Space(15f);
				if (GUILayout.Button("+",GUILayout.Width(boxSize / 2.65f)))
				{
					if (listNodes[i].attributes == null)
					{
						listNodes[i].attributes = new List<string>();
					}
					listNodes[i].attributes.Add(step.defaultAttribute);
				}
				GUILayout.Space(15f);
				//GUILayout.Box("Attributes",GUILayout.Width(boxSize * 1.5f));
				int cpt=0;
				foreach(string _prm in listNodes[i].attributes) {
					//Debug.Log(listNodes[i].attributes[cpt]);
					if (_prm != null && listNodes[i].attributes[cpt] != null)
					{
						listNodes[i].attributes[cpt] = EditorGUILayout.TextField("",_prm,GUILayout.Width(boxSize*1.5f));
						if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
						{
							listNodes[i].attributes.Remove(_prm);
						}
					}
					cpt++;
					//j++;
				}

				EditorGUILayout.EndHorizontal();
				i++;
				if(lsElt.ListChilds.Count != 0) {trueNiveau++;displayChildNodes(lsElt.ListChilds);}
				if(i == listNodes.Count /*&& lsElt.ListChilds.Count == 0*/) {trueNiveau--;}
			}
			else {
				if (GUILayout.Button(trueNiveau+"Add Child",GUILayout.Width(boxSize * 1.5f)))
				{
					//Debug.Log(listNodes[i].ListChilds.Count-1);
					listNodes[i].ListChilds.Add(step.CreateChildNode());
					//Debug.Log(trueNiveau);
					
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].niveau = trueNiveau+1;
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].name = step.defaultName;
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].innerTxt = step.defaultInnerTxt;
					listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].attributes.Add(step.defaultAttribute);
				}
				if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
				{
					listNodes.Remove(listNodes[i]);
					//step.nbDatas--;
				}
				i++;
				if(i == listNodes.Count /*&& lsElt.ListChilds.Count == 0*/) {trueNiveau--;}
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	void displayBrickInfo(LevelParam _prm)
	{
		_prm.stepID					= EditorGUILayout.IntField("", _prm.stepID, GUILayout.Width(boxSize / 1.5f));
		_prm.ID						= EditorGUILayout.IntField("", _prm.ID, GUILayout.Width(boxSize));
		_prm.occurence				= EditorGUILayout.IntField("", _prm.occurence, GUILayout.Width(boxSize));
		_prm.goldScore				= EditorGUILayout.IntField("", _prm.goldScore , GUILayout.Width(boxSize));
		_prm.silverScore			= EditorGUILayout.IntField("", _prm.silverScore, GUILayout.Width(boxSize));
		_prm.bronzeScore			= EditorGUILayout.IntField("", _prm.bronzeScore, GUILayout.Width(boxSize));
		_prm.Locked 				= EditorGUILayout.Toggle("", _prm.Locked, GUILayout.Width(boxSize));
		_prm.UnlockCondition 		= EditorGUILayout.TextField("", _prm.UnlockCondition, GUILayout.Width(boxSize));
	}


	void debugLogChildNodes(List<XMLNodeCustom> listNodes) {
		int i=0;
		foreach(XMLNodeCustom lsElt in listNodes) {
			Debug.Log("Niveau : "+trueNiveau + "."+ i +" - "+listNodes[i].name +" - "+listNodes[i].innerTxt);
			int cpt=0;
			foreach(string _prm in listNodes[i].attributes) {
				Debug.Log(listNodes[i].attributes[cpt]);
				cpt++;
			}
			i++;
			if(lsElt.ListChilds.Count != 0) {trueNiveau++;displayChildNodes(lsElt.ListChilds);}
			else if(i == listNodes.Count /*&& lsElt.ListChilds.Count == 0*/) {trueNiveau--;}
		}
	}
}