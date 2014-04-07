using UnityEngine;
using System.Collections;

public class RobotMovement : MonoBehaviour {
	// Each vehicle has it's own variables
	float Wheel1, Wheel2;
	float Sensor1;
	float Sensor2;
	public float K11; 
	public float K12;
	public float K21; 
	public float K22;

	// Sensor1,2 = LightIntensity / distToLight()

	Vector3 myPos;  // this objects position
	
	// list of all lights
	private Light[] lights;
	Vector3 SensorLposition;
	Vector3 SensorRposition;
	float SensorLdistance;
	float SensorRdistance;	
	
	// Use this for initialization
	void Start () {
		SensorLposition = transform.Find ("SensorL").transform.position;
		SensorRposition = transform.Find ("SensorR").transform.position;
		
		//print (SensorLposition);
		//print (SensorRposition);
		
		lights = FindObjectsOfType (typeof(Light)) as Light[];
		myPos = transform.position;
		//print (string.Format("Initial position: {0}",myPos));
		foreach (Light light in lights) {
			Vector3 lightPos = light.transform.position;
			float lightIntensity = light.intensity;
			string msg = string.Format("{0} @ {1}: {2}",light,lightPos,lightIntensity);
			//print (msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//cast ray to lights, determine intensity (maximum is 8)
		// two points (sensors) on front of robot
		rigidbody.velocity = new Vector3(0,0,0);
		Wheel1 = 0; 
		Wheel2 = 0;
		Sensor1 = 0;
		Sensor2 = 0;
		float Sensor2range;
		foreach (Light light in lights) {
			Sensor2range = light.range;
			Sensor1 += Sensor2range / Vector3.Distance(SensorLposition,light.transform.position);
			Sensor2 += Sensor2range / Vector3.Distance(SensorRposition,light.transform.position);
			print (string.Format("Distance from Left sensor to light: {0}",Sensor1));
			print (string.Format("Distance from Right sensor to light: {0}",Sensor2));

			print (Sensor2range);
		}
		Sensor2 /= lights.Length;
		Sensor1 /= lights.Length;
		print (string.Format("Average distance from left sensor to light: {0}",Sensor1));
		print (string.Format("Average distance from right sensor to light: {0}",Sensor2));
		Wheel1 = K11*Sensor1 + K12*Sensor2;
		Wheel2 = K21*Sensor1 + K22*Sensor2;
		print (string.Format ("{0}, {1}, {2}, {3}",K11,K12,K21,K22));

		rigidbody.velocity = new Vector3(1,0,1);
	}
	// Xcoord, Ycoord
}
