using UnityEngine;
using System.Collections;

namespace RR {
	public class SpinPlatform : MonoBehaviour {
		
		// Update is called once per frame
		void Update () {
			float speed = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
			transform.Rotate(0, speed + 1, 0);
		}
	}
}