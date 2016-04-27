using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour {
	[SerializeField]
	private Text timerLabel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timerLabel.text = FormatTime(LevelManager.Instance.TimeRemaining);
		
	}
	
	private string FormatTime(float timeInSeconds)
	{
		return string.Format("{0}:{1:00}", Mathf.FloorToInt(timeInSeconds/60), Mathf.FloorToInt(timeInSeconds % 60));
	}
}
