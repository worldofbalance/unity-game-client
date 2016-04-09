/* 
 * File Name: GameController.cs
 * Description: Main script for the demo.
 */ 


using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameController : MonoBehaviour {

    public GameObject Prey;
    public Vector3 spawnValue;

    // Number of prey spawn at a game start
    public int numPrey;

    public Text scoreText;
    public Text UnscoredPointText; 
    public Text staminaText;

    private int score;          // Player's score
    private int unscoredPoint;
    public float stamina;       // Player's stamina
    private const float MaxStamina = 100;
    private float staminaRecoveryRate = 0.02f;

    public Boundary boundary;
    public Rigidbody player;
    private Vector3 playerInitialPosition = new Vector3(0,0,0);
    private Quaternion playerInitialRotation = Quaternion.Euler(0,90,0);

    // Initializes the player's score, and UI texts.
    // Also spawns numbers of prey at random positions.
    void Start () {
        Rigidbody playerClone = (Rigidbody)Instantiate (player, playerInitialPosition, playerInitialRotation);
        score = 0;
        unscoredPoint = 0; 
        UpdateScore ();
        UpdateStamina ();
        UpdateUnscoredPoint ();
        for (int i = 0; i < numPrey; i++) {
            spawnPrey ();
        }    
    }

    // Automatically revovers stamina, and refreshs staminaText UI every frame.
    void Update() {
        RecoverStamina ();
        UpdateStamina ();
        UpdateUnscoredPoint ();
        Debug.Log (score);
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


    public int GetUnscored(){
        return this.unscoredPoint;
    }

    public void ResetUnscored(){
        this.unscoredPoint = 0;
    }

    // Adds unscored points to player's actual score
    public void Score(){
        this.score += this.unscoredPoint;
        UpdateScore ();
    }
}
