/* 
 * File Name: GameController.cs
 * Description: Main script for the demo.
 */ 


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SD {
    public class GameController : MonoBehaviour {

        public GameObject Prey;
        public Vector3 spawnValue;
        public GameObject[] preyArray;

        // Number of prey spawn at a game start
        public int numPrey;

        public Text scoreText;
        public Text UnscoredPointText; 
        public Text staminaText;
        public Text healthText;
        public Text opponentScoreText;

        private int score;          // Player's score
        private int unscoredPoint;
        private int opponentScore;
        public float stamina;       // Player's stamina
        public int health;
        private const float MaxStamina = 100;
        private float staminaRecoveryRate = 0.02f;

        public Boundary boundary;
        public Rigidbody player;
        public Rigidbody opponent;
        public Rigidbody playerBase;
        public Rigidbody opponentBase;
        private Vector3 playerInitialPosition = new Vector3(-100,0,0);
        private Quaternion playerInitialRotation = Quaternion.Euler(0,90,0);
        private Vector3 opponentInitialPosition = new Vector3 (100, 0, 0);
        private Quaternion opponentInitialRotation = Quaternion.Euler (0, -90, 0);
        private Vector3 playerBaseInitialPosition = new Vector3(-260,0,0);
        private Quaternion playerBaseInitialRotation = Quaternion.Euler(0,0,0);
        private Vector3 opponentBaseInitialPosition = new Vector3(260,0,0);
        private Quaternion opponentBaseInitialRotation = Quaternion.Euler(0,0,0);

        private static GameController gameController;
        private static GameManager sdGameManager;
        private PlayTimePlayer currentPlayer;
        private PlayTimePlayer opponentPlayer;
        private Rigidbody rbOpponent;
        private Dictionary <int, NPCFish> npcFishes = new Dictionary<int, NPCFish>();
        private Dictionary <int, GameObject> npcFishObjects = new Dictionary<int, GameObject>();
        private int maxPreyId;
        private bool hasSurrendered;
        private bool isGameTimeTicking = false;

        public GameObject surrenderPanelCanvas;
        public GameObject countdownPanelCanvas;


        void Awake () {
            gameController = this;
        }
        // Initializes the player's score, and UI texts.
        // Also spawns numbers of prey at random positions.
        void Start () {

            if (Constants.PLAYER_NUMBER == 2) {  // The player who joins the host will have a different position to start from.
                swapPositions();
            }
            Rigidbody playerClone = (Rigidbody)Instantiate (player, playerInitialPosition, playerInitialRotation);
            Rigidbody playerBaseClone = (Rigidbody)Instantiate (playerBase, playerBaseInitialPosition, playerBaseInitialRotation);
            Rigidbody opponentBaseClone = (Rigidbody)Instantiate (opponentBase, opponentBaseInitialPosition, opponentBaseInitialRotation);
            score = 0;
            hasSurrendered = false;
            opponentScore = 0;
            unscoredPoint = 0; 
            UpdateScore ();
            UpdateOpponentScore ();
            UpdateStamina ();
            UpdateUnscoredPoint ();
            UpdateHealth ();

            sdGameManager = SD.GameManager.getInstance ();
            currentPlayer = new PlayTimePlayer ();

            for (int i = 1; i <= numPrey; i++) {
                NPCFish npcFish = new NPCFish (i);
                npcFishes [i] = npcFish;
                maxPreyId = i;
                if (SDMain.networkManager != null) {
                    sdGameManager.FindNPCFishPosition (i); // Finds and spawns prey at the returned location.
                } else {
                    spawnPrey (i, Random.Range(0, preyArray.Length-1));
                }
            }

            if (SDMain.networkManager != null) {  // We are playing multiplayer
                rbOpponent = (Rigidbody)Instantiate (opponent, opponentInitialPosition, opponentInitialRotation);
                rbOpponent.gameObject.SetActive (true);
                opponentPlayer = new PlayTimePlayer ();
                opponentPlayer.speedUpFactor = playerClone.GetComponent<PlayerController> ().speedUpFactor;
                opponentPlayer.yRotation = opponentInitialRotation.eulerAngles.y;
                isGameTimeTicking = false; // Wait for time sync if in multiplayer mode
                gameController.countdownPanelCanvas.SetActive (true);
            } else {
                isGameTimeTicking = true; // Start the timer immediately if in offline mode
                gameController.countdownPanelCanvas.SetActive (false);
            }

        }

        // Automatically revovers stamina, and refreshs staminaText UI every frame.
        void Update() {
            if (getIsGameTimeTicking ()) {
                RecoverStamina ();
                UpdateStamina ();
                UpdateUnscoredPoint ();
                UpdateOpponentScore ();
                UpdateHealth ();
            }
        }

        public static GameController getInstance() {
            return gameController;
        }

        // Swaps the positions and rotations of the players and bases for the opponent's view.
        private void swapPositions() {
            Vector3 tempV = playerInitialPosition;
            playerInitialPosition = opponentInitialPosition;
            opponentInitialPosition = tempV;

            tempV = playerBaseInitialPosition;
            playerBaseInitialPosition = opponentBaseInitialPosition;
            opponentBaseInitialPosition = tempV;

            Quaternion tempQ = playerInitialRotation;
            playerInitialRotation = opponentInitialRotation;
            opponentInitialRotation = tempQ;

            tempQ = playerBaseInitialRotation;
            playerBaseInitialRotation = opponentBaseInitialRotation;
            opponentBaseInitialRotation = playerBaseInitialRotation;
        }

        // Spawns prey at a random position within the boundary
        public void spawnPrey(int i, int preyIndex){
            Vector3 spawnPosition;
            if (npcFishes [i].xPosition != 0 && npcFishes [i].yPosition != 0) {
                spawnPosition = new Vector3 (npcFishes [i].xPosition, npcFishes [i].yPosition, npcFishes[i].xRotationAngle);
                Debug.Log ("Spawning NPCFish " + i + " from request result");
            } else {
                spawnPosition = new Vector3 (Random.Range(boundary.xMin, boundary.xMax), Random.Range(boundary.yMin, boundary.yMax), 0);
                Debug.Log ("Spawning NPCFish " + i + " from local random numbers");
            }
            Quaternion spawnRotation = Quaternion.Euler(0, 90,0);
            npcFishObjects [i] = Instantiate (preyArray[preyIndex], spawnPosition, spawnRotation) as GameObject;
            npcFishObjects [i].name = "NPCFish_" + preyIndex + "_" + i;
            npcFishObjects [i].SetActive (true);
            // Associate the metadata of the prey with the gameobject.
            npcFishObjects[i].GetComponent<NPCFishController>().setNPCFishData(npcFishes[i]);
        }

        public void destroyPrey(int i) {
            if (npcFishObjects [i] != null) {
                Destroy (npcFishObjects [i]);
            }
        }

        // Spawns 'num' Npc fish of type 'speciesId'
        public void spawnNpcSet(int speciesId, int num) {
            int startId = maxPreyId + 1;
            for (int i = startId; i < (startId + num); i++) {
                Vector3 spawnPosition;
                NPCFish npcFish = new NPCFish (i);
                npcFishes [i] = npcFish;
                maxPreyId = i;
                // TODO: Change random position to out-of-screen position once the movement is added.
                spawnPosition = new Vector3 (Random.Range(boundary.xMin, boundary.xMax), Random.Range(boundary.yMin, boundary.yMax), 0);
                // set the attributes of the npc fish to spawn.
                npcFish.xPosition = spawnPosition.x;
                npcFish.yPosition = spawnPosition.y;
                npcFish.speciesId = speciesId;
                spawnPrey (i, speciesId);
            }
        }
        // Increases the current score value, and pass the info to scoreText
        // by calling UpdateScore().
        public void AddScore(int newScoreValue) {
            score += newScoreValue;
            UpdateScore ();
            // Send the score to the opponent.
            sdGameManager.SendScoreToOpponent(score);
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

        void UpdateOpponentScore() {
            opponentScoreText.text = "Opponent: " + opponentScore;
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
            // Send the score to the opponent.
            if (this.unscoredPoint != 0)  // to send the request only once.
                sdGameManager.SendScoreToOpponent(score);
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
            hasSurrendered = true;
            sdGameManager.EndGame (false, score);
        }

        public Rigidbody getOpponent() {
                return rbOpponent;
        }

        public PlayTimePlayer getCurrentPlayer() {
            return currentPlayer;
        }
        public PlayTimePlayer getOpponentPlayer() {
            return opponentPlayer;
        }

        public Dictionary<int, NPCFish> getNpcFishes() {
            return npcFishes;
        }

        public Dictionary<int, GameObject> getNpcFishObjects() {
            return npcFishObjects;
        }

        public void hideSurrenderPanel(){
            surrenderPanelCanvas.SetActive(false);
        }

        public int getPlayerScore() {
            return score;
        }

        public bool getHasSurrendered() {
            return hasSurrendered;
        }

        public void setOpponentScore(int opScore) {
            opponentScore = opScore;
        }

        public bool getIsGameTimeTicking() {
            return isGameTimeTicking;
        }

        public void setIsGameTimeTicking(bool isTicking) {
            isGameTimeTicking = isTicking;
        }

        public int getMaxPreyId() {
            return maxPreyId;
        }
    } 

}
