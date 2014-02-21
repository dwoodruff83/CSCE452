using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	static bool paintToggle = false;

	static float d;
	static float theta3;
	static float theta4;

	static Matrix ZeroMatrix = new Matrix(new float[] { 0f, 0, 0,1}, 1);
	static Matrix Link1CenterM = new Matrix(new float[] { 1.5f, 0, 0,1}, 1);
	static Matrix Link2CenterM = new Matrix(new float[] { 1f, 0, 0,1}, 1);
	static Matrix Link3CenterM = new Matrix(new float[] { .8f, 0, 0,1}, 1);

	static Matrix Link1TopM = new Matrix(new float[] { 3f,0,0,1}, 1);
	static Matrix Link2TopM = new Matrix(new float[] { 2f,0,0,1}, 1);
	static Matrix Link3TopM = new Matrix(new float[] { 1.6f,0,0,1}, 1);

	static Matrix Link1TopVec;
	static Matrix Link2TopVec;
	static Matrix Link3TopVec;

	static Matrix T01Matrix;
	static Matrix T12Matrix;
	static Matrix T23Matrix;
	static Matrix T34Matrix;

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
			theta4 += .1f;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (72,60,60,60), "Upper\nC")) {
			theta4 -= .1f;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (10,122,60,60), "Middle\nCC")) {
			theta3 += .1f;
			UpdateRobot ();
		}
		
		if (GUI.Button (new Rect (72,122,60,60), "Middle\nC")) {
			theta3 -= .1f;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (10,184,60,60), "Slider\nLeft")) {
			d -= .1f;
			UpdateRobot ();
		}
		
		if (GUI.Button (new Rect (72,184,60,60), "Slider\nRight")) {
			d += .1f;
			UpdateRobot ();
		}

		if (GUI.Button (new Rect (10,246,60,60), "Paint")) {
			paintToggle = !paintToggle;
			Paint ();
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

	void SetTransform(Matrix a, GameObject b, Matrix LinkMid, Matrix LinkTop)
	{
		Matrix positionMatrix = a * link;

		List<float> positionVec = positionMatrix.GetColumn (0);
		Vector3 rotationVec;

		if (b == Slider)
		{
			rotationVec= new Vector3 (0, 90, 90);
		} 
		else
		{


			rotationVec = new Vector3 ();
		}

		//X needs to be flipped to match up with our initial framing, and then X and Z get switched
		//To match up with Unity's framing;
		b.transform.position = new Vector3 (positionVec[0],(-1*positionVec[1]),positionVec[2]);
		b.transform.eulerAngles = rotationVec;
	}

	void Paint()
	{
		//Instantiate (Resources.Load ("PaintDot"), new Vector3(Link3TopM[0][0], -1*Link3TopM[0][1], Link3TopM[0][2]), new Quaternion ());
	}

	void UpdateRobot()
	{
		T01Matrix = new Matrix(new float[] {0,1,0,0,
											-1,0,0,0,
											0,0,1,d,
											0,0,0,1}	, 4);

		T12Matrix = new Matrix (new float[] {Mathf.Cos (theta3),-1*Mathf.Sin (theta3),0,3,
											 0,0,-1,0,
											 Mathf.Sin (theta3),Mathf.Cos (theta3),0,0,
											 0,0,0,1}	, 4);
		T23Matrix = new Matrix (new float[] {
											Mathf.Cos (theta4),-1 * Mathf.Sin (theta4),0,2,
											Mathf.Sin (theta4),Mathf.Cos (theta4),0,0,
											0,0,1,0,
											0,0,0,1}	, 4);
		T34Matrix = new Matrix (new float[] {
											0,-1,0,1.6f,
											0,0,1,0,
											-1,0,0,0,
											0,0,0,1 }	, 4);

		T01Matrix.Print ("T-01");
		T12Matrix.Print ("T-12");
		T23Matrix.Print ("T-23");
		T34Matrix.Print ("T-34");

		SetTransform (T01Matrix, Slider, ZeroMatrix);

		Matrix LowerArmMatrix = T01Matrix;

		Matrix MiddleArmMatrix = T01Matrix * T12Matrix;

		Matrix UpperArmMatrix = MiddleArmMatrix*T23Matrix;

		Link1TopVec = (T01Matrix * Link1TopM).GetColumn (0);
		Link1TopVec[0][1] *= -1;
		Link2TopVec = (MiddleArmMatrix * Link2TopM).GetColumn (0);
		Link2TopVec[0][1] *= -1;
		Link3TopVec = (UpperArmMatrix * Link3TopM).GetColumn (0);
		Link3TopVec[0][1] *= -1;

		SetTransform (LowerArmMatrix, LowerArm, Link1CenterM, Link1TopVec);
		SetTransform (MiddleArmMatrix, MiddleArm, Link2CenterM, Link2TopVec);
		SetTransform (UpperArmMatrix, UpperArm, Link3CenterM, Link3TopVec);

		if (paintToggle) 
		{
			Paint ();
		}
	}
}