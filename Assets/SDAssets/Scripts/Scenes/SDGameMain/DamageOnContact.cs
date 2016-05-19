/* 
 * File Name: DamageOnContact.cs
 * Description: The player takes a damage when contacting to non-prey objects or eating poisonous fish.
 */

using UnityEngine;
using System.Collections;

namespace SD {
    public class DamageOnContact : MonoBehaviour {

        private GameController gameController;
        private const int damage = -20; // Score to be recieved by eating prey
        private GameObject mainCamera;
        private AudioSource audioSource;
        public AudioClip audioClip;
        public GameObject bleeding;
        private float lastDamage = 0;
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
                Instantiate (bleeding, other.transform.position, Quaternion.identity);
                StartCoroutine (destroyBleeding ());
                AudioSource.PlayClipAtPoint (audioClip, mainCamera.transform.position);
                if (gameController.GetHealth() > 0) {
                    gameController.UpdateHealth (damage);
                }
                lastDamage = 0;
            }
        }

        void OnTriggerStay(Collider other) {
            if (gameObject.tag == "Predator" && other.tag == "Player") {
                lastDamage += Time.deltaTime;
                if (lastDamage >= 2) {
                    Instantiate (bleeding, other.transform.position, Quaternion.identity);
                    StartCoroutine (destroyBleeding ());
                    AudioSource.PlayClipAtPoint (audioClip, mainCamera.transform.position);
                    if (gameController.GetHealth() > 0) {
                        gameController.UpdateHealth (damage);
                    }
                    lastDamage = 0;
                }
            }
        }

        IEnumerator destroyBleeding(){
            yield return new WaitForSeconds (3.85f);
            Destroy (GameObject.FindGameObjectWithTag ("Bubbles"));
        }
    }
}
