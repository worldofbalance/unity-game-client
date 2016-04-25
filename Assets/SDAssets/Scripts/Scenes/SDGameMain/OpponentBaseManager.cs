using UnityEngine;
using System.Collections;

namespace SD {
    public class OpponentBaseManager : MonoBehaviour {

        private GameController gameController;
        private GameObject player;
        private GameObject opponentBaseL, opponentBaseLR;
        // Use this for initialization
        void Start () {
            gameController = GameController.getInstance ();
            player = GameObject.FindGameObjectWithTag ("Player");
            opponentBaseL = GameObject.Find ("OpponentBaseL");
            opponentBaseL = GameObject.Find ("OpponentBaseR");
            player.GetComponent<Rigidbody> ();
        }

        // Update is called once per frame
        void Update () {
            if (Input.GetKey (KeyCode.UpArrow)){
                //player.isStatic = false;
            }
        }

        // Runs when the player collides the base
        void OnTriggerEnter(Collider other) {
            bool fromLeft;
            bool fromTop;
            if (other.tag == "Player") {
                if (player.transform.position.x < -50) {                
                    fromLeft = true;
                } 
                if (player.transform.position.y > 10) {
                    fromTop = true;
                }
            }
            
        }

        void OnTriggerExit(Collider other) {
            if (other.tag == "Player") {
                //player.isStatic = false;
            }
        }
            

        // Runs while the player is staying in the base
        void OnTriggerStay(Collider other) {
            if (other.tag == "Player") {

            }
        }
    }
}