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

		center = new Vector3 (top - (height / 2), (leftm + width / 2), 0);
		topleft = new Vector3 (left, top);
		topright = new Vector3 (right, top);
		botleft = new Vector3 (left, bot);
		botright = new Vector3 (right, bot);

	}

}

public class obstacle
{
	public obstacle(GameObject gameobjectm, float halfwidthm, float halfheightm)
	{
		gameobject = gameobjectm;
		position = gameobject.transform.position;
		halfwidth = halfwidthm;
		halfheight = halfheightm;
	}
	
	public void update()
	{
		position = gameobject.transform.position;
	}
	
	public bool contains(Rectangle test)
	{
		float leftx, rightx, topy, boty;
		leftx = position.x - halfwidth;
		rightx = position.x + halfwidth;
		topy = position.y + halfheight;
		boty = position.y - halfheight;
		
		if (test.topleft.x >= leftx && test.topleft.x <= rightx && test.topleft.y <= topy && test.topleft.y >= boty ||
		    test.topright.x >= leftx && test.topright.x <= rightx && test.topright.y <= topy && test.topright.y >= boty ||
		    test.botleft.x >= leftx && test.botleft.x <= rightx && test.botleft.y <= topy && test.botleft.y >= boty ||
		    test.botright.x >= leftx && test.botright.x <= rightx && test.botright.y <= topy && test.botright.y >= boty)
		{
			return true;
		}
		return false;
		
	}
	
	public float halfwidth;
	public float halfheight;
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

	public static void printer(Vector3 start, Vector3 end)
	{
		Debug.DrawLine (start, end, Color.white ,20);
	}

	public void printLines()
	{
		printer(rect.topleft, rect.topright);
		printer(rect.topleft, rect.botleft);
		printer(rect.topright, rect.botright);
		printer(rect.botleft, rect.botright);
		/*foreach (rectWrap entry in up) {
			Debug.DrawLine(rect.center,entry.rect.center, Color.white,10000);
		}*/
		
		/*foreach (rectWrap entry in down) {
			Debug.DrawLine(rect.center,entry.rect.center, Color.white,10000);
		}
		
		foreach (rectWrap entry in left) {
			Debug.DrawLine(rect.center,entry.rect.center, Color.white,10000);
		}
		
		foreach (rectWrap entry in right) {
			Debug.DrawLine(rect.center,entry.rect.center, Color.white,10000);
		}*/
	}

	public void print()
	{
		string above = "";
		string below = "";
		string totheleft = "";
		string totheright = "";

		foreach (rectWrap entry in up) {
			above += " ," + entry.id;
				}

		foreach (rectWrap entry in down) {
			below +=" ," + entry.id;
				}

		foreach (rectWrap entry in left) {
			totheleft +=" ," + entry.id;
				}

		foreach (rectWrap entry in right) {
			totheright +=" ," + entry.id;
				}

		Debug.Log (string.Format ("ID {0} UP {1}, Down {2}, Left {3}, Right {4} \nAbove: {5}\n Below: {6}\n totheleft: {7}\n totheright: {8}\n", id, rect.top, rect.bot, rect.left, rect.right,above, below, totheleft, totheright));
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
			for (int j = i+1; j <rectList.Count; j++)
			{
				rectWrap entry = rectList[i];
				rectWrap entry2 = rectList[j];
				if (entry.rect.top == entry2.rect.bot)
				{
					entry.up.Add(entry2);
				}

				if (entry.rect.bot == entry2.rect.top)
				{
					entry.down.Add(entry2);
				}

				if (entry.rect.left == entry2.rect.right)
				{
					entry.left.Add(entry2);
				}

				if (entry.rect.right == entry2.rect.left)
				{
					entry.right.Add(entry2);
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
			//BuildGraph ();
			for (int i = 0; i < rectList.Count; i++)
			{
				rectList [i].printLines ();
			}
			printed = true;
		}
		else if (waitTime > 0)
		{
			waitTime -= Time.deltaTime;
		}
	}
}