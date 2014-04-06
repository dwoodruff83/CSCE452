using UnityEngine;
using System.Collections;

public class RobotMovement : MonoBehaviour {
	// Each vehicle has it's own variables
	// Wheel1, Wheel2
	// Sensor1, Sensor2
	// K11, K12, K21, K22
	// Xcoord, Ycoord
	
	// Wheel1 = k11*Sensor1 + k12*Sensor2
	// Wheel2 = k21*Sensor1 + k22*Sensor2
	// Sensor1,2 = LightIntensity / distToLight()


	// list of all lights
	private Light[] lights;
	Vector3 myPos;
	// Use this for initialization
	void Start () {
		lights = FindObjectsOfType (typeof(Light)) as Light[];
		myPos = transform.position;
		print (string.Format("Initial position {0}:  ",myPos));
		foreach (Light light in lights) {
			Vector3 lightPos = light.transform.position;
			float lightIntensity = light.intensity;
			string msg = string.Format("{0} @ {1}: {2}",light,lightPos,lightIntensity);
			print (msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//cast ray to lights, determine intensity (maximum is 8)
		// two points (sensors) on from of robot

	}
}
