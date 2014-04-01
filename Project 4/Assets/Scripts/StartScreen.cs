using UnityEngine;
using System.Collections;

/*
	Possible format for reading in Robot parameters from file:

	N
	1 k11 k12 k21 k22 x y e
	2 k11 k12 k21 k22 x y e
	.
	.
	.
	N k11 k12 k21 k22 x y e

	"N" is number of Robots requested by user
	"k11".."k22" are the values of the K matrix
	"x" and "y" are starting position coordinates
	"e" represents end of line

*/

public static class SetupData
{
	public static string filename = string.Empty;
	public static string xCoord = string.Empty;
	public static string yCoord = string.Empty;
	public static string k11 = string.Empty;
	public static string k12 = string.Empty;
	public static string k21 = string.Empty;
	public static string k22 = string.Empty;
}

public class StartScreen : MonoBehaviour {

	public enum selectedControl {setText, /*setNum,*/ x, y, k11, k12, k21, k22, none};
	selectedControl focus = selectedControl.none;
//	public static string numBots = string.Empty;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		string input = getKeyboardInput ();
		if (input != string.Empty) {
			if (focus == selectedControl.setText) {
				if (input == "b")
				{
					if (SetupData.filename.Length > 0)
						SetupData.filename = SetupData.filename.Remove (SetupData.filename.Length-1,1);
				}
				else
				{
					SetupData.filename += input;
				}
			} /*else if (focus == selectedControl.setNum) {
				if (input == "b")
				{
					if (numBots.Length > 0)
						numBots = numBots.Remove (numBots.Length-1,1);
				}
				else
				{
					numBots += input;
				}*/
			} else if (focus == selectedControl.x) {
				if (input == "b")
				{
					if (SetupData.xCoord.Length > 0)
						SetupData.xCoord = SetupData.xCoord.Remove (SetupData.xCoord.Length-1,1);
				}
				else
				{
					SetupData.xCoord += input;
				}
			} else if (focus == selectedControl.y) {
				if (input == "b")
				{
					if (SetupData.yCoord.Length > 0)
						SetupData.yCoord = SetupData.yCoord.Remove (SetupData.yCoord.Length-1,1);
				}
				else
				{
					SetupData.yCoord += input;
				}
			} else if (focus == selectedControl.k11) {
				if (input == "b")
				{
					if (SetupData.k11.Length > 0)
						SetupData.k11 = SetupData.k11.Remove (SetupData.k11.Length-1,1);
				}
				else
				{
					SetupData.k11 += input;
				}
			} else if (focus == selectedControl.k12) {
				if (input == "b")
				{
					if (SetupData.k12.Length > 0)
						SetupData.k12 = SetupData.k12.Remove (SetupData.k12.Length-1,1);
				}
				else
				{
					SetupData.k12 += input;
				}
			} else if (focus == selectedControl.k21) {
				if (input == "b")
				{
					if (SetupData.k21.Length > 0)
						SetupData.k21 = SetupData.k21.Remove (SetupData.k21.Length-1,1);
				}
				else
				{
					SetupData.k21 += input;
				}
			} else if (focus == selectedControl.k22) {
				if (input == "b")
				{
					if (SetupData.k22.Length > 0)
						SetupData.k22 = SetupData.k22.Remove (SetupData.k22.Length-1,1);
				}
				else
				{
					SetupData.k22 += input;
				}
			}
		}
	}

	string getKeyboardInput()
	{
		string ret = string.Empty;
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			ret = "0";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha1)){
			ret = "1";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ret = "2";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha3)){
			ret = "3";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha4)){
			ret = "4";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			ret = "5";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha6)){
			ret = "6";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha7)){
			ret = "7";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha8)){
			ret = "8";
		}
		else if (Input.GetKeyDown (KeyCode.Alpha9)) {
			ret = "9";
		}
		else if (Input.GetKeyDown (KeyCode.Period)){
			ret = ".";
		}
		else if (Input.GetKeyDown (KeyCode.Backspace)){
			ret = "b";
		}
		else
		{
			ret = "";
		}
		return ret;
	}
	
	void OnGUI ()
	{
		GUI.TextArea (new Rect (0, 0, 160, 20), "Braitenberg Vehicles - Team Alpha");
		GUI.TextArea (new Rect (396, 300, 80, 20), "Upload from file");		// Change to button later
		GUI.TextArea (new Rect (346, 320, 100, 40), "Add robot");			// Change to button later
		// "Start Simulation" Button needed still


		// Need text boxes for all parameters
			// "Add Robot" button will create Robot with given parameters and clear all parameters for new Robot, should user decide to add another

		

		// Reference vvvvvvvvvvvvvvvvv
		/*	if (GUI.Button (new Rect (256,360,100,100), "START\n SERVER")) {
			NetworkData.isServer = true;
			Application.LoadLevel("PencilBot");
		}
		if (GUI.Button (new Rect (456,360,100,100), "START\n CLIENT")) {
			NetworkData.isServer = false;
			Application.LoadLevel("PencilBot");
		}
		if (GUI.Button (new Rect (456,300,160,20), NetworkData.ipAddressString)) {
			focus = selectedControl.ipAddress;
			inputTimeout = 5;
		}
		if (GUI.Button (new Rect (456,320,160,20), NetworkData.portString)) {
			focus = selectedControl.port;
			inputTimeout = 5;
		}*/
	}
}
