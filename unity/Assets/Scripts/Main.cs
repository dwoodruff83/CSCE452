using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	static Matrix LowerArmMatrix;
	static Matrix MiddleArmMatrix;
	static Matrix UpperArmMatrix;

	static Component LowerArm;
	static Component MiddleArm;
	static Component UpperArm;

	// Use this for initialization
	void Start () {
		LowerArm = GetComponent ("LowerArm");
		MiddleArm = GetComponent ("MiddleArm");
		UpperArm = GetComponent ("UpperArm");
	}

	void OnGUI () {
		GUI.TextArea (new Rect (0, 0, 150, 55), "Movement Controls:\nCC = Counter-Clockwise\nC = Clockwise");

		if (GUI.Button (new Rect (10,60,60,60), "Upper\nCC")) {
			print ("You clicked the button!");
		}

		if (GUI.Button (new Rect (72,60,60,60), "Upper\nC")) {
			print ("You clicked the button!");
		}

		if (GUI.Button (new Rect (10,122,60,60), "Middle\nCC")) {
			print ("You clicked the button!");
		}
		
		if (GUI.Button (new Rect (72,122,60,60), "Middle\nC")) {
			print ("You clicked the button!");
		}

		if (GUI.Button (new Rect (10,184,60,60), "Lower\nCC")) {
			print ("You clicked the button!");
		}
		
		if (GUI.Button (new Rect (72,184,60,60), "Lower\nC")) {
			print ("You clicked the button!");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}