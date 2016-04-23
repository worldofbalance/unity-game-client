﻿using UnityEngine;
using System.Collections;

namespace SD {
    public class SDPersistentData : MonoBehaviour {

        private int playerFinalScore;
        private int roundsCompleted;
        private bool isGameCompleted;
        private int winningScore;
        private int gameResult;
        private static SDPersistentData sdPersistentData;

        void Awake() {
            if (sdPersistentData) {
                DestroyImmediate (gameObject);
            } else {
                DontDestroyOnLoad (gameObject);
                sdPersistentData = this;
            }
        }

        void Start() {
            initializeData ();
        }

        void Update () {

        }

        public void initializeData() {
            setPlayerFinalScore (0);
            setRoundsCompleted (0);
            setIsGameCompleted (false);  // is the current game completed.
            setWinningScore(0);
            setGameResult (0);
        }

        public static SDPersistentData getInstance() {
            return sdPersistentData;
        }

        public void setPlayerFinalScore(int score) {
            playerFinalScore = score;
        }

        public int getPlayerFinalScore() {
            return playerFinalScore;
        }

        public void setRoundsCompleted(int rounds) {
            roundsCompleted = rounds;
        }

        public int getRoundsCompleted() {
            return roundsCompleted;
        }

        public void setIsGameCompleted(bool isCompleted) {
            isGameCompleted = isCompleted;
        }

        public bool getIsGameCompleted() {
            return isGameCompleted;
        }

        public void setWinningScore(int score) {
            winningScore = score;
        }

        public int getWinningScore() {
            return winningScore;
        }

        public void setGameResult(int status) {
            gameResult = status;
        }

        public int setGameResult() {
            return gameResult;
        }
    }
}
