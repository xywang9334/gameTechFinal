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

	private Vector3 ballPos;
	private Vector3 cameraPos;

	private static int coefficient = -1;
	private static List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
	private System.Random rnd = new System.Random();
	private Stack<Vector2> stack = new Stack<Vector2>();

	private Vector2 _currentTile;
	public Vector2 CurrentTile
	{
		get { return _currentTile; }
		private set
		{
			if (value.x < 1 || value.x >= this.mazeWidth - 1 || value.y < 1 || value.y >= this.mazeHeight - 1)
			{
				print (value.x);
				print (value.y);
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
		ballPos = new Vector3 (mazeWidth / 2, 0.0f, 0.0f);
		cameraPos = new Vector3 (0.0f, 0.0f, -10.0f);
		maze = new int[mazeWidth, mazeHeight];
		GameObject sphereBall = Instantiate (sphere, ballPos, Quaternion.identity) as GameObject;
		Instantiate (mCamera, cameraPos, Quaternion.identity);
		FollowBall followBall = GameObject.FindObjectOfType<FollowBall> ();
		followBall.ball = sphereBall;
		createMaze (difficulty);
	}

	private void createFramework() {
		Vector3 pos = new Vector3 (mazeWidth / 2, mazeHeight / 2, 0);
		Vector3 scale = new Vector3 (mazeWidth, 1.0f, 1.1f);
		GameObject wallUp = Instantiate(wall, pos, Quaternion.identity) as GameObject;
		wallUp.transform.localScale = scale;
		pos.y *= coefficient;
		GameObject wallDown = Instantiate (wall, pos, Quaternion.identity) as GameObject;
		wallDown.transform.localScale = scale;
		GameObject wallRight = Instantiate (wall, new Vector3 (0, 0, 0), 
			Quaternion.AngleAxis (90, new Vector3 (0, 0, 1))) as GameObject;
		wallRight.transform.localScale = scale;
		GameObject wallLeft = Instantiate(wall, new Vector3(mazeWidth, 0, 0), 
			Quaternion.AngleAxis (90, new Vector3 (0, 0, 1))) as GameObject;
		wallLeft.transform.localScale = scale;
	}


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
		for (int i = 0; i < mazeWidth; i++) {
			for (int j = 0; j < mazeHeight; j++) {
				print (maze [i, j]);
			}
			print ("\n");
		}
	}


	private int[,] fillTwoDimensionArray () {
		List<Vector2> neighbours;
		while (stack.Count > 0) {
			maze [(int)CurrentTile.x, (int)CurrentTile.y] = 0;
			neighbours = getValidNeighbour (CurrentTile);
			if (neighbours.Count > 0) {
				stack.Push (CurrentTile);
				int next = rnd.Next (neighbours.Count);
				print (neighbours [next]);
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
}
