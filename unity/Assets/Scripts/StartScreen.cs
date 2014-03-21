using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	public enum selectedControl {ipAddress, port, none};
	selectedControl focus = selectedControl.none;
	static bool IsServer = false;
	static string ipAddressString = string.Empty;
	static string portString = string.Empty;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetMouseButton (0)) {
		//	focus = selectedControl.none;
		//		}
		string input = getKeyboardInput ();
		if (getKeyboardInput() != string.Empty) {
			Debug.Log ("INSIDE");
						if (focus == selectedControl.ipAddress) {
				Debug.Log (input);
						} else if (focus == selectedControl.port) {
				Debug.Log (input);
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

		if (GUI.Button (new Rect (256,360,100,100), "START\n SERVER")) {

		}
		if (GUI.Button (new Rect (456,360,100,100), "START\n CLIENT")) {
			
		}
		if (GUI.Button (new Rect (456,320,160,20), ipAddressString)) {
			focus = selectedControl.ipAddress;
		}
		if (GUI.Button (new Rect (456,340,160,20), portString)) {
			focus = selectedControl.port;
		}
		}
}
