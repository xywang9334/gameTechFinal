using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour {

	Button button;

	void Awake() {
		button = GetComponent<Button>();
		button.onClick.AddListener (() => {findLastLevel();});
	}


	private void findLastLevel() {
		LevelManager.Instance.loadLastLevel ();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
