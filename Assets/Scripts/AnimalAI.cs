using UnityEngine;
using System.Collections;

public class AnimalAI : MonoBehaviour {
	//Vector3 territoryPos { get; set; }
	Vector3 position;
	bool changeDirection;
	int count;
	GameObject blood;
	
	// Use this for initialization
	void Start () {
		changeDirection = true;
		count = Random.Range(1, 179);
		//bleeding = false;
		//position = territoryPos;
	}
	
	// Update is called once per frame
	void Update () {
		if (count > 180) {
			count = 1;
			changeDirection = !changeDirection;
			this.transform.localScale = new Vector3(-(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
		} else {
			count++;
		}
		
		if (changeDirection) { 
			this.transform.position = new Vector3(this.transform.position.x+0.01f, this.transform.position.y, this.transform.position.z);
		} else {
			this.transform.position = new Vector3(this.transform.position.x-0.01f, this.transform.position.y, this.transform.position.z);
		}
	}
}

