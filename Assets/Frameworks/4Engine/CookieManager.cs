using UnityEngine;
using System.Collections;

public class CookieManager : MonoBehaviour {

	//private GUIText textTest;
	public string Level1Unlocked="0", Level2Unlocked="0", Level3Unlocked="0", Level4Unlocked="0", Level5Unlocked="0";
	private string _recoveredValue="NO";
	private GameObject lvl1,lvl2,lvl3,lvl4,lvl5;

	// Use this for initialization
	void Start () {
		//textTest = gameObject.GetComponent<GUIText>();
		writeCommunicationFunction();
		lvl1 = GameObject.Find("Menu/Level1");
		lvl2 = GameObject.Find("Menu/Level2");
		lvl3 = GameObject.Find("Menu/Level3");
		lvl4 = GameObject.Find("Menu/Level4");
		lvl5 = GameObject.Find("Menu/Level5");
//		setCookie("TOAST","YEAH");
		setCookie("Level1Unlocked","1");
		//print (Level1Unlocked);
		//StartCoroutine("getLevelUnlocked");
		getLevelUnlocked();
		//print (_recoveredValue);
	}
	void getLevelUnlocked() {
		getCookie("Level1Unlocked");Level1Unlocked = _recoveredValue;
		getCookie("Level2Unlocked");
		if(_recoveredValue!="NULL") Level2Unlocked = _recoveredValue;
		else Level2Unlocked = "0";
		getCookie("Level3Unlocked");
		if(_recoveredValue!="NULL") Level3Unlocked = _recoveredValue;
		else Level3Unlocked = "0";
		getCookie("Level4Unlocked");
		if(_recoveredValue!="NULL") Level4Unlocked = _recoveredValue;
		else Level4Unlocked = "0";
		getCookie("Level5Unlocked");
		if(_recoveredValue!="NULL") Level5Unlocked = _recoveredValue;
		else Level5Unlocked = "0";
		//yield return new WaitForSeconds(1f);
		/*string tr = "NO";
		getCookie("Level1Unlocked");tr = tr+"get1";
		//yield return new WaitForSeconds(0f);
		Level1Unlocked = _recoveredValue;tr = tr+"-"+Level1Unlocked+"/";
		//yield return new WaitForSeconds(1f);
		getCookie("Level2Unlocked");tr = tr+"get2";
		
		//yield return new WaitForSeconds(0f);
		if(_recoveredValue!="NULL") Level2Unlocked = _recoveredValue;
		else Level2Unlocked = "0";
		//yield return new WaitForSeconds(1f);
		tr = tr+"-"+Level2Unlocked+"/";
		getCookie("Level3Unlocked");tr = tr+"get3";
		//yield return new WaitForSeconds(0f);
		if(_recoveredValue!="NULL") Level3Unlocked = _recoveredValue;
		else Level3Unlocked = "0";
		//yield return new WaitForSeconds(1f);
		tr = tr+"-"+Level3Unlocked+"/";*/
		//Application.ExternalEval("alert(\""+tr+"\");");
		//hideUnlockedLevels();
	}
	void hideUnlockedLevels() {
		if(Level1Unlocked!="1") lvl1.gameObject.SetActive(false);
		if(Level2Unlocked!="1") lvl2.gameObject.SetActive(false);
		if(Level3Unlocked!="1") lvl3.gameObject.SetActive(false);
		if(Level4Unlocked!="1") lvl4.gameObject.SetActive(false);
		if(Level5Unlocked!="1") lvl5.gameObject.SetActive(false);
	}
	void writeCommunicationFunction() {

		//Application.ExternalEval("alert(\"lololol\");");//var u = new UnityObject2();u.initPlugin(jQuery(\"#unityPlayer\")[0], \"buildLudum28.unity3d\");");
//		Application.ExternalEval("function SaySomethingToUnity(cvalue){u.getUnity().SendMessage(\"CookieManager\", \"ShowRead\", cvalue);}");
		
//		Application.ExternalEval("function getCookie(cname){var name = cname + \"=\";var test=false;var ca = document.cookie.split(';');for(var i=0; i<ca.length; i++){var c = ca[i].trim();if (c.indexOf(name)==0) {test=true;SaySomethingToUnity(c.substring(name.length,c.length));}}if(test==false)SaySomethingToUnity(\"NULL\");}");
		//Application.ExternalCall( "SaySomethingToUnity", "The game says hello!" );
	}
	void setCookie(string name, string value) {
//		Application.ExternalEval("document.cookie = \""+name+"="+value+"; \"");
	}
	void getCookie(string name) {
//		Application.ExternalCall( "getCookie", name );
	}
	public void ShowRead(string param)
	{
		//Application.ExternalEval("alert(\""+param+"\");");
		//textTest.text = param;
		_recoveredValue = param;
	}
}
