using UnityEngine;
using System.Collections;

/*public class Robot
{
	// Needed Variables
	// Wheel1, Wheel2
	// Sensor1, Sensor2
	// K11, K12, K21, K22
	// Xcoord, Ycoord

	// Wheel1 = k11*Sensor1 + k12*Sensor2
	// Wheel2 = k21*Sensor1 + k22*Sensor2
	// Sensor1,2 = LightIntensity / distToLight()
}*/

public class Main : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		foreach (Robot entry in SetupData.robotList) {
			GameObject temp = (GameObject)Instantiate(Resources.Load ("Panzer_II_Ausf_F_static"),new Vector3(entry.m_x, 1.5f, entry.m_y),new Quaternion());
			temp.transform.Rotate(new Vector3(-90,0,0));
		    RobotMovement a = (RobotMovement)temp.GetComponent("RobotMovement");
			a.K11 = entry.m_k11;
			a.K12 = entry.m_k12;
			a.K21 = entry.m_k21;
			a.K22 = entry.m_k22;
			}
		foreach (rLight entry in SetupData.lightList) {
			GameObject temp = (GameObject)Instantiate (Resources.Load ("Point light 1"),new Vector3(entry.m_x,1.5f,entry.m_y),new Quaternion());
			temp.light.range = entry.m_intensity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
