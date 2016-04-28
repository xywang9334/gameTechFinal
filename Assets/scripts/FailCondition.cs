using UnityEngine;
using System.Collections;

public class FailCondition : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	// TODO:
	// More UI to add
	void OnTriggerEnter(Collider collide) {
		if (collide.CompareTag("ball")) {
			LevelManager.Instance.isWin = false;
			Application.LoadLevel ("win");
		}
	}
}
