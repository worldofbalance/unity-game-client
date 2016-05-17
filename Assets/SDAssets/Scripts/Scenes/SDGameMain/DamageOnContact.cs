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
        private GameObject mainCamera;
        private AudioSource audioSource;
        public AudioClip audioClip;

        // Use this for initialization
        void Start () {
            gameController = GameController.getInstance ();
            audioSource = GetComponent<AudioSource> ();
            mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
            audioSource.clip = audioClip;
        }


        // Update is called once per frame
        void Update () {

        }
            
        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                Debug.Log (mainCamera.transform.position);
                AudioSource.PlayClipAtPoint (audioClip, mainCamera.transform.position);
                if (gameController.GetHealth() > 0) {
                    gameController.UpdateHealth (damage);
                }
            }
            }
        }
    }
