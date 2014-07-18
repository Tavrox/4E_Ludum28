using UnityEngine;
using System.Collections;

public class InputManager : ScriptableObject {

	public float X_AxisPos_Sensibility = 0.5f;
	public float X_AxisNeg_Sensibility = -0.5f;
	public float Y_AxisPos_Sensibility = 0.5f;
	public float Y_AxisNeg_Sensibility = -0.5f;

	[HideInInspector] public string Xpad_A = "joystick button 0";
	[HideInInspector] public string Xpad_B = "joystick button 1";
	[HideInInspector] public string Xpad_X = "joystick button 2";
	[HideInInspector] public string Xpad_Y = "joystick button 3";
	[HideInInspector] public string Xpad_LB = "joystick button 4";
	[HideInInspector] public string Xpad_RB = "joystick button 5";
	[HideInInspector] public string Xpad_BACK = "joystick button 6";
	[HideInInspector] public string Xpad_START = "joystick button 7";
	
	[HideInInspector] public KeyCode Up = KeyCode.UpArrow;
	[HideInInspector] public KeyCode Down = KeyCode.DownArrow;
	[HideInInspector] public KeyCode Left = KeyCode.LeftArrow;
	[HideInInspector] public KeyCode Right = KeyCode.RightArrow;
	[HideInInspector] public KeyCode Action =  KeyCode.E;
	[HideInInspector] public KeyCode Hold =  KeyCode.LeftShift;
	[HideInInspector] public KeyCode Reset =  KeyCode.Backspace;
	[HideInInspector] public KeyCode Reset2 =  KeyCode.R;
	[HideInInspector] public KeyCode Pause =  KeyCode.Escape;
	[HideInInspector] public KeyCode Up2 = KeyCode.Z;
	[HideInInspector] public KeyCode Down2 = KeyCode.S;
	[HideInInspector] public KeyCode Left2 = KeyCode.Q;
	[HideInInspector] public KeyCode Right2 = KeyCode.D;
	[HideInInspector] public KeyCode Action2 =  KeyCode.E;
	[HideInInspector] public KeyCode Action3 =  KeyCode.E;
	[HideInInspector] public KeyCode Hold2 =  KeyCode.A;
	[HideInInspector] public KeyCode Hold3 =  KeyCode.RightShift;
	[HideInInspector] public KeyCode Enter =  KeyCode.Return;
	
	[HideInInspector] public KeyCode Up3 = KeyCode.Space;
	[HideInInspector] public KeyCode PauseMenu =  KeyCode.Escape;
	
	[HideInInspector] public string PadAction;
	[HideInInspector] public string PadHold;
	[HideInInspector] public string PadReset;
	[HideInInspector] public string PadSkipDeath;
	[HideInInspector] public string PadPause;
	[HideInInspector] public string PadJump;

	public void Setup()
	{
		Xpad_A = "joystick button 0";
		Xpad_B = "joystick button 1";
		Xpad_X = "joystick button 2";
		Xpad_Y = "joystick button 3";
		Xpad_LB = "joystick button 4";
		Xpad_RB = "joystick button 5";
		Xpad_BACK = "joystick button 6";
		Xpad_START = "joystick button 7";
		
		PadJump = Xpad_A;
		PadAction = Xpad_X;
		PadHold = Xpad_B;
		PadReset = Xpad_BACK;
		PadSkipDeath = Xpad_BACK;
		PadPause = Xpad_START;
			
		Up = KeyCode.UpArrow;
		Down = KeyCode.DownArrow;
		Left = KeyCode.LeftArrow;
		Right = KeyCode.RightArrow;
		Action =  KeyCode.E;
		Hold =  KeyCode.LeftShift;
		Reset =  KeyCode.Backspace;
		Reset2 =  KeyCode.R;
		Pause =  KeyCode.Escape;
		Up2 = KeyCode.Z;
		Down2 = KeyCode.S;
		Left2 = KeyCode.Q;
		Right2 = KeyCode.D;
		Action2 =  KeyCode.E;
		Action3 =  KeyCode.E;
		Hold2 =  KeyCode.A;
		Hold3 =  KeyCode.RightShift;
		Enter =  KeyCode.Return;
		
		Up3 = KeyCode.Space;
		PauseMenu =  KeyCode.Escape;
	}

}
