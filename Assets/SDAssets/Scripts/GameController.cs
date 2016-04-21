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
    public Text staminaText;

    private int score;          // Player's score
    public float stamina;       // Player's stamina
    private const float MaxStamina = 100;
    private float staminaRecoveryRate = 0.02f;

    public Boundary boundary;
   
    // Initializes the player's score, and UI texts.
    // Also spawns numbers of prey at random positions.
    void Start () {
        Debug.Log ("gc started");
        score = 0;
        UpdateScore ();
        UpdateStamina ();
        for (int i = 0; i < numPrey; i++) {
            spawnPrey ();
        }    
    }

    // Automatically revovers stamina, and refreshs staminaText UI every frame.
    void Update() {
        RecoverStamina ();
        UpdateStamina ();
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
}
