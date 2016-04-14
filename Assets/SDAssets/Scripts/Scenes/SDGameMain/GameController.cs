/* 
 * File Name: GameController.cs
 * Description: Main script for the demo.
 */ 


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SD {
    public class GameController : MonoBehaviour {

        public GameObject Prey;
        public Vector3 spawnValue;

        // Number of prey spawn at a game start
        public int numPrey;

        public Text scoreText;
        public Text UnscoredPointText; 
        public Text staminaText;
        public Text healthText;

        private int score;          // Player's score
        private int unscoredPoint;
        public float stamina;       // Player's stamina
        public int health;
        private const float MaxStamina = 100;
        private float staminaRecoveryRate = 0.02f;

        public Boundary boundary;
        public Rigidbody player;
        private Rigidbody opponent;

        private Vector3 playerInitialPosition = new Vector3(0,0,0);
        private Quaternion playerInitialRotation = Quaternion.Euler(0,90,0);

        private static SD.GameManager sdGameManager;
        private PlayTimePlayer opponentPlayer;
        private bool isMultiplayer = false;

        // Initializes the player's score, and UI texts.
        // Also spawns numbers of prey at random positions.
        void Start () {
            Rigidbody playerClone = (Rigidbody)Instantiate (player, playerInitialPosition, playerInitialRotation);
            score = 0;
            unscoredPoint = 0; 
            UpdateScore ();
            UpdateStamina ();
            UpdateUnscoredPoint ();
            UpdateHealth ();
            for (int i = 0; i < numPrey; i++) {
                spawnPrey ();
            }

            sdGameManager = SD.GameManager.getInstance ();
            if (sdGameManager.getConnectionManager ()) {  // We might be playing multiplayer TODO: Check position response from opponent.
                opponent = (Rigidbody)Instantiate (player, playerInitialPosition, playerInitialRotation);
                opponent.gameObject.GetComponent<PlayerController> ().enabled = false;
                opponentPlayer = new PlayTimePlayer ();
                opponentPlayer.speedUpFactor = playerClone.GetComponent<PlayerController> ().speedUpFactor;
                opponent.name = "Opponent";
                opponent.gameObject.tag = "Opponent";
                isMultiplayer = true;
            }

        }

        // Automatically revovers stamina, and refreshs staminaText UI every frame.
        void Update() {
            RecoverStamina ();
            UpdateStamina ();
            UpdateUnscoredPoint ();
            UpdateHealth ();
        }

        // Spawns prey at a random position within the boundary
        void spawnPrey(){
            Vector3 spawnPosition = new Vector3 (Random.Range(boundary.xMin, boundary.xMax), 
                Random.Range(boundary.yMin, boundary.yMax), 0);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate (Prey, spawnPosition, spawnRotation);
        }


        // Increases the current score value, and pass the info to scoreText
        // by calling UpdateScore().
        public void AddScore(int newScoreValue) {
            score += newScoreValue;
            UpdateScore ();
        }

        // Updates scoreText UI.
        void UpdateScore () {
            scoreText.text = "Score: " + score;
        }

        public void AddUnscoredPoint(int newScoreValue) {
            unscoredPoint += newScoreValue;
            UpdateUnscoredPoint ();
        }

        // Updates scoreText UI.
        void UpdateUnscoredPoint() {
            UnscoredPointText.text = "Unscored Point: " + unscoredPoint;
        }

        public int GetUnscored(){
            return this.unscoredPoint;
        }

        public void ResetUnscored(){
            this.unscoredPoint = 0;
        }

        // Updates staminaText UI with no decimal place.
        void UpdateStamina() {
            staminaText.text = "Stamina: " + stamina.ToString("F0");
        }

        // Recovers the current stamina 
        void RecoverStamina(){
            stamina = stamina + staminaRecoveryRate;
            if (stamina >= MaxStamina)
                stamina = MaxStamina;
        }

        // Returns the current stamina
        public float GetStamina(){
            return this.stamina;
        }

        // Sets stamina
        public void SetStamina(float newStamina){
            this.stamina = newStamina;
        }

        // Sets score
        public void SetScore(int newScore){
            this.score = newScore;
        }

        // Adds unscored points to player's actual score
        public void Score(){
            this.score += this.unscoredPoint;
            UpdateScore ();
        }

        public int GetHealth(){
            return this.health;
        }

        public void SetHealth(int newHealth){
            this.health = newHealth;
        }

        void UpdateHealth(){
            healthText.text = "Health: " + health;
        }

        public void BtnSurrenderClick() {
            sdGameManager.EndGame (false, score);
        }

        public Rigidbody getOpponent() {
                return opponent;
        }
        
        public PlayTimePlayer getOpponentPlayer() {
            return opponentPlayer;
        }

        public bool getIsMultiplayer() {
            return isMultiplayer;
        }
    }
}
