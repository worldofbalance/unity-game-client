/*
 * File Name: MainCameraController.cs
 * Date: Created on Arp 7 2016
 * Description: Moves the main camera along the player's movement
 *
 */

using UnityEngine;
using System.Collections;

namespace SD {
public class MainCameraController : MonoBehaviour {

    private GameObject player;
    private Vector3 playerPosition;
    private Vector3 cameraPosition;
    private GameObject mainCamera;

    void Start(){
        mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
        playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
        mainCamera.transform.position = playerPosition;
        mainCamera.transform.position = new Vector3(0f,0f,-25f);
    }

    void Update() {
        playerPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
        mainCamera.transform.position = playerPosition;
        cameraPosition = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y, -25f);
        mainCamera.transform.position = cameraPosition;
    }
}
}