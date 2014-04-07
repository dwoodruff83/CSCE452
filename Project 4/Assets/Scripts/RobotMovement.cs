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

	//Vector3 myPos;  // this objects position
	
	// list of all lights
	private Light[] lights;
	Vector3 SensorLposition;
	Vector3 SensorRposition;
	float SensorLdistance;
	float SensorRdistance;	
	
	// Use this for initialization
	void Start () {
		//print (SensorLposition);
		//print (SensorRposition);
		

		//myPos = transform.position;
		//print (string.Format("Initial position: {0}",myPos));
		/*foreach (Light light in lights) {
			Vector3 lightPos = light.transform.position;
			float lightIntensity = light.intensity;
			//string msg = string.Format("{0} @ {1}: {2}",light,lightPos,lightIntensity);
			//print (msg);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		SensorLposition = transform.Find ("SensorL").transform.position;
		SensorRposition = transform.Find ("SensorR").transform.position;
		lights = FindObjectsOfType (typeof(Light)) as Light[];
		//cast ray to lights, determine intensity (maximum is 8)
		// two points (sensors) on front of robot
		Wheel1 = 0; 
		Wheel2 = 0;
		Sensor1 = 0;
		Sensor2 = 0;
		float Sensor2range;
		foreach (Light light in lights) {
			Sensor2range = light.range;
			Sensor1 += Sensor2range / Vector3.Distance(SensorLposition,light.transform.position);
			Sensor2 += Sensor2range / Vector3.Distance(SensorRposition,light.transform.position);
			//print (string.Format("Distance from Left sensor to light: {0}",Sensor1));
			//print (string.Format("Distance from Right sensor to light: {0}",Sensor2));

			//print (Sensor2range);
		}
		Sensor2 /= lights.Length;
		Sensor1 /= lights.Length;

		float xdisplacement =0;
		float ydisplacement =0;
		//print (string.Format("Average distance from left sensor to light: {0}",Sensor1));
		//print (string.Format("Average distance from right sensor to light: {0}",Sensor2));
		Wheel1 = (K11*Sensor1 + K12*Sensor2)*1;
		Wheel2 = (K21*Sensor1 + K22*Sensor2)*1;
		//print (string.Format ("{0}, {1}, {2}, {3}",K11,K12,K21,K22));
		//print (transform.rotation.y);
		//Wheel1 is on the left side of the tank (depending on where it is facing)
		//print (string.Format ("Sen1: {0}, Sen2: {1}",Sensor1,Sensor2));
		//print (string.Format ("WHEEL1: {0}, WHEEL2: {1}",Wheel1,Wheel2));
		float movementAngle = 0;
		if (Wheel1 == Wheel2) {
				
		} else if (Wheel1 > Wheel2) {
			//Turning Right
			movementAngle = Mathf.Atan (Wheel1/Wheel2);  

		} else if (Wheel2 > Wheel1) {
			//Turning Left	
			movementAngle = Mathf.Atan (Wheel2/Wheel1);

		}
		xdisplacement = (Mathf.Sin (movementAngle) * Wheel1 + Mathf.Sin (movementAngle) * Wheel2);
		ydisplacement = (Mathf.Cos (movementAngle) * Wheel1 + Mathf.Cos (movementAngle) * Wheel2);
		//print (string.Format ("DisX: {0}, DisY: {1}",xdisplacement,ydisplacement));
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, Mathf.Rad2Deg*Mathf.Atan (xdisplacement/ydisplacement), transform.eulerAngles.z);
		transform.position = new Vector3 (transform.position.x+xdisplacement*Time.deltaTime, transform.position.y, transform.position.z+ydisplacement*Time.deltaTime);

		Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
		if (transform.position.x > 50)
		{
			transform.position = new Vector3(0, transform.position.y, transform.position.z);
		}
		else if (transform.position.x < 0)
		{
			transform.position = new Vector3(50, transform.position.y, transform.position.z);
		}

		if (transform.position.z > 30)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		}
		else if (transform.position.z < 0)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 30);
		}
	}
	// Xcoord, Ycoord
}
