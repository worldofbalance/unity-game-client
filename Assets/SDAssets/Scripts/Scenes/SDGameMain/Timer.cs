/*
 * File Name: TimerScore.cs
 * Description: Script for TimeText. This sciprt works as a timer.
 */


using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace SD {
    public class Timer : MonoBehaviour {

        public Text timeText;
        public Text countdownText;
        public float time = 180f;
        private static GameManager sdGameManager;
        private static GameController sdGameController;
        private static SDPersistentData sdPersistentData;
        private bool hasTimerStarted;

        public void Start(){
            timeText.text = "Time: " +time.ToString ();
            sdGameManager = GameManager.getInstance ();
            sdGameController = GameController.getInstance ();
            sdPersistentData = SDPersistentData.getInstance ();
            hasTimerStarted = false;
        }

        void Update() {
            if (!hasTimerStarted && sdPersistentData != null) {
                if (DateTime.Equals (TrimMilliseconds (DateTime.UtcNow), sdPersistentData.getRoundStartTime ())) {
                    sdGameController.setIsGameTimeTicking (true);
                    Debug.Log ("Starting the timer now at: " + DateTime.UtcNow);
                    hasTimerStarted = true;
                    sdGameController.countdownPanelCanvas.SetActive (false);
                } else {
                    double secondsToGo = (sdPersistentData.getRoundStartTime () - TrimMilliseconds (DateTime.UtcNow)).TotalSeconds;
                    if (secondsToGo >= 0)
                        countdownText.GetComponent<Text>().text = secondsToGo.ToString ();
                }
            }
            if (sdGameController.getIsGameTimeTicking ()) {
                time -= 1f * Time.deltaTime;
                timeText.text = "Time: " + ((int)time).ToString ();
                if (time <= 0) {
                    timeText.text = "Time's Up!";
                    if (time <= -3) {
                        sdGameManager.EndGame (true, sdGameController.getPlayerScore ());
                        Debug.Log ("The player's final score is " + sdGameController.getPlayerScore ());
                        Destroy (this);
                    }
                }
            }
            
        }

        float GetTime (){
            return this.time;
        }
        
        public static DateTime TrimMilliseconds(DateTime dt) {
            return new DateTime (dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }
    }
}