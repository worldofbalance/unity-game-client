using UnityEngine;
using System.Collections;

namespace SD {
    public class RadorCameraController : MonoBehaviour {

        private GameObject player;
        private Vector3 playerPosition;
        private Vector3 cameraPosition;
        private GameObject mainCamera;

        void Start(){
            mainCamera = GameObject.FindGameObjectWithTag ("RadorCamera");
            //playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position; // causing a NullPointerException
            mainCamera.transform.position = playerPosition;
            mainCamera.transform.position = new Vector3(0f,0f,-35f);
        }

        void Update() {
            playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
            mainCamera.transform.position = playerPosition;
            cameraPosition = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y, -35f);
            mainCamera.transform.position = cameraPosition;
        }
    }
}