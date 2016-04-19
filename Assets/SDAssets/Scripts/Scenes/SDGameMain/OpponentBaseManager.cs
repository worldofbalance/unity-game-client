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
        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                //player.isStatic = false;
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