using UnityEngine;
using System.Collections;

public class ResultCheck : MonoBehaviour {
	private GameObject winText;
	private GameObject loseText;

	// Use this for initialization
	void Start () {
		winText = GameObject.Find ("cong");
		loseText = GameObject.Find ("fail");
		if (LevelManager.Instance.isWin) {
			print ("1");
			loseText.SetActive (false);
		} 
		if (!LevelManager.Instance.isWin) {
			print ("0");
			winText.SetActive (false);

		}
	}
	

}
