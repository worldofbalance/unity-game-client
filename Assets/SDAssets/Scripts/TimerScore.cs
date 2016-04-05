/*
 * File Name: TimerScore.cs
 * Description: Script for TimeText. This sciprt works as a timer.
 */


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerScore : MonoBehaviour {

    public Text timeGUI;
    public float time = 180f;

    public void Start(){
        timeGUI.text = "Time: " +time.ToString ();
    }

    void Update() {
        time -= 1f * Time.deltaTime;
        timeGUI.text = "Time: " +((int)time).ToString ();
    }

}