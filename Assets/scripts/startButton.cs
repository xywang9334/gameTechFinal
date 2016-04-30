 using UnityEngine;
using System.Collections;

public class startButton : MonoBehaviour {

	// Use this for initialization
	void Start_Game_Button () {
		LevelManager.Instance.LoadLevel ("BallScene");
	}

}
