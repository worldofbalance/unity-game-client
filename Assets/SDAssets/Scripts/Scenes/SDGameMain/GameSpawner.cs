using UnityEngine;
using System.Collections;

public class GameSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GameObject[] prey = GameObject.FindGameObjectsWithTag("Prey");

        //if number of prey falls below 20 add a new prey at a random location
        if (prey.Length<=20)
        {
            //figure out how to spawn prey 
            GameObject newprey = (GameObject)Instantiate(Resources.Load("SDAssets/Prefabs/Prey"), new Vector3(Random.Range(0, 20), Random.Range(0, 20), 0f), Quaternion.identity);
        }
    }
}
