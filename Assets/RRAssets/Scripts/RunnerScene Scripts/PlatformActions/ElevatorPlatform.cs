using UnityEngine;
using System.Collections;

namespace RR {
	public class ElevatorPlatform : MonoBehaviour {
	
		private float initial_posY;
	
		[HideInInspector]
		private bool up;
	
		// Use this for initialization
		void start() {
			initial_posY = transform.position.y;
			up = true;
		}
	
		// Update is called once per frame
		void Update () {
			float speed = 5.0f * Time.deltaTime;
	
			if (transform.position.y >= 5 + initial_posY) {
				//transform.Translate (0, (0 - speed) , 0);		
				up = false;
			} else if (transform.position.y <= initial_posY - 5) {
				//transform.Translate(0, speed, 0);
				up = true;
			}
	
			if (up) {
				transform.Translate(0, speed, 0);
			} else {
				transform.Translate (0, (0 - speed) , 0);
			}
		}
	}
}