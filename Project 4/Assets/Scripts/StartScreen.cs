using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

public class Light
{
	public int m_id;
	public float m_x, m_y, m_intensity;

	public Light(int id, float x, float y, float intensity)
	{
		m_id = id;
		m_x = x;
		m_y = y;
		m_intensity = intensity;
	}
}

public class Robot
{
	public int m_id;
	public float m_x, m_y, m_k11, m_k12, m_k21, m_k22;

	public Robot(int id ,float x, float y, float k11, float k12, float k21, float k22)
	{
		m_id = id;
		m_x = x;
		m_y = y;
		m_k11 = k11;
		m_k12 = k12;
		m_k21 = k21;
		m_k22 = k22;
	}
}

public static class SetupData
{
	public static List<Light> lightList = new List<Light>();
	public static List<Robot> robotList = new List<Robot>();
}

public class StartScreen : MonoBehaviour {

	public static string filename = string.Empty;
	public static string xCoord = string.Empty;
	public static string yCoord = string.Empty;
	public static string k11 = string.Empty;
	public static string k12 = string.Empty;
	public static string k21 = string.Empty;
	public static string k22 = string.Empty;
	public static string xlight = string.Empty;
	public static string ylight = string.Empty;
	public static string intensity = string.Empty;

	public enum selectedControl {setText, xbot, ybot, k11, k12, k21, k22, xlight, ylight, intensity, none};
	selectedControl focus = selectedControl.none;
//	public static string numBots = string.Empty;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		string input = getKeyboardInput ();
		if (input != string.Empty) 
		{
			if (focus == selectedControl.setText) {
				if (input == "\b")
				{
					if (filename.Length > 0)
					{
						filename = filename.Remove(filename.Length-1,1);
					}
				}
				else
				{
					filename += input;
				}
			} else if (focus == selectedControl.intensity) {
				if (input == "\b")
				{
					if (intensity.Length > 0)
						intensity = intensity.Remove (intensity.Length-1,1);
				}
				else
				{
<<<<<<< HEAD
					intensity += input;
				}
			} else if (focus == selectedControl.xbot) {
				if (input == "\b")
=======
					numBots += input;
				}
			}*/ else if (focus == selectedControl.x) {
				if (input == "b")
>>>>>>> 39dc474133cff02730ec8aa687af693ac00f49cd
				{
					if (xCoord.Length > 0)
						xCoord = xCoord.Remove (xCoord.Length-1,1);
				}
				else
				{
					xCoord += input;
				}
			} else if (focus == selectedControl.ybot) {
				if (input == "\b")
				{
					if (yCoord.Length > 0)
						yCoord = yCoord.Remove (yCoord.Length-1,1);
				}
				else
				{
					yCoord += input;
				}
			} else if (focus == selectedControl.k11) {
				if (input == "\b")
				{
					if (k11.Length > 0)
						k11 = k11.Remove (k11.Length-1,1);
				}
				else
				{
					k11 += input;
				}
			} else if (focus == selectedControl.k12) {
				if (input == "\b")
				{
					if (k12.Length > 0)
						k12 = k12.Remove (k12.Length-1,1);
				}
				else
				{
					k12 += input;
				}
			} else if (focus == selectedControl.k21) {
				if (input == "\b")
				{
					if (k21.Length > 0)
						k21 = k21.Remove (k21.Length-1,1);
				}
				else
				{
					k21 += input;
				}
			} else if (focus == selectedControl.k22) {
				if (input == "\b")
				{
					if (k22.Length > 0)
						k22 = k22.Remove (k22.Length-1,1);
				}
				else
				{
					k22 += input;
				}
			}
			else if (focus == selectedControl.xlight) {
				if (input == "\b")
				{
					if (xlight.Length > 0)
						xlight = xlight.Remove (xlight.Length-1,1);
				}
				else
				{
					xlight += input;
				}
			}
			else if (focus == selectedControl.ylight) {
				if (input == "\b")
				{
					if (ylight.Length > 0)
						ylight = ylight.Remove (ylight.Length-1,1);
				}
				else
				{
					ylight += input;
				}
			}
		}
	}

<<<<<<< HEAD
	void parseFile()
	{

	}

	string getKeyboardInput()
=======
	public string getKeyboardInput()
>>>>>>> 39dc474133cff02730ec8aa687af693ac00f49cd
	{
		string ret = string.Empty;
		ret = Input.inputString;
		if (ret.Length > 1)
		{
				ret = ret.Substring(0,1);
		}
		return ret;
	}

	void OnGUI ()
	{
		GUI.TextArea (new Rect (0, 0, 250, 20), "Braitenberg Vehicles - Team Alpha");
		GUI.TextArea (new Rect (326, 280, 70, 20), "File Name:");
		GUI.TextArea (new Rect (326, 360, 70, 20), "Xcoord:");
		GUI.TextArea (new Rect (326, 380, 70, 20), "Ycoord:");
		GUI.TextArea (new Rect (326, 400, 70, 20), "K11:");
		GUI.TextArea (new Rect (326, 420, 70, 20), "K12:");
		GUI.TextArea (new Rect (326, 440, 70, 20), "K21:");
		GUI.TextArea (new Rect (326, 460, 70, 20), "K22:");

		GUI.TextArea (new Rect (636, 360, 60, 20), "xlight:");
		GUI.TextArea (new Rect (636, 380, 60, 20), "ylight:");
		GUI.TextArea (new Rect (636, 400, 60, 20), "intensity:");

		if (GUI.Button (new Rect(396, 300, 200, 20), "Upload data from file and start"))
		{
			parseFile ();
			Application.LoadLevel ("Main");
		}

		if (GUI.Button (new Rect(396,480,100,20), "Add Robot"))
		{
			float xf, yf, k11f, k12f, k21f, k22f;

			if (float.TryParse (xCoord, out xf) &&
			    float.TryParse (yCoord, out yf) &&
			    float.TryParse (k11, out k11f) &&
			    float.TryParse (k12, out k12f) &&
			    float.TryParse (k21, out k21f) &&
			    float.TryParse (k22, out k22f))
			{
				SetupData.robotList.Add (new Robot(SetupData.robotList.Count, xf,yf,k11f,k12f,k21f,k22f));
			}

			xCoord = string.Empty;
			yCoord = string.Empty;
			k11 = string.Empty;
			k12 = string.Empty;
			k21 = string.Empty;
			k22 = string.Empty;
		}

		if (GUI.Button (new Rect(396,280,200,20), filename))
		{
			focus = selectedControl.setText;
		}

		if (GUI.Button (new Rect(396,360,200,20), xCoord))
		{
			focus = selectedControl.xbot;
		}

		if (GUI.Button (new Rect(396,380,200,20), yCoord))
		{
			focus = selectedControl.ybot;
		}

		if (GUI.Button (new Rect(396,400,200,20), k11))
		{
			focus = selectedControl.k11;
		}

		if (GUI.Button (new Rect(396,420,200,20), k12))
		{
			focus = selectedControl.k12;
		}

		if (GUI.Button (new Rect(396,440,200,20), k21))
		{
			focus = selectedControl.k21;
		}

		if (GUI.Button (new Rect(396,460,200,20), k22))
		{
			focus = selectedControl.k22;
		}

		if (GUI.Button (new Rect(696,360,200,20), xlight))
		{
			focus = selectedControl.xlight;
		}

		if (GUI.Button (new Rect(696,380,200,20), ylight))
		{
			focus = selectedControl.ylight;
		}

		if (GUI.Button (new Rect(696,400,200,20), intensity))
		{
			focus = selectedControl.intensity;
		}

		if (GUI.Button (new Rect(696,420,100,20), "Add Light"))
		{
			float xf, yf, itf;

			if (float.TryParse (xlight, out xf) &&
			    float.TryParse (ylight, out yf) &&
			    float.TryParse (intensity, out itf))
			{
				SetupData.lightList.Add (new Light(SetupData.lightList.Count, xf, yf, itf));
			}

			xlight = string.Empty;
			ylight = string.Empty;
			intensity = string.Empty;
		}

		if (GUI.Button (new Rect(696,  460, 125,125), "Start Simulation\n without file"))
		{
			Application.LoadLevel ("Main");
		}
	}
}
