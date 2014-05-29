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
	
	public override void OnInspectorGUI()
	{
		step = (GameSaveLoad)target;
		boxSize = maxSize / 10 ;
		brpm = LevelParam.CreateInstance("LevelParam") as LevelParam;

		base.OnInspectorGUI();
		
		if (GUILayout.Button("Add Data", GUILayout.ExpandWidth(true)))
		{
			//step.nbDatas++;
			step._hehe.Add(step.createData());
			step._hehe[step._hehe.Count-1].niveau = 0;
			step._hehe[step._hehe.Count-1].name = step.defaultName;
			step._hehe[step._hehe.Count-1].innerTxt = step.defaultInnerTxt;
			step._hehe[step._hehe.Count-1].ListTest.Add(step.defaultAttribute);
		}
//		if (GUILayout.Button("Refresh", GUILayout.ExpandWidth(true)))
//		{
//			trueNiveau=0;
//			//displayChildNodes(step._hehe);
//			debugLogChildNodes(step._hehe);
//		}
//		if(step.ListLevels.Count > 0) {
//		if(!step.stop) { //LoadXML to construct _hehe
//			step.stop=true;
//			step.myList = new ListString();
//			step.myList.name="c0name";
//			step.myList.ListTest.Add("0");step.myList.ListTest.Add("1");step.myList.ListTest.Add("2");
//			step.myList.ListChilds.Add(step.createChildNode());
//			step.myList.ListChilds[0].name="c01name";
//			step.myList.ListChilds[0].ListTest.Add("c01");step.myList.ListChilds[0].ListTest.Add("c02");
//			step.myList.ListChilds[0].ListChilds.Add(step.createChildNode());
//			step.myList.ListChilds[0].ListChilds[0].name="c001name";
//			step.myList.ListChilds[0].ListChilds[0].ListTest.Add("c001");
//			step.myList.ListChilds[0].ListChilds.Add(step.createChildNode());
//			step.myList.ListChilds[0].ListChilds[1].name="c002name";
//			step.myList.ListChilds[0].ListChilds[1].ListTest.Add("c002");
//			step.myList.ListChilds.Add(step.createChildNode());
//			step.myList.ListChilds[1].name="c11name";
//			step.myList.ListChilds[1].ListTest.Add("c11");step.myList.ListChilds[1].ListTest.Add("c12");
//			step._hehe.Add(step.myList as ListString);
//			step._hehe.Add(step.myList as ListString);
//			//step._hehe.Add(step.createData());
//		}
		if(step._hehe.Count>0) {
			//setInterfaceBasis();
			trueNiveau=0;
			//EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			//GUILayout.Label("lololoollllolo lololoollllolo lololoollllolo lololoollllolo lololoollllololololoollllolo lololoollllolo lololoollllolo lololoollllolo");

			displayChildNodes(step._hehe);
			EditorGUILayout.EndScrollView();
			//EditorGUILayout.EndHorizontal();
		}

		EditorUtility.SetDirty(step);

//		}
	}

//	void setInterfaceBasis() {
//		EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
//		GUILayout.Box("Filename",GUILayout.Width(boxSize * 1.5f));
//		EditorGUILayout.TextField("","",GUILayout.Width(boxSize));
//		EditorGUILayout.EndHorizontal();
//
//		//Debug.Log(step._hehe[0].ListChilds[0].ListChilds[0].name);
//
//		for(i=0; i<step._hehe.Count; i++) {
//			EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
//			GUILayout.Box("DataName",GUILayout.Width(boxSize * 1.5f));
//			step._hehe[i].name = EditorGUILayout.TextField("",step._hehe[i].name,GUILayout.Width(boxSize));
//			GUILayout.Box("InnerText",GUILayout.Width(boxSize * 1.5f));
//			step._hehe[i].innerTxt = EditorGUILayout.TextField("",step._hehe[i].innerTxt,GUILayout.Width(boxSize));
//			if (GUILayout.Button("Add Child",GUILayout.Width(boxSize)))
//			{
//				step._hehe[i].ListChilds.Add(step.createChildNode());
//			}
//			if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
//			{
//				step._hehe.Remove(step._hehe[i]);
//				step.nbDatas--;
//			}
//			EditorGUILayout.EndHorizontal();
//			EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
//			GUILayout.Box("Attributes",GUILayout.Width(boxSize * 1.5f));
//			if (GUILayout.Button("+",GUILayout.Width(boxSize / 2.65f)))
//			{
//				if (step._hehe[i].ListTest == null)
//				{
//					step._hehe[i].ListTest = new List<string>();
//				}
//				//			AssetDatabase.CreateAsset(brpm , "Assets/Resources/Tools/Parameters/" + step.NAME + "/" + Random.Range(0,1000000).ToString() +".asset");
//				//			EditorUtility.SetDirty(brpm);
//				step._hehe[i].ListTest.Add("name=value");
//			}
//			cpt = 0;
//			foreach(string _prm in step._hehe[i].ListTest)
//			{
//				if (_prm != null && step._hehe[i].ListTest[cpt] != null)
//				{
//					
//					step._hehe[i].ListTest[cpt] = EditorGUILayout.TextField("",_prm,GUILayout.Width(boxSize));
//					//if(i==0) print(step._hehe[0].ListTest[0]+" "+_prm);
//					//				displayBrickInfo(AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath(_prm), typeof(LevelParam)) as LevelParam);
//					//				EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath(_prm), typeof(LevelParam)) as LevelParam);
//					if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
//					{
//						//						DestroyObject(_prm);
//						//AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_prm));
//						step._hehe[i].ListTest.Remove(_prm);// .ListLevels.Remove(_prm);
//					}
//				}
//				cpt++;
//			}
//		//	displayChildNodes(step._hehe);
//			//EditorGUILayout.EndHorizontal();
//			//EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
//			//		displayBrickInfo(brpm);
//
//			EditorGUILayout.EndHorizontal();
//			EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
//			///displayNodes(step._hehe[i].ListChilds);
//			EditorGUILayout.EndHorizontal();
//		}
//
//		//displayNodes();
//	}
	void debugLogChildNodes(List<ListString> listNodes) {
		int i=0;
		foreach(ListString lsElt in listNodes) {
			Debug.Log("Niveau : "+trueNiveau + "."+ i +" - "+listNodes[i].name +" - "+listNodes[i].innerTxt);
			int cpt=0;
			foreach(string _prm in listNodes[i].ListTest) {
				Debug.Log(listNodes[i].ListTest[cpt]);
				cpt++;
			}
			i++;
			if(lsElt.ListChilds.Count != 0) {trueNiveau++;displayChildNodes(lsElt.ListChilds);}
			else if(i == listNodes.Count /*&& lsElt.ListChilds.Count == 0*/) {trueNiveau--;}
		}
	}
//	[ContextMenu ("Empty All Nodes")]
//	public void emptyAllNodes() {
//		callEmptyAllNodes(step._hehe);
//	}

	void displayChildNodes(List<ListString> listNodes) {
		int i=0;
		foreach(ListString lsElt in listNodes) {
			//Debug.Log("Niveau : "+trueNiveau + "."+ i +" - "+listNodes[i].name +" - "+listNodes[i].innerTxt);
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			for(int cptNiveau = 0; cptNiveau<trueNiveau;cptNiveau++) {
				//GUILayout.Box("",currentStyle,GUILayout.Width(boxSize / 2));
				//GUILayout.Space(20f);
				GUILayout.Label("|----------",GUILayout.Width(boxSize / 1.5f));
			}
			GUILayout.Label("DataName",GUILayout.Width(boxSize * 1.2f));
			//GUILayout.Box("DataName",GUILayout.Width(boxSize * 1.5f));
			listNodes[i].name = EditorGUILayout.TextField("",listNodes[i].name,GUILayout.Width(boxSize));
			GUILayout.Label("InnerText",GUILayout.Width(boxSize * 1.15f));
			//GUILayout.Box("InnerText",GUILayout.Width(boxSize * 1.5f));
			listNodes[i].innerTxt = EditorGUILayout.TextField("",listNodes[i].innerTxt,GUILayout.Width(boxSize));
			GUILayout.Space(20f);
			if (GUILayout.Button(trueNiveau+"Add Child",GUILayout.Width(boxSize * 1.5f)))
			{
				//Debug.Log(listNodes[i].ListChilds.Count-1);
				listNodes[i].ListChilds.Add(step.createChildNode());
				Debug.Log(trueNiveau);

				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].niveau = trueNiveau+1;
				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].name = step.defaultName;
				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].innerTxt = step.defaultInnerTxt;
				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].ListTest.Add(step.defaultAttribute);
			}
			if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
			{
				listNodes.Remove(listNodes[i]);
				//step.nbDatas--;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			for(int cptNiveau = 0; cptNiveau<trueNiveau;cptNiveau++) {
				//GUILayout.Box("",GUILayout.Width(boxSize / 2));
				GUILayout.Label("|----------",GUILayout.Width(boxSize / 1.5f));
			}
			GUILayout.Label("Attributes",GUILayout.Width(boxSize * 1.2f));
			
			GUILayout.Space(15f);
			if (GUILayout.Button("+",GUILayout.Width(boxSize / 2.65f)))
			{
				if (listNodes[i].ListTest == null)
				{
					listNodes[i].ListTest = new List<string>();
				}
				listNodes[i].ListTest.Add(step.defaultAttribute);
			}
			GUILayout.Space(15f);
			//GUILayout.Box("Attributes",GUILayout.Width(boxSize * 1.5f));
			int cpt=0;
			foreach(string _prm in listNodes[i].ListTest) {
				//Debug.Log(listNodes[i].ListTest[cpt]);
				if (_prm != null && listNodes[i].ListTest[cpt] != null)
				{
					
					listNodes[i].ListTest[cpt] = EditorGUILayout.TextField("",_prm,GUILayout.Width(boxSize*1.5f));
					//if(i==0) print(step._hehe[0].ListTest[0]+" "+_prm);
					//				displayBrickInfo(AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath(_prm), typeof(LevelParam)) as LevelParam);
					//				EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath(_prm), typeof(LevelParam)) as LevelParam);
					if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
					{
						//						DestroyObject(_prm);
						//AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_prm));
						listNodes[i].ListTest.Remove(_prm);// .ListLevels.Remove(_prm);
					}
				}
				cpt++;
				//j++;
			}
			EditorGUILayout.EndHorizontal();
			//EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
			//displayNodes(listNodes[i].ListChilds);
			i++;
			if(lsElt.ListChilds.Count != 0) {trueNiveau++;displayChildNodes(lsElt.ListChilds);}
			if(i == listNodes.Count /*&& lsElt.ListChilds.Count == 0*/) {trueNiveau--;}

			//EditorGUILayout.EndHorizontal();
		}
	}
	void displayNodes(List<ListString> listNodes) {
		for(int i=0; i<listNodes.Count; i++) {
			EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
			GUILayout.Box("DataName",GUILayout.Width(boxSize * 1.5f));
			listNodes[i].name = EditorGUILayout.TextField("",listNodes[i].name,GUILayout.Width(boxSize));
			GUILayout.Box("InnerText",GUILayout.Width(boxSize * 1.5f));
			listNodes[i].innerTxt = EditorGUILayout.TextField("",listNodes[i].innerTxt,GUILayout.Width(boxSize));
			if (GUILayout.Button("Add Child",GUILayout.Width(boxSize / 2.65f)))
			{
				listNodes[i].ListChilds.Add(step.createChildNode());
				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].name = "New";
				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].innerTxt = "New";
				listNodes[i].ListChilds[listNodes[i].ListChilds.Count-1].ListTest.Add("");
			}
			if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
			{
				listNodes.Remove(listNodes[i]);
				//step.nbDatas--;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
			GUILayout.Box("Attributes",GUILayout.Width(boxSize * 1.5f));
			cpt = 0;
			foreach(string _prm in listNodes[i].ListTest)
			{
				if (_prm != null && listNodes[i].ListTest[cpt] != null)
				{
					
					listNodes[i].ListTest[cpt] = EditorGUILayout.TextField("",_prm,GUILayout.Width(boxSize));
					//if(i==0) print(step._hehe[0].ListTest[0]+" "+_prm);
					//				displayBrickInfo(AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath(_prm), typeof(LevelParam)) as LevelParam);
					//				EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath(_prm), typeof(LevelParam)) as LevelParam);
					if (GUILayout.Button("X", GUILayout.Width(boxSize / 3)))
					{
						//						DestroyObject(_prm);
						//AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_prm));
						listNodes[i].ListTest.Remove(_prm);// .ListLevels.Remove(_prm);
					}
				}
				cpt++;
			}
			//EditorGUILayout.EndHorizontal();
			//EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
			//		displayBrickInfo(brpm);
			GUILayout.Space(20f);
			if (GUILayout.Button("+",GUILayout.Width(boxSize / 2.65f)))
			{
				if (listNodes[i].ListTest == null)
				{
					listNodes[i].ListTest = new List<string>();
				}
				//			AssetDatabase.CreateAsset(brpm , "Assets/Resources/Tools/Parameters/" + step.NAME + "/" + Random.Range(0,1000000).ToString() +".asset");
				//			EditorUtility.SetDirty(brpm);
				listNodes[i].ListTest.Add("name=value");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal(GUILayout.Width(maxSize));
			displayNodes(listNodes[i].ListChilds);
			EditorGUILayout.EndHorizontal();
		}
	}
	private void displayBrickInfo(LevelParam _prm)
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
}
