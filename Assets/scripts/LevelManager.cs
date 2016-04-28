﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager> {

	private float _timeRemaining;
	public float maxTime;
	public int levelNum = 0;
	private Text timerLabel;
	public bool isWin = false;
	public bool isred = true;

	// Use this for initialization
	void Start () {
		TimeRemaining = maxTime;
	}
	
	public void setColor(bool value) {
		isred = value;
	}
	
	// Update is called once per frame
	void Update () {
		TimeRemaining -= Time.deltaTime;
		
		if (TimeRemaining <= 0)
		{
			Application.LoadLevel(Application.loadedLevel);
			TimeRemaining = maxTime;
			print ("died");
		}
	}
	
	public float TimeRemaining 
	{
		get { return _timeRemaining; }
		set { _timeRemaining = value; }
	}
	
	
	public void LoadLevel(string name){
		Application.LoadLevel (name);
		TimeRemaining = maxTime;
	}
	
	
	public void QuitRequest(){
		Application.Quit ();
	}
	
	public void LoadNextLevel() {
//		Application.LoadLevel(Application.loadedLevel + 1);
		TimeRemaining = maxTime;
	}
}
