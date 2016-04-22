using UnityEngine;
using System.Collections;

namespace SD {
    public class SDPersistentData : MonoBehaviour {

        private int playerFinalScore;
        private int opponentFinalScore;
        private int roundsCompleted;
        private bool isGameCompleted;
        private static SDPersistentData sdPersistentData;

        void Awake() {
            sdPersistentData = this;
        }

        void Start () {
            DontDestroyOnLoad (this);
            initializeData ();
        }

        void Update () {

        }

        void initializeData() {
            setPlayerFinalScore (0);
            setOpponentFinalScore (0);
            setRoundsCompleted (0);
            setIsGameCompleted (false);  // is the current game completed.
        }

        public static SDPersistentData getInstance() {
            return sdPersistentData;
        }

        public void setPlayerFinalScore(int score) {
            playerFinalScore = score;
        }

        public int getPlayerFinalSCore() {
            return playerFinalScore;
        }

        public void setOpponentFinalScore(int score) {
            opponentFinalScore = score;
        }

        public int getOpponentFinalScore() {
            return opponentFinalScore;
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
    }
}
