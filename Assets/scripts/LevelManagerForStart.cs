using UnityEngine;
using System.Collections;

public class LevelManagerForStart : MonoBehaviour {

	// Use this for initialization
	public void LoadScene (string name) {
		print(name);
		Application.LoadLevel (name);

	}
}
