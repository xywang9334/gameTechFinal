using UnityEngine;
using System.Collections;
//for button use
public class LevelManagerForStart : MonoBehaviour {

	// Use this for initialization
	public void LoadScene (string name) {
		print(name);
		Application.LoadLevel (name);

	}
}
