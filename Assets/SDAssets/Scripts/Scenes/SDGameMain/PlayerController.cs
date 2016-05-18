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
    private const int MindSpeed = 40;
    private float speed = 45;
    public float speedUpFactor = 1.5f;
    private const int MaxSpeed = 80;
    private float turnSpeed = 10f;
    private Vector3 turn;
    private Vector3 goUpDown;

    private float movementHorizontal;
    private float movementVertical;
    private float xRotationAngle;
    private Vector3 target;

    public Boundary boundary;
    private Vector3 scale;
    public int tilt;

    private GameController gameController;

    private float currentStamina;
    private const float MinimunStamina = 10;

    private static SD.GameManager sdGameManager;
    private float oldXPosition, oldYPosition;
    private float oldYRotation;
    private int turnCounter = 0;
	// Detects the player object, and reads the 'GameController' Object
    void Start () {
        rb = GetComponent<Rigidbody> ();
        scale = rb.transform.localScale;
        turn = new Vector3 (turnSpeed,0f , 0f);
        goUpDown = new Vector3 (turnSpeed/2, 0f, 0f);
        oldXPosition = oldYPosition = 0.0f;

        sdGameManager = SD.GameManager.getInstance ();
        gameController = GameController.getInstance ();
	}
	

	// Update with anykind of physics calculation
	void FixedUpdate () {
        // Gets arrow key input
        if (gameController.getIsGameTimeTicking()) {
            movementHorizontal = Input.GetAxis("Horizontal");
            movementVertical = Input.GetAxis ("Vertical");
            turn = new Vector3(0f, turnSpeed, 0f);
            Vector2 movement = new Vector2(movementHorizontal, movementVertical);
            // Send x and y position to the opponent if they have changed.
            if (rb.position.x != oldXPosition || rb.position.y != oldYPosition) {
                sdGameManager.SetPlayerPositions (rb.position.x, rb.position.y, xRotationAngle);
            }
            oldXPosition = rb.position.x;
            oldYPosition = rb.position.y;

            // Assigns the player's movement speed, and move the player object
            rb.velocity = movement * speed;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                gameController.getCurrentPlayer ().xPosition = transform.position.x;
                gameController.getCurrentPlayer ().yPosition = transform.position.y;
                speed = speed * 0.95f;// speed down after movement 
                if (speed < 40) { speed = 40; }
                currentStamina++;
                rb.position = new Vector3 (
                Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp (rb.position.y, boundary.yMin, boundary.yMax),
                0.0f
            );

            if (rb.velocity.x > 0)
            {
                if (rb.transform.rotation.eulerAngles.y > 90) { // Right turn
                    float eulerX = rb.transform.rotation.eulerAngles.x;  // Required to support syncing orientation for keyboard
                    if (eulerX >= 270 && eulerX <= 360)
                        xRotationAngle = 360 - eulerX;
                    else if (eulerX >= 0 && eulerX <= 90)
                        xRotationAngle = -eulerX;
                    rb.transform.Rotate (-turn);
                }
            } 
            if (rb.velocity.x < 0)
            {
                if (rb.transform.rotation.eulerAngles.y < 270) { // Left turn
                    float eulerX = rb.transform.rotation.eulerAngles.x;
                    if (eulerX >= 270 && eulerX <= 360)
                        xRotationAngle = eulerX - 180;
                    else if (eulerX >= 0 && eulerX <= 90)
                        xRotationAngle = eulerX - 180;
                    rb.transform.Rotate (turn);
                }
            }
                // Flips the player object left or right
                // depending on the direction the player is moving
                // Moving to Right

                //rb.rotation = Quaternion.Euler (0.0f, rb.velocity.z * -tilt,  0.0f);
                
            }
    }
        
    // So far, the Update function only manages temporally speed up of the player
    // by pressing a space bar.
    // *Incomplete: the player can still speed up even if their stamina goes down to 0
    //              as long as the player keeps on presing a space bar
    void Update(){
        if (gameController.getIsGameTimeTicking ()) {
            currentStamina = gameController.GetStamina ();
       
            if (Input.GetMouseButton (0)) {
                var mouse = Input.mousePosition;
                var screenPoint = Camera.main.WorldToScreenPoint (transform.localPosition);
                var offset = new Vector2 (mouse.x - screenPoint.x, mouse.y - screenPoint.y);
                var angle = Mathf.Atan2 (offset.y, offset.x) * Mathf.Rad2Deg;
                var yAngle = -90;
                xRotationAngle = angle;
                if (angle >= -90 && angle <= 90) {
                    // invert the angle to avoid upside down movement.
                    angle = 180 - angle;
                    yAngle = 90;
                }

                transform.rotation = Quaternion.Euler (angle - 180, yAngle, 0);
                mouse.z = transform.position.z - Camera.main.transform.position.z;
                mouse = Camera.main.ScreenToWorldPoint (mouse);
                target = mouse;

           
                //  transform.position = Vector3.MoveTowards(transform.position, mouse, speed * Time.deltaTime);
            }
                if (Input.GetKey(KeyCode.Space) && currentStamina > 0)
                {
                    speed = speed * speedUpFactor;
                    if (speed > MaxSpeed)
                    {
                        speed = MaxSpeed;
                    }
                    gameController.SetStamina(currentStamina - .25f);
                }

            }
    }

    // Returns true if the player is moving.
    // Otherwise returns false.
    bool isMoving(){
            if (rb.velocity.x != 0 || rb.velocity.y != 0) {
                return true;
            }
            return false;
    }


}
}

