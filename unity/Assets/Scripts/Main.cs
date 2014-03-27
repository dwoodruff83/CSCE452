using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;

public class Main : MonoBehaviour {

	static bool paintToggle = false;
	static bool delayToggle = false;
	static bool Quit = false;

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

	object serverLock = new object();
	ThreadStart server;
	Thread thread;

	static List<string> ServerQueue = new List<string>();
	//-> Client / Server variables
	static bool isServer;
	static string ipAddressString;
	static string portString;
	static int packets;
	static string packetString = "0";
	IPAddress ipAddr;
	TcpClient tcpClient;
	int timeDelay = 2;
	string status;

	TcpListener listener;
	Socket sock;

	Vector2 scrollPosition;

	void StartServer() {
		//init server
		try {
			ipAddressString = NetworkData.ipAddressString;
			//ipAddressString = "127.0.0.1";
			portString = NetworkData.portString;
			//portString = "8081";
			status = string.Format ("Local IP: {0}", ipAddressString);
			status += "\nAwating connection to client...";
			status += string.Format ("\nListening on port: {0}", portString);
			ipAddr = IPAddress.Parse(ipAddressString);
			int port = int.Parse(portString);
			listener = new TcpListener( ipAddr, port);
			listener.Start ();

			Socket s = listener.AcceptSocket ();
			status += string.Format("\nSocket {0} accepted.", s.SocketType);
			ASCIIEncoding encoder = new ASCIIEncoding();
			while (!Quit)
			{
				byte[] b = new byte[100];
				int k = s.Receive(b);
				
				string command = string.Empty;

				for(int i =0; i <k; i++)
				{
					command += ( Convert.ToChar(b[i]));
				}

				if (command != string.Empty)
				{
					command = command.Trim();
					status += string.Format("\nReceived command: {0}",command);
					lock (serverLock)
					{
						ServerQueue.Add( command );
					}
				packets++;
				packetString = string.Format( "{0}", packets );
				}

				s.Send (encoder.GetBytes("Received"));
				status += "\nConfirmation sent.";
			}
			Application.Quit();
		}
		catch (Exception e) {
			Debug.Log(e);
			status += ("Exception: {0}" + e);
		}			
	}

	// Use this for initialization
	void Start () {
		//THIS IS HOW YOU ACCESS THE INFORMATION ACQUIRED FROM THE FIRST SCENE.
		//Debug.Log (NetworkData.isServer);
		//Debug.Log (NetworkData.ipAddressString);
		//Debug.Log (NetworkData.portString);
		
		isServer = NetworkData.isServer;
		ipAddressString = NetworkData.ipAddressString;
		//ipAddressString = "127.0.0.1";
		portString = NetworkData.portString;
		//portString = "8081";
		
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
		
		if (isServer) {
			server = new ThreadStart(this.StartServer);
			thread = new Thread(server);
			thread.Start ();
		} else {
			//init client
			//Debug.Log ("isClient");
			status = "Awating connection to server...";
			try {
				tcpClient = new TcpClient(ipAddressString,int.Parse(portString));
				if(tcpClient.Connected){
					status = "Connected to server.";
					status += string.Format("\nIP Address: {0}", ipAddressString);
					status += string.Format("\n   on port: {0}", portString);

				}
				else {
					status = "Couldn't connect.\n";
				}
			}
			catch (ArgumentNullException e) {
				status += ("\nArgumentNullException: = " + e);
			} 
			catch (SocketException e) {
				status += ("\nSocketException: {0}" + e);
			}
		}
		
	}

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
		if (tcpClient.Connected) {

			Debug.Log ("Command: " + command);
			Stream stream = tcpClient.GetStream ();
			ASCIIEncoding asciiEncoding = new ASCIIEncoding ();
			byte[] byt = asciiEncoding.GetBytes(command);
			foreach (byte a in byt) {
					Debug.Log (Convert.ToChar(a));
			}
			stream.Write (byt, 0, byt.Length);					
			packets++;
			packetString = string.Format( "{0}", packets );

			byte[] bb = new byte[100];
			int k = stream.Read(bb,0,100);
			if(Quit) {
				Application.Quit();
			}
		}
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



	IEnumerator DelayAction( string action )
	{
		if (delayToggle)
			yield return new WaitForSeconds (timeDelay);
		DoAction (action);
		SendCommand (action);
		status += string.Format ("\nSent command {0}: {1}", packetString, action);
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
			Quit = true;
			tcpClient.Close();
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
			if (GUI.Button (new Rect (10, 60, 60, 60), "Upper\nCC"))
					StartCoroutine (DelayAction (topCCW));
			if (GUI.Button (new Rect (72, 60, 60, 60), "Upper\nC"))
					StartCoroutine (DelayAction (topCW));
			if (GUI.Button (new Rect (10, 122, 60, 60), "Middle\nCC"))
					StartCoroutine (DelayAction (midCCW));			
			if (GUI.Button (new Rect (72, 122, 60, 60), "Middle\nC"))
					StartCoroutine (DelayAction (midCW));
			if (GUI.Button (new Rect (10, 184, 60, 60), "Slider\nLeft"))
					StartCoroutine (DelayAction (sldLT));			
			if (GUI.Button (new Rect (72, 184, 60, 60), "Slider\nRight"))
					StartCoroutine (DelayAction (sldRT));
			if (GUI.Button (new Rect (10, 246, 80, 60), paintIndicator))
					StartCoroutine (DelayAction (pnt));
			if (GUI.Button (new Rect (90, 246, 80, 60), delayIndicator)) {
					delayToggle = !delayToggle;
					if (delayToggle)
							delayIndicator = "Delay On";
					else
							delayIndicator = "Delay Off";			
			}
			//X is Z in our case
			if (GUI.Button (new Rect (10, 308, 60, 60), "X-"))
				StartCoroutine (DelayAction (Xminus));
			if (GUI.Button (new Rect (72, 308, 60, 60), "X+"))
				StartCoroutine (DelayAction (Xplus));
			if (GUI.Button (new Rect (10, 370, 60, 60), "Y-"))
				StartCoroutine (DelayAction (Yminus));
			if (GUI.Button (new Rect (72, 370, 60, 60), "Y+"))
				StartCoroutine (DelayAction (Yplus));
		}

		if (GUI.Button (new Rect (Screen.width - 375, 0, 75, 75), "QUIT"))
			StartCoroutine (DelayAction (quit));
		// display client info
		GUILayout.BeginArea (new Rect(Screen.width-300,0,300,300));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width (300), GUILayout.Height (300));
		GUI.skin.box.wordWrap = true; 
		GUILayout.TextArea(status);
		GUILayout.EndScrollView ();
		GUILayout.EndArea();
		GUI.TextArea (new Rect (160, 0, 150, 55), inverseCoordinates);
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
					StartCoroutine( DelayAction( sldLT ) );
					break;
				case 1:
					StartCoroutine( DelayAction( midCCW ) );
					break;
				case 2:
					StartCoroutine( DelayAction( topCCW ) );
					break;
				}
			}
			if (Input.GetKey ("d")) {
				switch (currentJoint) {
				case 0:
					StartCoroutine( DelayAction( sldRT ) );
					break;
				case 1:
					StartCoroutine( DelayAction( midCW ) );
					break;
				case 2:
					StartCoroutine( DelayAction( topCW ) );
					break;
				
				}
			}

			if (Input.GetKey (KeyCode.UpArrow))
				StartCoroutine( DelayAction( Yplus ) );
			if (Input.GetKey (KeyCode.DownArrow))
				StartCoroutine( DelayAction( Yminus ) );
			if (Input.GetKey (KeyCode.LeftArrow))
				StartCoroutine( DelayAction( Xminus ) );
			if (Input.GetKey (KeyCode.RightArrow))
				StartCoroutine( DelayAction( Xplus ) );
			if (Input.GetKey (KeyCode.Space))
				StartCoroutine( DelayAction( pnt ) );
		}
		else
		{
			lock (serverLock)
			{
				while(ServerQueue.Count > 0)
				{
					DoAction (ServerQueue[0]);
					ServerQueue.RemoveAt(0);
				}
			}
		}
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

		//T01Matrix.Print ("T-01");
		//T12Matrix.Print ("T-12");
		//T23Matrix.Print ("T-23");
		//T34Matrix.Print ("T-34");

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
		//print (UpperArm.transform.eulerAngles.z);
	}
}