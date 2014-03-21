using UnityEngine;
using System.Collections;

public static class NetworkData
{
	public static bool isServer = false;
	public static string ipAddressString = string.Empty;
	public static string portString = string.Empty;
}

public class StartScreen : MonoBehaviour {

	public enum selectedControl {ipAddress, port, none};
	selectedControl focus = selectedControl.none;
	float inputTimeout = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (inputTimeout > 0) {
			inputTimeout -= Time.deltaTime;
		} else if (inputTimeout <= 0) {
			focus = selectedControl.none;
		}
		string input = getKeyboardInput ();
		if (input != string.Empty) {
			inputTimeout = 2;
						if (focus == selectedControl.ipAddress) {
							if (input == "b")
							{
								if (NetworkData.ipAddressString.Length > 0)
									NetworkData.ipAddressString = NetworkData.ipAddressString.Remove (NetworkData.ipAddressString.Length-1,1);
							}
							else
							{
								NetworkData.ipAddressString += input;
							}
						} else if (focus == selectedControl.port) {
							if (input == "b")
							{
								if (NetworkData.portString.Length > 0)
									NetworkData.portString = NetworkData.portString.Remove (NetworkData.portString.Length-1,1);
							}
							else
							{
								NetworkData.portString += input;
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
	void OnGUI () {


		GUI.TextArea (new Rect (0, 0, 160, 20), "PencilBot - Team Alpha");
		GUI.TextArea (new Rect (376, 300, 80, 20), "IPAddress:");
		GUI.TextArea (new Rect (356, 320, 100, 40), "Port Number: \nClick and Type");

		if (GUI.Button (new Rect (256,360,100,100), "START\n SERVER")) {
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
		}
		}
}
