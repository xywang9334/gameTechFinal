using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowBall : MonoBehaviour {
	public GameObject ball;
	Vector3 ballPosition;
	// Use this for initialization
	void Start () {
		transform.RotateAround(transform.position, Vector2.right, 20);
	}
	// TODO:
	// Camera delay
	// Update is called once per frame
	void Update () {
		ballPosition = ball.transform.position;
		ballPosition.z -= 10.0f; 
		ballPosition.y += 3.0f; 
		transform.position = ballPosition;
	}
}
