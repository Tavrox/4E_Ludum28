using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhpLeaderboards : MonoBehaviour
{
	private string secretKey = "lp4edges";

	public string addScoreURL = "http://4edges-games.com/games/lavapools/log.php";
	public string highscoreURL = "http://4edges-games.com/games/lavapools/display.php";

	public List<UserLeaderboard> ListUser;
	public List<LBEntry> ListEntries;
	private int lbLength =  15;
	
	void Awake()
	{
		ListUser = new List<UserLeaderboard>();
		ListEntries = new List<LBEntry>();
		for (int i = 0 ; i <= 14 ; i++) 
		{
			ListUser.Add(ScriptableObject.CreateInstance("UserLeaderboard") as UserLeaderboard);
		}
		for (int i = 1 ; i <= lbLength ; i++)
		{
			LBEntry _lb = FETool.findWithinChildren(gameObject, i.ToString()).GetComponent<LBEntry>();
			_lb.Setup();
			ListEntries.Add(_lb);
		}
		StartCoroutine(GetScores());
	}
	
	public void GatherScores()
	{
		StartCoroutine(GetScores());
		updateScores(ref ListUser, ref ListEntries);
	}

 	private void updateScores(ref List<UserLeaderboard> _listLB,ref List<LBEntry> _listEntries)
	{
		for (int i = 0 ; i <= 14 ; i++)
		{
			_listEntries[i].Setup();
			_listEntries[i].UpdateScore(_listLB[i].ranking.ToString(), _listLB[i].userBestScore.ToString(), _listLB[i].userName);
		}
	}

	public void SendScore(string _name, float _score, int _lvl)
	{
		string nm = _name;
		float sco = _score;
		int lvlID = _lvl;
		StartCoroutine(PostScores(nm, sco, _lvl));
	}
	
	// remember to use StartCoroutine when calling this function!
	IEnumerator PostScores(string name, float score, int level_id)
	{
		string hash = FETool.Md5Sum(name + score + secretKey);
		int prse = Mathf.RoundToInt(score);
		string post_url = 
			addScoreURL + 
				"?name=" + WWW.EscapeURL(name) +
				"&levelid=" + level_id.ToString() +
				"&score=" + prse.ToString() +
				"&hash=" + hash;

		WWW hs_post = new WWW(post_url);
		yield return hs_post;
		
		if (hs_post.error != null)
		{
			print("There was an error posting the high score: " + hs_post.error);
		}
		else
		{
			print ("Record envoyé"
			       + level_id.ToString() + "]["
			       + name  + "]["
			       + score + "]");
		}
	}
	
	// Get the scores from the MySQL DB to display in a GUIText.
	// remember to use StartCoroutine when calling this function!
	IEnumerator GetScores()
	{
		WWW hs_get = new WWW(highscoreURL);
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			print("There was an error getting the high score: " + hs_get.error);
		}
		else
		{
			string[] entries = hs_get.text.Split(']');
			for (int i = 0; i < entries.Length -1 ; i++)
			{
				ListUser[i].ranking = i+1;
				ListUser[i].userName = entries[i].Split('|')[0];
				ListUser[i].userBestScore = int.Parse(entries[i].Split('|')[1]);
//				Debug.Log (ListUser[i].ranking +""+ ListUser[i].userName +""+ ListUser[i].userBestScore);
			}
		}
	}

	
	public void sortLeaderboard(ref List<UserLeaderboard> _listLb, int length)
	{
		length = lbLength;
		
		_listLb.Sort(delegate (UserLeaderboard x, UserLeaderboard y)
		             {
			if (x.userBestScore > y.userBestScore) return -1;
			if (x.userBestScore < y.userBestScore) return 1;
			else return 0;
		});
		
		foreach (UserLeaderboard _usr in _listLb)
		{
			_usr.ranking = _listLb.IndexOf(_usr) + 1;
		}
		for (int i = 0; i <= length - 1; i++)
		{
			
		}
	}

	public void displayLeaderboard(List<UserLeaderboard> _listLb, ref TextUI[] _lines)
	{
		int _lengthLb = lbLength;
		if (_listLb.Count < _lengthLb )
		{
			Debug.LogError("There are not enough entries");
		}
		for (int i = 0; i <= _lengthLb - 1; i++)
		{
			_lines[i].text = _listLb[i].ranking + " - " + _listLb[i].userName + " - " + _listLb[i].userBestScore;
		}
	}
	
}