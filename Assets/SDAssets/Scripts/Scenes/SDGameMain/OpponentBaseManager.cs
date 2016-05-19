using UnityEngine;
using System.Collections;

namespace SD {
    public class OpponentBaseManager : MonoBehaviour {

        private GameController gameController;
        private GameObject player;
        // Use this for initialization
        void Start () {
            gameController = GameController.getInstance ();
            player = GameObject.FindGameObjectWithTag ("Player");
            player.GetComponent<Rigidbody> ();
        }

        // Update is called once per frame
        void Update () {
            if (Input.GetKey (KeyCode.UpArrow)){
                //player.isStatic = false;
            }
        }

        // Runs when the player collides the base
        // Pushes back the player if they colliede to the opponent base
        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                int xDirection;
                int yDirection;

                if (player.transform.position.x > 10) {
                    xDirection = -1;
                } else {
                    xDirection = 1;
                }

                if (player.transform.position.y > 10) {
                    yDirection = 1;
                } else {
                    yDirection = -1;
                }

                player.transform.position = new Vector3 (player.transform.position.x + (5 * xDirection),
                    player.transform.position.y + (5 * yDirection),
                    player.transform.position.z);

                // Debug.Log (xDirection + "" + yDirection);
            } else if (other.tag == "Opponent") {
                gameController.setIsOpponentInBase (true);
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.tag == "Player") {
                //player.isStatic = false;
            }

            if (other.tag == "Opponent") {
                gameController.setIsOpponentInBase (false);
            }
        }
            

        // Runs while the player is staying in the base
        void OnTriggerStay(Collider other) {
            if (other.tag == "Player") {

            }
        }
    }
}