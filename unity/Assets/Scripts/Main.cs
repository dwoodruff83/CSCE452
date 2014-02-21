using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	static float d;
	static float theta3;
	static float theta4;

	static float LowerOffset = 2;
	static float MiddleOffset = 1f;
	static float UpperOffset = .8f;

	static Matrix SliderMatrix;
	static Matrix LowerArmMatrix;
	static Matrix MiddleArmMatrix;
	static Matrix UpperArmMatrix;

	static GameObject Slider;
	static GameObject LowerArm;
	static GameObject MiddleArm;
	static GameObject UpperArm;

	// Use this for initialization
	void Start () {
		d = 0;
		theta3 = 0;
		theta4 = 0;

		Slider = GameObject.Find ("Slider");
		LowerArm = GameObject.Find ("LowerArm");
		MiddleArm = GameObject.Find ("MiddleArm");
		UpperArm = GameObject.Find ("UpperArm");

		UpdateRobot ();
	}

	void OnGUI () {
		GUI.TextArea (new Rect (0, 0, 150, 55), "Movement Controls:\nCC = Counter-Clockwise\nC = Clockwise");

		if (GUI.Button (new Rect (10,60,60,60), "Upper\nCC")) {
			theta4 += 1;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (72,60,60,60), "Upper\nC")) {
			theta4 -= 1;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (10,122,60,60), "Middle\nCC")) {
			theta3 += 1;
			UpdateRobot ();
		}
		
		if (GUI.Button (new Rect (72,122,60,60), "Middle\nC")) {
			theta3 -= 1;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (10,246,60,60), "Slider\nLeft")) {
			d -= 1;
			UpdateRobot ();
		}
		
		if (GUI.Button (new Rect (72,246,60,60), "Slider\nRight")) {
			d += 1;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (10,306,60,60), "Paint")) {
			Paint (new Vector3( UpperArmMatrix[0][3], UpperArmMatrix[1][3]+UpperOffset, UpperArmMatrix[2][3]));
		}
	}

	// Update is called once per frame
	void Update () {
		//Handles movig the camera for debugging.
		if (Input.GetKey (KeyCode.LeftArrow))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-10*Time.deltaTime);
		}
		else if (Input.GetKey (KeyCode.RightArrow))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+10*Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.DownArrow))
		{
			transform.position = new Vector3(transform.position.x+10*Time.deltaTime, transform.position.y, transform.position.z);
		}
		else if (Input.GetKey (KeyCode.UpArrow))
		{
			transform.position = new Vector3(transform.position.x-10*Time.deltaTime, transform.position.y, transform.position.z);
		}
	}

	void SetTransform(Matrix a, GameObject b, float offset)
	{
		List<float> positionVec = a.GetColumn (3);
		Vector3 rotationVec = new Vector3 (a[0][0], a[1][1], a[2][2]);

		//X needs to be flipped to match up with our initial framing, and then X and Z get switched
		//To match up with Unity's framing;
		b.transform.position = new Vector3 (positionVec[0],positionVec[1],positionVec[2]);
		b.transform.eulerAngles = rotationVec;
	}

	void Paint(Vector3 paintLocation)
	{
		Instantiate (Resources.Load ("PaintDot"), paintLocation, new Quaternion ());
	}

	void UpdateRobot()
	{
		SliderMatrix = new Matrix(new float[] {90,0,0,0,0,1,0,0,0,0,1,d,0,0,0,1}, 4);
		LowerArmMatrix = new Matrix (new float[] {0,1,0,0,0,90,1,1.5f,1,0,0,0,0,0,0,1}, 4);
		MiddleArmMatrix = new Matrix (new float[] {
			Mathf.Cos (theta3),
			-1 * Mathf.Sin (theta3),
			0,
			0,
			Mathf.Sin (theta3),
			Mathf.Cos (theta3),
			0,
			4,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			1
		}, 4);
		UpperArmMatrix = new Matrix (new float[] {
			Mathf.Cos (theta3),
			-1 * Mathf.Sin (theta3),
			0,
			0,
			Mathf.Sin (theta4),
			Mathf.Cos (theta4),
			0,
			5.8f,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			1
		}, 4);

		SliderMatrix.Print ("Slider");
		SetTransform (SliderMatrix, Slider, 0f);

		LowerArmMatrix.Print ("LowerArm");
		LowerArmMatrix = SliderMatrix * LowerArmMatrix;
		SetTransform (LowerArmMatrix, LowerArm, 0f);

		MiddleArmMatrix.Print ("MiddleArm");
		MiddleArmMatrix = LowerArmMatrix * MiddleArmMatrix;
		SetTransform (MiddleArmMatrix, MiddleArm, 0);

		UpperArmMatrix.Print ("UpperArm");
		UpperArmMatrix = MiddleArmMatrix * UpperArmMatrix;
		SetTransform (UpperArmMatrix, UpperArm, 0);
	}
}