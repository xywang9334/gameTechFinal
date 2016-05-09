using UnityEngine;
using System.Collections;

public class FailCondition : MonoBehaviour {

	public Color color;
	private Material material;

	// Use this for initialization
	void Start () {
		material = gameObject.GetComponent<Renderer>().material;
		material.SetColor("_Color", color);
	}

	void OnTriggerEnter(Collider collide) {
		if (collide.CompareTag("ball")) {
			LevelManager.Instance.isWin = false;
			LevelManager.Instance.LoadLevel ("Lose");
		}
	}
}
