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
	[HideInInspector] public KeyCode Action =  KeyCode.Return;
	[HideInInspector] public KeyCode Hold =  KeyCode.Return;
	[HideInInspector] public KeyCode Reset =  KeyCode.Backspace;
	[HideInInspector] public KeyCode Pause =  KeyCode.Escape;

	[HideInInspector] public string PadAction;
	[HideInInspector] public string PadHold;
	[HideInInspector] public string PadReset;
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
		PadAction = Xpad_B;
		PadHold = Xpad_RB;
		PadReset = Xpad_START;
	}

}
