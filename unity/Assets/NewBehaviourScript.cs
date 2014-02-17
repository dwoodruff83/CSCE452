using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
				if (Input.anyKeyDown)
				{	
						transform.position = new Vector3 (transform.position.x + 10 * Time.deltaTime, transform.position.y + 1, transform.position.z + 1);
				}
		}
}
