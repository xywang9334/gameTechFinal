using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public Vector3 scale;
	public Vector3 position;

	// Use this for initialization
	void Start () {
		transform.localScale = scale;
		transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
