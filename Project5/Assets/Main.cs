using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rectangle
{
	public float bot, top, left, right, width, height;
	public Vector3 center, topleft, topright, botleft, botright;

	public Rectangle(float topm, float leftm, float widthm, float heightm)
	{
		top = topm;
		left = leftm;
		width = widthm;
		height = heightm;

		bot = top - height;
		right = left + width;

		center = new Vector3 ((left + width / 2),(top - height / 2), 0);
		topleft = new Vector3 (left, top);
		topright = new Vector3 (right, top);
		botleft = new Vector3 (left, bot);
		botright = new Vector3 (right, bot);
	}

	public bool contains(Vector3 point)
	{
		if (point.x > left && point.x < right && point.y < top && point.y > bot)
		{
			return true;
		}
		return false;
	}
}

public class obstacle
{
	public obstacle(GameObject gameobjectm, float halfwidthm, float halfheightm)
	{
		gameobject = gameobjectm;
		position = gameobject.transform.position;
		width = halfwidthm*2;
		height = halfheightm*2;
	}
	
	public void update()
	{
		position = gameobject.transform.position;

		top = position.y + height/2;
		left = position.x - width/2;
		
		bot = top - height;
		right = left + width;

		center = position;
		topleft = new Vector3 (left, top);
		topright = new Vector3 (right, top);
		botleft = new Vector3 (left, bot);
		botright = new Vector3 (right, bot);
	}
	
	public bool contains(Rectangle test)
	{
		if (test.topleft.x >= left && test.topleft.x <= right && test.topleft.y <= top && test.topleft.y >= bot ||
		    test.topright.x >= left && test.topright.x <= right && test.topright.y <= top && test.topright.y >= bot ||
		    test.botleft.x >= left && test.botleft.x <= right && test.botleft.y <= top && test.botleft.y >= bot ||
		    test.botright.x >= left && test.botright.x <= right && test.botright.y <= top && test.botright.y >= bot)
		{
			return true;
		}
		else if (topleft.x >= test.left && topleft.x <= test.right && topleft.y <= test.top && topleft.y >= test.bot ||
		         topright.x >= test.left && topright.x <= test.right && topright.y <= test.top && topright.y >= test.bot ||
		         botleft.x >= test.left && botleft.x <= test.right && botleft.y <= test.top && botleft.y >= test.bot ||
		         botright.x >= test.left && botright.x <= test.right && botright.y <= test.top && botright.y >= test.bot)
		{
			return true;
		}
		return false;
	}

	public float bot, top, left, right, width, height;
	public Vector3 center, topleft, topright, botleft, botright;
	public Vector3 position;
	public GameObject gameobject;
}

public class rectWrap
{
	public rectWrap(float ID,float topm, float leftm, float width, float height, float subcounter)
	{
		id = ID;
		rect = new Rectangle (topm, leftm, width, height);
		weight = Mathf.Abs (width * height);
		subcount = subcounter + 1;
		up = new List<rectWrap> ();
		down = new List<rectWrap> ();
		left = new List<rectWrap> ();
		right = new List<rectWrap> ();
	}

	public static void printer(Vector3 start, Vector3 end, Color color)
	{
		Debug.DrawLine (start, end, color, 1);
	}

	public void printLines()
	{
		printer(rect.topleft, rect.topright, Color.white);
		printer(rect.topleft, rect.botleft, Color.white);
		printer(rect.topright, rect.botright, Color.white);
		printer(rect.botleft, rect.botright, Color.white);

		foreach (rectWrap entry in up) {
			printer (rect.center, entry.rect.center, Color.red);
		}
		
		foreach (rectWrap entry in down) {
			printer (rect.center, entry.rect.center, Color.green);
		}
		
		foreach (rectWrap entry in left) {
			printer (rect.center, entry.rect.center, Color.blue);
		}
		
		foreach (rectWrap entry in right) {
			printer (rect.center, entry.rect.center, Color.yellow);
		}
	}

	public void print()
	{
		string above = "";
		string below = "";
		string totheleft = "";
		string totheright = "";

		foreach (rectWrap entry in up) {
			above += " " + entry.id;
				}

		foreach (rectWrap entry in down) {
			below +=" " + entry.id;
				}

		foreach (rectWrap entry in left) {
			totheleft +=" " + entry.id;
				}

		foreach (rectWrap entry in right) {
			totheright +=" " + entry.id;
				}

		Debug.Log (string.Format ("ID {0} UP {1}, Down {2}, Left {3}, Right {4}\nCenter: {5}\nAbove: {6}\n Below: {7}\n totheleft: {8}\n totheright: {9}\n", id, rect.top, rect.bot, rect.left, rect.right, rect.center, above, below, totheleft, totheright));
	}

	public int colcount;
	public float id;
	public Rectangle rect;
	public float weight;
	public float subcount;
	public List<rectWrap> up;
	public List<rectWrap> down;
	public List<rectWrap> left;
	public List<rectWrap> right;
}

public class Main : MonoBehaviour {
	
	static int idmaster = 0;
	public List<obstacle> obstacleList = new List<obstacle> ();
	public List<rectWrap> rectList = new List<rectWrap>();
	public float upperbound, leftbound, width, height;
	public float waitTime = 0;

	GameObject selected = null;
	bool mousedown = false;
	bool printed = false;

	List<rectWrap> SubdivideArea(float top, float left, float width, float height, float cuts, float subdivisions)
	{
		List<rectWrap> tempList = new List<rectWrap>();

		float dx = width / cuts;
		float dy = height / cuts;

		for (int i = 0; i < cuts; i++)
		{
			for (int j = 0; j < cuts; j++)
			{
				tempList.Add(new rectWrap(idmaster++, top-j*dy,left+i*dx,dx,dy,subdivisions));
			}
		}

		return tempList;
	}

	// Use this for initialization
	void Start () {
		upperbound = 6;
		leftbound = -5;
		width = 10;
		height = 10;
		obstacleList.Add (new obstacle(GameObject.Find ("block1"),2.5f,2.5f));
		obstacleList.Add (new obstacle(GameObject.Find ("block2"),1.5f,1.5f));
		obstacleList.Add (new obstacle(GameObject.Find ("block3"),1,1));
	}

	public void BuildGraph()
	{
		for (int i = 0; i < rectList.Count; i++)
		{
			for (int j = 0; j <rectList.Count; j++)
			{
				rectWrap entry = rectList[i];
				rectWrap entry2 = rectList[j];

				if (entry != entry2)
				{
					Vector3 toptest = new Vector3(entry.rect.center.x,entry.rect.top+.01f, 0);
					Vector3 bottest = new Vector3(entry.rect.center.x,entry.rect.bot-.01f, 0); 
					Vector3 lefttest = new Vector3(entry.rect.left-.01f,entry.rect.center.y, 0); 
					Vector3 righttest = new Vector3(entry.rect.right+.01f,entry.rect.center.y, 0); 

					if (entry2.rect.contains (toptest))
					{
						if (!entry.up.Contains(entry2))
						{
							entry.up.Add(entry2);
						}

						if (!entry2.down.Contains(entry))
						{
							entry2.down.Add(entry);
						}
					}

					if (entry2.rect.contains(bottest))
					{
						if (!entry.down.Contains(entry2))
						{
							entry.down.Add(entry2);
						}

						if (!entry2.up.Contains(entry))
						{
							entry2.up.Add(entry);
						}
					}

					if (entry2.rect.contains(lefttest))
					{
						if (!entry.left.Contains(entry2))
						{
							entry.left.Add (entry2);
						}

						if (!entry2.right.Contains(entry))
						{
							entry2.right.Add(entry);
						}
					}

					if (entry2.rect.contains (righttest))
					{
						if (!entry.right.Contains(entry2))
						{
							entry.right.Add (entry2);
						}

						if (!entry2.left.Contains(entry))
						{
							entry2.left.Add(entry);
						}
					}
				}
			}
		}
	}

	public void DetermineCollisions()
	{
		for (int i = 0; i < rectList.Count;)
		{
			rectWrap temp = rectList[i];
			bool collided = false;

			foreach (obstacle entry in obstacleList)
			{
				int collisionCount = 0;

				if (entry.contains ( temp.rect))
				{
					collisionCount++;
					collided = true;
				}
				if (entry.contains ( temp.rect))
				{
					collisionCount++;
					collided = true;
				}
				if (entry.contains ( temp.rect))
				{
					collisionCount++;
					collided = true;
				}
				if (entry.contains ( temp.rect))
				{
					collisionCount++;
					collided = true;
				}

				if (collisionCount > temp.colcount)
				{
					temp.colcount = collisionCount;
				}

				if (collided)
				{
					break;
				}
			}

			if (collided && temp.subcount <= 3)
			{
				List<rectWrap> subdividedArea = SubdivideArea(temp.rect.top,temp.rect.left,temp.rect.width,temp.rect.height,4,temp.subcount);
				rectList.Remove(temp);
				rectList.AddRange(subdividedArea);
			}
			else
			{
				i++;
			}
		}

		for( int i = 0; i < rectList.Count;)
		{
			if (rectList[i].colcount >= 1)
			{
				rectList.RemoveAt(i);
			}
			else
			{
				i++;
			}
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
			waitTime = 2;
			printed = false;
			mousedown = true;
		}

		if (!mouseStatus) 
		{
			mousedown = false;
		} 

		if (waitTime <= 0 && !printed)
		{
			foreach (obstacle entry in obstacleList)
			{
				entry.update();
			}

			rectList.Clear();
			rectList = SubdivideArea (upperbound, leftbound, width, height, 4, 0);
			DetermineCollisions ();
			BuildGraph ();
			/*for (int i = 0; i < rectList.Count; i++)
			{
				rectList[i].print();
			}*/
			printed = true;
		}
		else if (waitTime > 0)
		{
			waitTime -= Time.deltaTime;
		}

		for (int i = 0; i < rectList.Count; i++)
		{
			rectList [i].printLines ();
		}
	}
}