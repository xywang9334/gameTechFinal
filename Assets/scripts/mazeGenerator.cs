using UnityEngine;
using System.Collections;
using System;

public class mazeGenerator : MonoBehaviour {
	public int mazeWidth, mazeHeight;
	private int[,] maze;
	public int difficulty;

	public GameObject wall;
	public GameObject sphere;

	private Vector3 ballPos;

	private static int coefficient = -1;

	void Start() {
		ballPos = new Vector3 (0.0f, 0.0f, 0.0f);
		maze = new int[mazeWidth, mazeHeight];
		GameObject sphereBall = Instantiate (sphere, ballPos, Quaternion.identity) as GameObject;
		createMaze (1);
	}

	private void createFramework() {
		Vector3 pos = new Vector3 (mazeWidth / 2, mazeHeight / 2, 0);
		Vector3 scale = new Vector3 (10.0f, 1.0f, 1.1f);
		GameObject wallUp = Instantiate(wall, pos, Quaternion.identity) as GameObject;
		wallUp.transform.position = pos;
		wallUp.transform.localScale = scale;
	}


	public void createMaze(int difficulty) {
		createFramework ();
	}
}
