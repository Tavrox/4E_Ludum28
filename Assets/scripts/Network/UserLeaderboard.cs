using UnityEngine;
using System.Collections;

public class UserLeaderboard : ScriptableObject 
{
	public int userID;
	public string userName = "No Score";
	public int userBestScore = 0;
	public int userBestTime = 0;
	public int ranking = 0;
	public bool isCurrentPlayer;
}