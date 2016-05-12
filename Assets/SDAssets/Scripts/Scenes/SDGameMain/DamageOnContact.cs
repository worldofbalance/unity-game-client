/* 
 * File Name: DamageOnContact.cs
 * Description: The player takes a damage when contacting to non-prey objects or eating poisonous fish.
 */

using UnityEngine;
using System.Collections;

namespace SD {
    public class DamageOnContact : MonoBehaviour {

        private GameController gameController;
        private const int damage = -10; // Score to be recieved by eating prey

        // Use this for initialization
        void Start () {
            gameController = GameController.getInstance ();
        }


        // Update is called once per frame
        void Update () {

        }
            
        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                if (gameController.GetHealth() > 0) {
                    gameController.UpdateHealth (damage);
                }
            }
            }
        }
    }
