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
			Debug.Log ("Robot Created");
				Instantiate(Resources.Load ("Panzer_II_Ausf_F_static"),new Vector3(entry.m_x, 1.5f, entry.m_y),new Quaternion());
			}
			foreach (rLight entry in SetupData.lightList) {
			Debug.Log("Light Created");
				Instantiate (Resources.Load ("Point light 1"),new Vector3(entry.m_x,1.5f,entry.m_y),new Quaternion());
				}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
