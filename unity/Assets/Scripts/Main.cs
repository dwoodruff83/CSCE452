﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class Main : MonoBehaviour {

	static bool paintToggle = false;
	static bool delayToggle = false;

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

	static GameObject Pivot1;
	static GameObject Pivot2;

	static GameObject Slider;
	static GameObject LowerArm;
	static GameObject MiddleArm;
	static GameObject UpperArm;

	static Color sliderColor;
	static Color pivot1Color;
	static Color pivot2Color;

	static int currentJoint;
	static string delayIndicator = "Delay Off";
	static string paintIndicator = "Paint Off";
	static string inverseCoordinates = string.Empty;

	const string topCCW = "topCCW";
	const string topCW  = "topCW";
	const string midCCW = "midCCW";
	const string midCW  = "midCW";
	const string sldLT  = "sldrLT";
	const string sldRT  = "sldrRT";
	const string Xminus = "x-";
	const string Xplus  = "x+";
	const string Yminus = "y-";
	const string Yplus  = "y+";
	const string quit = "q";
	const string clear = "c";
	const string pnt = "p";

	//-> Client / Server variables
	static bool isServer;
	static string ipAddressString;
	static string portString;
	TcpClient tcpClient;
	int timeDelay = 2;

	TcpListener listener;
	Socket sock;

  	void ChangeJointColor()
	{
		Slider.transform.renderer.material.color = sliderColor;
		Pivot1.transform.renderer.material.color = pivot1Color;
		Pivot2.transform.renderer.material.color = pivot2Color;
		
		if (currentJoint == 0) {
			Slider.transform.renderer.material.color = Color.green;
		}
		else if (currentJoint == 1) {
			Pivot1.transform.renderer.material.color = Color.green;
		}
		else if (currentJoint == 2) {
			Pivot2.transform.renderer.material.color = Color.green;
		}
	}

	void SendCommand(string command) {
		Stream stream = tcpClient.GetStream ();
		ASCIIEncoding asciiEncoding = new ASCIIEncoding ();
		byte[] byt = asciiEncoding.GetBytes (command);
		stream.Write (byt,0,byt.Length);
	}

	//get local IP address of server
	public string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}
		return localIP;
	}

	// Use this for initialization
	void Start () {
		//THIS IS HOW YOU ACCESS THE INFORMATION ACQUIRED FROM THE FIRST SCENE.
		//Debug.Log (NetworkData.isServer);
		//Debug.Log (NetworkData.ipAddressString);
		//Debug.Log (NetworkData.portString);

		isServer = NetworkData.isServer;
		ipAddressString = NetworkData.ipAddressString;
		portString = NetworkData.portString;
	
		d = 0;
		theta3 = 0;
		theta4 = 0;
		currentJoint = 0; // default selected movement is the slider

		Slider = GameObject.Find ("Slider");
		LowerArm = GameObject.Find ("LowerArm");
		MiddleArm = GameObject.Find ("MiddleArm");
		UpperArm = GameObject.Find ("UpperArm");

		Pivot1 = GameObject.Find ("Pivot1");
		Pivot2 = GameObject.Find ("Pivot2");
		
		sliderColor = Slider.transform.renderer.material.color;
		pivot1Color = Pivot1.transform.renderer.material.color;
		pivot2Color = Pivot2.transform.renderer.material.color;

		ChangeJointColor ();
		UpdateRobot ();
		inverseCoordinates = string.Format("Paint Brush Coords: \n X: {0} \n Y: {1}",Link3TopVec[2][0],Link3TopVec[1][0]);

		//string localIP;

		if (isServer) {
			Debug.Log ("isServer");
			//init server
			int port = 80;
			string ip = "10.9.66.212";//LocalIPAddress();
			IPAddress ipAddr = IPAddress.Parse( ip );
			//int.TryParse( portString, port );
			listener = new TcpListener( ipAddr, port );
			listener.Start ();
			sock = listener.AcceptSocket();
			//ServerStuff ();


		} else {
			//init client
			tcpClient.Connect(ipAddressString,int.Parse(portString));
		}

	}


	void DoAction (string action) {
		if (!isServer) {
			//
			//  add delay here
			//
		}
		switch (action) {
		case topCCW:
			theta4 -= .1f;
			UpdateRobot ();
			break;
		case topCW:
			theta4 += .1f;
			UpdateRobot ();
			break;
		case midCCW:
			theta3 -= .1f;
			UpdateRobot ();
			break;
		case midCW:
			theta3 += .1f;
			UpdateRobot ();
			break;
		case sldLT:
			if (d > 0f)
				d -= .1f;
			UpdateRobot ();
			break;
		case sldRT:
			if (d < 9.8f)
				d += .1f;
			UpdateRobot ();
			break;
		case Xminus:
			InverseKineUpdate (Link3TopVec [2] [0] - .05f, Link3TopVec [1] [0]);
			break;
		case Xplus:
			InverseKineUpdate (Link3TopVec [2] [0] + .05f, Link3TopVec [1] [0]);
			break;
		case Yminus:
			InverseKineUpdate (Link3TopVec [2] [0], Link3TopVec [1] [0] - .05f);
			break;
		case Yplus:
			InverseKineUpdate (Link3TopVec [2] [0], Link3TopVec [1] [0] + .05f);
			break;
		case quit:
			// add quit button
			//tcpClient.Close();
			sock.Close ();
			listener.Stop ();
			break;
		case clear:
			// add clear button maybe
			break;
		case pnt:
			paintToggle = !paintToggle;
			if (paintToggle) {
				paintIndicator = "Paint On";
			} 
			else {
				paintIndicator = "Paint Off";
			}
			Paint ();
			break;
		default:
			break;
		}
	}

	void OnGUI () {
		if (!isServer) {
			GUI.TextArea (new Rect (0, 0, 150, 55), "Movement Controls:\nCC = Counter-Clockwise\nC = Clockwise");
			//GUI.TextArea (new Rect (600, 0, 225, 65), "Keyboard Controls:\nUp/Down Arrows to Toggle Joint\nLeft/Right Arrows to rotate or slide\nHold Spacebar to paint");
			GUI.TextArea (new Rect (600, 0, 225, 65), "Keyboard Controls:\nUp/Down Arrows move in Y direction\nLeft/Right Arrows move in X direction\nHold Spacebar to paint");
			if (GUI.Button (new Rect (10, 60, 60, 60), "Upper\nCC")) {
				DoAction (topCCW);
				SendCommand (topCCW);
			}

			if (GUI.Button (new Rect (72, 60, 60, 60), "Upper\nC")) {
				DoAction (topCW);
				SendCommand (topCW);
			}

			if (GUI.Button (new Rect (10, 122, 60, 60), "Middle\nCC")) {
				DoAction (midCCW);
				SendCommand (midCCW);
			}
			
			if (GUI.Button (new Rect (72, 122, 60, 60), "Middle\nC")) {
				DoAction (midCW);
				SendCommand (midCW);
			}

			if (GUI.Button (new Rect (10, 184, 60, 60), "Slider\nLeft")) {
				DoAction (sldLT);
				SendCommand (sldLT);
			}
			
			if (GUI.Button (new Rect (72, 184, 60, 60), "Slider\nRight")) {
				DoAction (sldRT);
				SendCommand(sldRT);
			}

			if (GUI.Button (new Rect (10, 246, 80, 60), paintIndicator)) {
				DoAction (pnt);
				SendCommand(pnt);
			}

			if (GUI.Button (new Rect (90, 246, 80, 60), delayIndicator)) {
				delayToggle = !delayToggle;
				if(delayToggle)
				{
					delayIndicator = "Delay On";
				}
				else
				{
					delayIndicator = "Delay Off";
				}
			}

			GUI.TextArea (new Rect (160, 0, 150, 55), inverseCoordinates);
			//X is Z in our case
			if (GUI.Button (new Rect (10, 308, 60, 60), "X-")) {
				DoAction (Xminus);
				SendCommand (Xminus);
			}

			if (GUI.Button (new Rect (72, 308, 60, 60), "X+")) {
				DoAction(Xplus);
				SendCommand (Xplus);
			}

			if (GUI.Button (new Rect (10, 370, 60, 60), "Y-")) {
				DoAction(Yminus);
				SendCommand(Yminus);
			}
			
			if (GUI.Button (new Rect (72, 370, 60, 60), "Y+")) {
				DoAction (Yplus);
				SendCommand (Yplus);
			}
			// display client info
		} else {
			// display server info
		}
	}

	void Update () {
		if (!isServer) { // controls disabled for server
			if (Input.GetKey ("1")) {
				currentJoint = 0;
				ChangeJointColor ();
			}
			if (Input.GetKey ("2")) {
				currentJoint = 1;
				ChangeJointColor ();
			}
			if (Input.GetKey ("3")) {
				currentJoint = 2;
				ChangeJointColor ();
			}
			if (Input.GetKey ("a")) {
				switch (currentJoint) {
				case 0:
					DoAction (sldLT);
					break;
				case 1:
					DoAction (midCCW);
					break;
				case 2:
					DoAction (topCCW);
					break;
				}
			}
			if (Input.GetKey ("d")) {
				switch (currentJoint) {
				case 0:
					DoAction (sldRT);
					break;
				case 1:
					DoAction (midCW);
					break;
				case 2:
					DoAction (topCW);
					break;
				
				}
			}

			if (Input.GetKey (KeyCode.UpArrow))
				DoAction (Yplus);
			if (Input.GetKey (KeyCode.DownArrow))
				DoAction (Yminus);
			if (Input.GetKey (KeyCode.LeftArrow))
				DoAction (Xminus);
			if (Input.GetKey (KeyCode.RightArrow))
				DoAction (Xplus);
			if (Input.GetKey (KeyCode.Space))
				DoAction (pnt);
		}
		/*
		else
		{
			if (sock.Connected) {
				byte[] b = new byte[100];
				int k = sock.Receive (b);
				
				string command = k.ToString();
				DoAction( command );
				
				if( command == quit )
					DoAction( command ); 
			}

		}
		*/
	}

	float GetAngle(Matrix Top, Matrix Mid)
	{
		return Mathf.Rad2Deg*Mathf.Atan2 (Top [2] [0] - Mid [2] [0],Top [1] [0] - Mid [1] [0]);
	}

	void SetTransform(Matrix a, GameObject b, Matrix LinkMid, Matrix LinkTop)
	{
		Matrix positionMatrix = a * LinkMid;

		Matrix positionVec = positionMatrix.GetColumnM (0);
		positionVec [1] [0] *= -1;

		Vector3 rotationVec = new Vector3(0,90,0);

		if (b == Slider)
		{
			rotationVec[2] = 90;
		} 
		else
		{
			float z = 0;

			if (LinkTop != null)
			{
				z = GetAngle (LinkTop, positionVec);
			}

			rotationVec = new Vector3 (0,90,z);
		}

		//X needs to be flipped to match up with our initial framing, and then X and Z get switched
		//To match up with Unity's framing;
		b.transform.position = new Vector3 (positionVec[0][0],positionVec[1][0],positionVec[2][0]);
		b.transform.localEulerAngles = rotationVec;
	}

	void Paint()
	{
		Instantiate (Resources.Load ("PaintDot"), new Vector3(Link3TopVec[0][0]-.5f, Link3TopVec[1][0], Link3TopVec[2][0]), new Quaternion ());
	}

	void InverseKineUpdate(float Zn, float Yn)
	{
		float L1 = 3;
		float L2 = 2;
		float L3 = 1.6f;
		if (Yn > 6.6f) {
						Yn = 6.6f;
				} else if (Yn < L1 - L2 + L3)
			{
			Yn = L1 - L2 + L3;
				}
		float orgZn = Link1TopVec[2][0];
		float orgYn = Link1TopVec[1][0];
		float FirstZn = Link3TopVec [2] [0];

		float NewDistance = Mathf.Sqrt ((Zn - orgZn) * (Zn - orgZn) + (Yn - orgYn) * (Yn - orgYn));

		/*if (NewDistance > 3.6f) 
		{
			if (Zn < orgZn)
			{
				d -= Mathf.Abs(Zn - FirstZn);
			}
			else if (Zn > orgZn)
			{
				d += Mathf.Abs(Zn - FirstZn);
			}
		}*/

		float newT3 = theta3;
		float newT4 = theta4;

		//newT4 = Mathf.Acos (((Zn-d)*(Zn-d) + (Yn - L1)*(Yn - L1) - L2*L2 - L3*L3)/(2*L2*L3));
		//float r = Mathf.Sqrt ((Mathf.Cos (newT4) * L3 + L2) * (Mathf.Cos (newT4) * L3 + L2) + (Yn - L1) * (Yn - L1));
		//float alpha = Mathf.Atan2 (Yn - L1, Zn - d);
		//float gamma = Mathf.Atan2 (L3 * Mathf.Sin (newT4), L2 + L3 * Mathf.Cos (newT4));
		//newT3 = Mathf.PI / 2 + gamma - alpha;

		float alpha = Mathf.PI/2;
		newT3 = (Mathf.Acos ((Yn - L1 - L3*Mathf.Cos (Mathf.PI/2 - alpha)) / L2));
		newT4 = (Mathf.PI / 2 - newT3 - alpha);
		d = Zn - L2 * Mathf.Sin (newT3) - L3 * Mathf.Sin (newT3 + newT4);

		if (!float.IsNaN (newT3) && !float.IsNaN (newT4))
		{
			theta3 = newT3;
			theta4 = newT4;
		}

		UpdateRobot ();
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

		Matrix LowerArmMatrix = T01Matrix;

		Matrix MiddleArmMatrix = T01Matrix * T12Matrix;

		Matrix UpperArmMatrix = MiddleArmMatrix*T23Matrix;

		Link1TopVec = (T01Matrix * Link1TopM);
		Link1TopVec[1][0] *= -1;
		Link2TopVec = (MiddleArmMatrix * Link2TopM);
		Link2TopVec[1][0] *= -1;
		Link3TopVec = (UpperArmMatrix * Link3TopM);
		Link3TopVec[1][0] *= -1;

		SetTransform (T01Matrix, Slider, ZeroMatrix, null);
		SetTransform (LowerArmMatrix, LowerArm, Link1CenterM, null);
		SetTransform (MiddleArmMatrix, MiddleArm, Link2CenterM, Link2TopVec);
		SetTransform (UpperArmMatrix, UpperArm, Link3CenterM, Link3TopVec);

		inverseCoordinates = string.Format("Paint Brush Coords: \n X: {0} \n Y: {1}",Link3TopVec[2][0],Link3TopVec[1][0]);

		if (paintToggle) 
		{
			Paint ();
		}
		print (UpperArm.transform.eulerAngles.z);
	}
}