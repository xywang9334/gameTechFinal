using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class showHint : MonoBehaviour {

	Button button;


	void Awake() {
		button = GetComponent<Button>();
		button.onClick.AddListener (() => {Hint();});
	}


	public void Hint() {
		mazeGenerator maze = GameObject.FindObjectOfType<mazeGenerator>();
		maze.puzzleSolver();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
