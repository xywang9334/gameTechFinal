using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class mazeGenerator : MonoBehaviour {
	public int mazeWidth, mazeHeight;
	private int[,] maze;
	public int difficulty;

	public GameObject wall;
	public GameObject sphere;
	public GameObject mCamera;
	public GameObject redBrick;
	public GameObject blueBrick;

	private Vector3 ballPos;
	public GameObject parent;
	private Vector3 cameraPos;
	private static int coefficient = -1;
	private static List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
	private System.Random rnd = new System.Random();
	private Stack<Vector2> stack = new Stack<Vector2>();
	public GameObject winObject;
	private GameObject sphereBall;
	private GameObject win;

	private Vector2 _currentTile;

	enum state {PASSED, UNPASSED, UNTESTED};

	struct node {
		public double pathLengthToNode;
		public double straightLineDistance;
		public state passed;
		public Vector2 position;
		public Vector2 previous;
		public bool previousAssigned;
		public double estimatedDistance {
			get { return straightLineDistance + pathLengthToNode; }
		}
	};


	private node[,] mazeSolver;
	public Vector2 CurrentTile
	{
		get { return _currentTile; }
		private set
		{
			if (value.x < 1 || value.x >= this.mazeWidth - 1 || value.y < 1 || value.y >= this.mazeHeight - 1)
			{
				throw new ArgumentException("CurrentTile must be within the one tile border all around the maze");
			}
			if (value.x % 2 == 1 || value.y % 2 == 1)
			{ _currentTile = value; }
			else
			{
				throw new ArgumentException("The current square must not be both on an even X-axis and an even Y-axis, to ensure we can get walls around all tunnels");
			}
		}
	}

	void Start() {
//		ballPos = new Vector3 (mazeWidth / 2.0f - 1.0f, mazeHeight / 2.0f - 1.0f, 0.0f);
		cameraPos = new Vector3 (0.0f, 0.0f, -30.0f);
		maze = new int[mazeWidth, mazeHeight];
		sphereBall = Instantiate (sphere, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
		Instantiate (mCamera, cameraPos, Quaternion.identity);
		RenderSettings.skybox = (Material)Resources.Load("CloudyCrown_Midday");
		FollowBall followBall = GameObject.FindObjectOfType<FollowBall> ();
		followBall.ball = sphereBall;
		createMaze (difficulty);
	}

	private void createFramework() {
		Vector3 pos = new Vector3 (mazeWidth / 2.0f, mazeHeight / 2.0f, 0.0f);
		Vector3 scale = new Vector3 (mazeWidth + 2.0f, 1.0f, 1.1f);
		Vector3 scaleVerticle = new Vector3 (mazeHeight + 2.0f, 1.0f, 1.1f);
		GameObject wallUp = Instantiate(wall, pos, Quaternion.identity) as GameObject;
		wallUp.transform.localScale = scale;
		// attach to another object
		wallUp.transform.parent = parent.transform;
		pos.y *= coefficient;
		GameObject wallDown = Instantiate (wall, pos, Quaternion.identity) as GameObject;
		wallDown.transform.localScale = scale;
		// attach to another object
		wallDown.transform.parent = parent.transform;
		GameObject wallRight = Instantiate (wall, new Vector3 (0, 0, 0), 
			Quaternion.AngleAxis (90, new Vector3 (0, 0, 1))) as GameObject;
		wallRight.transform.localScale = scaleVerticle;
		wallRight.transform.parent = parent.transform;
		GameObject wallLeft = Instantiate(wall, new Vector3(mazeWidth, 0, 0), 
			Quaternion.AngleAxis (90, new Vector3 (0, 0, 1))) as GameObject;
		wallLeft.transform.localScale = scaleVerticle;
		wallLeft.transform.parent = parent.transform;

	}

	enum nextToGenerate {WALL, RED, BLUE};
	private void createMaze(int difficulty) {
		createFramework ();
		for (int i = 0; i < mazeWidth; i++) {
			for (int j = 0; j < mazeHeight; j++) {
				maze [i, j] = 1;
			}
		}
		CurrentTile = Vector2.one;
		stack.Push (CurrentTile);
		maze = fillTwoDimensionArray ();
		Vector3 scale = new Vector3 (1.0f, 1.0f, 1.0f);
		// maximum number of bricks in the maze
		int maximumBlackWallNumber = difficulty * 15;
		Vector3 position = new Vector3 (mazeWidth, mazeHeight, 0);
		int count = 0;
		for (int i = 0; i < mazeWidth; i++) {
			for (int j = 0; j < mazeHeight; j++) {
				Vector3 pos = new Vector3 (i, j - mazeHeight / 2.0f + 1.0f, 0);
				if (maze [i, j] == 1) {
					bool boundary = checkBoundary (i, j);
					if (boundary) {
						continue;
					}
					nextToGenerate n = findNextToGenerate (count, maximumBlackWallNumber);
					if (n == nextToGenerate.WALL) {
						GameObject blackWall = Instantiate (wall, pos, Quaternion.identity) as GameObject;
						blackWall.transform.localScale = scale;
						blackWall.transform.parent = parent.transform;
					} else if (n == nextToGenerate.RED) {
						GameObject redWall = Instantiate (redBrick, pos, Quaternion.identity) as GameObject;
						redWall.transform.localScale = scale;
						redWall.transform.parent = parent.transform;
					} else {
						GameObject blueWall = Instantiate (blueBrick, pos, Quaternion.identity) as GameObject;
						blueWall.transform.localScale = scale;
						blueWall.transform.parent = parent.transform;
					}
				} else {
					if (pos.y < position.y) {
						position = pos;
					}
				}
			}
		}
		sphereBall.transform.position = position;
		position = findMiddleAvailablePoint (maze);
		win = Instantiate (winObject, position, Quaternion.identity) as GameObject;
		win.transform.parent = parent.transform;
	}


	Vector3 findMiddleAvailablePoint(int [,]maze) {
		int i = (int)mazeWidth / 2; int j = (int)mazeHeight / 2;
		while (i >= 0 && i < mazeWidth && j >= 0 && j < mazeHeight) {
			if (maze [i, j] == 0) {
				return new Vector3 (i, j - mazeHeight / 2.0f + 1.0f, 0);
			} else {
				i += 1;
				j += 1;
			}
		}
		return new Vector2 (0.0f, 0.0f);
	}


	bool checkBoundary(int i, int j) {
		return j == 0 || j == mazeHeight - 1 /*|| i == 0 || i == mazeWidth - 1*/;
	}


	nextToGenerate findNextToGenerate(int count, int maximumBlackWallNumber) {
		nextToGenerate n;
		int next = rnd.Next (maximumBlackWallNumber + 10);
		if (next < maximumBlackWallNumber && count < maximumBlackWallNumber) {
			n = nextToGenerate.WALL;
		} else if (next < maximumBlackWallNumber + 5) {
			n = nextToGenerate.BLUE;
		} else {
			n = nextToGenerate.RED;
		}
		return n;
	}


	private int[,] fillTwoDimensionArray () {
		List<Vector2> neighbours;
		while (stack.Count > 0) {
			maze [(int)CurrentTile.x, (int)CurrentTile.y] = 0;
			neighbours = getValidNeighbour (CurrentTile);
			if (neighbours.Count > 0) {
				stack.Push (CurrentTile);
				int next = rnd.Next (neighbours.Count);
				CurrentTile = neighbours [next];
			} else {
				CurrentTile = stack.Pop ();
			}

		}
		return maze;
	}
	private List<Vector2> getValidNeighbour (Vector2 tile) {
		List<Vector2> validNeighbors = new List<Vector2>();
		foreach (var offset in offsets) {
			Vector2 tileToCheck = new Vector2 (tile.x + offset.x, tile.y + offset.y);
			if ((tileToCheck.x % 2 == 1) || (tileToCheck.y % 2 == 1)) {
				if (maze [(int)tileToCheck.x, (int)tileToCheck.y] == 1 && HasThreeWalls (tileToCheck)) {
					validNeighbors.Add (tileToCheck);
				}
			}
		}
		return validNeighbors;
	}


	private bool HasThreeWalls(Vector2 VectorToCheck) {
		int wallCount = 0;
		foreach (var offset in offsets) {
			Vector2 toCheck = new Vector2 (VectorToCheck.x + offset.x, VectorToCheck.y + offset.y);
			if (insideMaze (toCheck) && maze[(int)toCheck.x, (int)toCheck.y] == 1) {
				wallCount++;
			}
		}
		return wallCount == 3;
	}

	private bool insideMaze(Vector2 p) {
		return p.x >= 0 && p.y >= 0 && p.x < mazeHeight && p.y < mazeWidth;
	}


	private List<Vector2> getAdjacent (Vector2 nodes) {
		List<Vector2> values = new List<Vector2> ();
		foreach (var offset in offsets) {
			Vector2 temp = new Vector2 (nodes.x + offset.x, nodes.y + offset.y);
			if (insideMaze (temp)) {
				values.Add (temp);
			}
		}
		return values;
	}


	private List<node> getAdjacentWalkableNodes (Vector2 nodes) {
		List<node> node = new List<node>();
		List<Vector2> adjacent = getAdjacent (nodes);
		foreach (var location in adjacent) {
			int x = (int)location.x;
			int y = (int)location.y;
			node n = mazeSolver [x, y];
			Vector2 parent = n.previous;
			node parentNode = mazeSolver [(int)parent.x, (int)parent.y];
			if (maze [x, y] == 1) {
				continue;
			}
			if (n.passed == state.PASSED) {
				continue;
			} else if (n.passed == state.UNPASSED) {
				double distance = getDistance (location, parent);
				double temp = n.straightLineDistance + distance;
				if (temp < n.straightLineDistance) {
					n.previous = nodes;
					n.straightLineDistance = parentNode.straightLineDistance + getDistance (n.position, n.previous);
					n.previousAssigned = true;
					node.Add (n);
				}
			} else {
				n.previous = nodes;
				n.straightLineDistance = parentNode.straightLineDistance + getDistance (n.position, n.previous);
				n.passed = state.UNPASSED;
				n.previousAssigned = true;
				node.Add (n);
			}

		}
			
		return node;
	}



	private bool search(Vector2 current, Vector2 end) {
		mazeSolver [(int)current.x, (int)current.y].passed = state.PASSED;
		List<node> nodes = getAdjacentWalkableNodes (current);
		nodes.Sort((node1, node2) => node1.estimatedDistance.CompareTo(node2.estimatedDistance));
		foreach (var node in nodes) {
			if (node.position == end) {
				return true;
			} else {
				if (search (node.position, end)) {
					return true;
				}
			}
		}
		return false;
	}


	private double getDistance(Vector2 a, Vector2 b) {
		return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
	}


	private void initializeMazeSolver (int i, int j, Vector2 start, Vector2 end) {
		mazeSolver [i, j].position = new Vector2 (i, j);
		// start to here
		mazeSolver [i, j].straightLineDistance = 0.0;
		// end to here
		mazeSolver [i, j].pathLengthToNode = getDistance (mazeSolver[i, j].position, end);
		mazeSolver [i, j].passed = state.UNTESTED;
		mazeSolver [i, j].previousAssigned = false;
	}

	public void puzzleSolver() {
		Vector2 start = sphereBall.transform.position;
		Vector2 end = win.transform.position;
		mazeSolver = new node[mazeWidth, mazeHeight];
		for (int i = 0; i < mazeWidth; i++) {
			for (int j = 0; j < mazeHeight; j++)
				initializeMazeSolver (i, j, start, end);
		}
		mazeSolver [(int)start.x, (int)start.y].passed = state.PASSED;
		bool success = search (start, end);
		if (success) {
			List<Vector2> path = new List<Vector2> ();
			node n = mazeSolver [(int)end.x, (int)end.y];
			while (n.previousAssigned) {
				path.Add (n.position);
				n = mazeSolver [(int)n.position.x, (int)n.position.y];
			}
			path.Reverse ();
		}
	}
}
