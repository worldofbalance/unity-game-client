/*
 * File Name: PlayerController.cs
 * Description: Takes care of player's movement, direction, and speed. 
 *
 */


using UnityEngine;
using System.Collections;


namespace SD {
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
    private float turnSpeed = 10f;
    private Vector3 turn;
    private Vector3 goUpDown;

    private float movementHorizontal;
    private float movementVertical;
  

    public Boundary boundary;
    private Vector3 scale;
    public int tilt;

    public GameController gameController;

    private float currentStamina;
    private const float MinimunStamina = 10;

    private static SD.GameManager sdGameManager;

	// Detects the player object, and reads the 'GameController' Object
    void Start () {
        rb = GetComponent<Rigidbody> ();
        scale = rb.transform.localScale;
        turn = new Vector3 (turnSpeed,0f , 0f);
        goUpDown = new Vector3 (turnSpeed/2, 0f, 0f);



        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController> ();
        } else {
            Debug.Log ("Game Controller not found");
        }

        sdGameManager = SD.GameManager.getInstance ();
	}
	

	// Update with anykind of physics calculation
	void FixedUpdate () {
        // Gets arrow key input
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (movementHorizontal, movementVertical, 0.0f);
        
        // Send the horizontal and vertical movement factors to the opponent
        sdGameManager.SetPlayerPositions(movementHorizontal, movementVertical);
        // Assigns the player's movement speed, and move the player object
        rb.velocity = movement * speed;

         // Update the velocity of the opponent.
            if (gameController.getIsMultiplayer ()) {
                Vector3 opponentMovement = new Vector3 (
                                           gameController.getOpponentPlayer ().movementHorizontal,
                                           gameController.getOpponentPlayer ().movementVertical,
                                           0.0f);
                gameController.getOpponent ().GetComponent<Rigidbody> ().velocity = opponentMovement * gameController.getOpponentPlayer ().speed;
                Debug.Log ("Opponents velocity is " + gameController.getOpponent ().GetComponent<Rigidbody> ().velocity);
            }

        /*rb.position = new Vector3 (
            Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp (rb.position.y, boundary.yMin, boundary.yMax),
            0.0f
        );*/

        // Flips the player object left or right
        // depending on the direction the player is moving
        // Moving to Right
      var mouse = Input.mousePosition;
             var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
             var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
             var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
             transform.rotation = Quaternion.Euler(angle-180 ,- 90, 0 );
        if (rb.velocity.x > 0) {
            
            //turnSpeed = turnSpeed * -1;
            rb.transform.Rotate (-turn);
            
        } 
        // Moving to Left
        if (rb.velocity.x < 0) {
            
            rb.transform.Rotate (turn);

            
        }
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
            sdGameManager.SetKeyboardActions ((int)KeyCode.Space, 0); // Simple space down
        }

        if(Input.GetKey (KeyCode.Space) && isMoving() && currentStamina >=0) {
            currentStamina = currentStamina - 0.2f;
            gameController.SetStamina (currentStamina);
        } 

        if (Input.GetKeyUp (KeyCode.Space)) {
            speed = speed / speedUpFactor;
                sdGameManager.SetKeyboardActions ((int)KeyCode.Space, 1); // Space up
        }
        if (Input.GetMouseButton(0)) {
            var pos = Input.mousePosition;
            pos.z = transform.position.z - Camera.main.transform.position.z;
            pos = Camera.main.ScreenToWorldPoint(pos);
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
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
}
