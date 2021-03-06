﻿using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
	
	public static event GameEvent GameStart, GamePause, GameUnpause, GameOver, NextInstance, NextLevel, PreviousLevel, FinishLevel;
	public static bool gamePaused = false;
	
	public static void TriggerGameStart(){
		if(GameStart != null){					
			GameStart();
		}
	}

	public static void TriggerGameOver(){
		if(GameOver != null){
			GameOver();
		}
	}
	public static void TriggerNextInstance(){
		if(NextInstance != null){
			NextInstance();
		}
	}
	public static void TriggerFinishLevel(){
		if(FinishLevel != null){
			FinishLevel();
		}
	}
	
	public static void TriggerNextLevel(){
		if(NextLevel != null){
			NextLevel();
		}
	}
	public static void TriggerPreviousLevel(){
		if(PreviousLevel != null){
			PreviousLevel();
		}
	}
	public static void TriggerGamePause()
	{
		if(GamePause != null)
		{

			GamePause();
		}
	}
	public static void TriggerGameUnpause()
	{
		if(GameUnpause != null)
		{
			gamePaused = false;
			GameUnpause();
		}
	}
}
