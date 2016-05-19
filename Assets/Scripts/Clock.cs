using UnityEngine;

using System;
using System.Collections;
using System.Globalization;

public class ClockEventArgs : EventArgs {

	public readonly int year;
	public readonly int month;
	public readonly int day;
	public readonly int second;

	public ClockEventArgs(int year, int month, int day, int second) {
		this.year = year;
		this.month = month;
		this.day = day;
		this.second = second;
	}
}

public class Clock : MonoBehaviour {
	
	public float scale { get; set; }
	public float currentTime { get; set; }
	public int year { get; private set; }
	public int month { get; private set; }
	public int day { get; private set; }
	public int second { get; private set; }

	public GUISkin skin;

	public delegate void ClockChangeHandler(Clock clock, ClockEventArgs args);
	public event ClockChangeHandler ClockChange;
	
	void Awake() {
		scale = 1;
		
		year = 1;
		month = 1;
		day = 1;

		skin = Resources.Load("Skins/DefaultSkin") as GUISkin;
	}

	// Use this for initialization
	void Start() {
		Game.networkManager.Send(
			UpdateTimeProtocol.Prepare(),
			ProcessUpdateTime
		);

		Game.networkManager.Listen(
			NetworkCode.UPDATE_TIME,
			ProcessUpdateTime
		);
	}
	
	// Update is called once per frame
	void Update() {
		currentTime += Time.deltaTime * scale;

		if (currentTime >= second + 1) {
			second = (int) currentTime;

			year = second / Constants.MONTH_DURATION / 12 + 1;
			month = ((second / Constants.MONTH_DURATION) + 1) % 13;
			day = second / (Constants.MONTH_DURATION / 30) % 30 + 1; 

			if (ClockChange != null) {
				ClockChange(this, new ClockEventArgs(year, month, day, second));
			}
		}
	}

	void OnDestroy() {
		Game.networkManager.Ignore(
			NetworkCode.UPDATE_TIME,
			ProcessUpdateTime
		);
	}
	
	void OnGUI() {

	}

	public void ProcessUpdateTime(NetworkResponse response) {
		ResponseUpdateTime args = response as ResponseUpdateTime;
		currentTime = (args.day - 1) * Constants.MONTH_DURATION / 30;
		scale = args.rate;
	}
}
