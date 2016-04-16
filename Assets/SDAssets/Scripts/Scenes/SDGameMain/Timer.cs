/*
 * File Name: TimerScore.cs
 * Description: Script for TimeText. This sciprt works as a timer.
 */


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SD {
public class Timer : MonoBehaviour {

    public Text timeText;
    public float time = 180f;

    public void Start(){
        timeText.text = "Time: " +time.ToString ();
    }

    void Update() {
        time -= 1f * Time.deltaTime;
        timeText.text = "Time: " +((int)time).ToString ();
        if (time <= 0) {
                timeText.text = "Time's Up!";
                if (time <= -3) {
                    Game.SwitchScene ("SDReadyScene");
                }
        }
    }

        float GetTime(){
            return this.time;
        }
}
}