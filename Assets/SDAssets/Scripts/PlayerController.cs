/*
 * File Name: PlayerController.cs
 * Description: Takes care of player's movement, direction, and speed. 
 *
 */


using UnityEngine;
using System.Collections;


// Takes care of the size of boundary of the entire screen
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    Rigidbody rb;
    private int speed = 20;
    public int speedUpFactor;
    private const int MaxSpeed = 30;

    private float movementHorizontal;
    private float movementVertical;

    public Boundary boundary;
    private Vector3 scale;
    public int tilt;

    public GameController gameController;

    private float currentStamina;
    private const float MinimunStamina = 10;

	// Detects the player object, and reads the 'GameController' Object
    void Start () {
        rb = GetComponent<Rigidbody> ();
        scale = rb.transform.localScale;

        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        } else {
            Debug.Log ("Game Controller not found");
        }
	}
	

	// Update with anykind of physics calculation
	void FixedUpdate () {
        // Gets arrow key input
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (movementHorizontal, movementVertical, 0.0f);

        // Assigns the player's movement speed, and move the player object
        rb.velocity = movement * speed;
        rb.position = new Vector3 (
            Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp (rb.position.y, boundary.yMin, boundary.yMax),
            0.0f
        );

        // Flips the player object left or right
        // depending on the direction the player is moving
        if (rb.velocity.x > 0) {
            scale.z = 1;
        } 
        if (rb.velocity.x < 0) {
            scale.z = -1;
        }
        transform.localScale = scale;

        //rb.rotation = Quaternion.Euler (0.0f, rb.velocity.z * -tilt,  0.0f);
	}
        
    // So far, the Update function only manages temporally speed up of the player
    // by pressing a space bar.
    // *Incomplete: the player can still speed up even if their stamina goes down to 0
    //              as long as the player keeps on presing a space bar
    void Update(){
        currentStamina = gameController.GetStamina();
        
        if (Input.GetKeyDown (KeyCode.Space)) {
            speed = speed * speedUpFactor;
        }

        if(Input.GetKey (KeyCode.Space) && isMoving() && currentStamina >=0) {
            currentStamina = currentStamina - 0.2f;
            gameController.SetStamina (currentStamina);
        } 

        if (Input.GetKeyUp (KeyCode.Space)) {
            speed = speed / speedUpFactor;
        }
    }

    // Returns true if the player is moving.
    // Otherwise returns false.
    bool isMoving(){
        if (movementVertical != 0 || movementHorizontal != 0) {
            return true;
        } else {
            return false;
        }
    }
}
