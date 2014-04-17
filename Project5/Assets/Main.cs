using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rectWrap
{
	public rectWrap(float topm, float leftm, float width, float height)
	{
		rect = new Rect (leftm, topm, width, height);
		up = new List<rectWrap> ();
		down = new List<rectWrap> ();
		left = new List<rectWrap> ();
		right = new List<rectWrap> ();
	}

	public void print()
	{
		Debug.Log (string.Format ("UP {0}, Down {1}, Left {2}, Right {3}", rect.yMin, rect.yMax, rect.xMin, rect.xMax));
	}

	public Rect rect;
	public List<rectWrap> up;
	public List<rectWrap> down;
	public List<rectWrap> left;
	public List<rectWrap> right;
}

public class Main : MonoBehaviour {
	
	public List<rectWrap> rectList = new List<rectWrap>();
	public float upperbound, lowerbound, leftbound, rightbound;

	GameObject selected = null;
	bool mousedown = false;
	bool printed = false;

	List<rectWrap> SubdivideArea(float top, float left, float width, float height, float cuts)
	{
		List<rectWrap> tempList = new List<rectWrap>();
		List<float> leftcoords = new List<float> ();
		List<float> topcoords = new List<float> ();

		float dx = width / cuts;
		float dy = height / cuts;

		for (int i = 0; i < cuts; i++)
		{
			for (int j = 0; j < cuts; j++)
			{
				tempList.Add(new rectWrap(top-j*dy,left+i*dx,dx,-1*dy));
			}
		}

		/*if (!printed) {
						foreach (rectWrap entry in tempList) {
								entry.print ();
						}
			printed = true;
				}*/

		return tempList;
	}

	// Use this for initialization
	void Start () {
		upperbound = 6;
		lowerbound = -4;
		leftbound = -5;
		rightbound = 5;
	}

	public void DetermineCollisions()
	{
		for (int i = 0; i < rectList.Count; i++)
		{

		}
	}

	// Update is called once per frame
	void Update ()
	{
		bool mouseStatus = Input.GetMouseButton (0);
		RaycastHit info;
		if (mouseStatus && Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out info))
		{
			if (selected == info.collider.gameObject || mousedown)
			{
				Ray current = Camera.main.ScreenPointToRay(Input.mousePosition);
				selected.transform.position = new Vector3(current.origin.x,current.origin.y,selected.transform.position.z);
			}
			else
			{

				selected = info.collider.gameObject;
			}
			mousedown = true;
		}

		if (!mouseStatus)
		{
			mousedown = false;
		}

		rectList = SubdivideArea (upperbound, leftbound, 10, 10, 4);
	}
}