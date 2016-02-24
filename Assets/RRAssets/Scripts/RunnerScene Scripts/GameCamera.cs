using UnityEngine;
using System.Collections;
namespace RR {
	public class GameCamera : MonoBehaviour {
	
		private Transform target;
		private float trackSpeed = 10;
	
		// Camera lerp
		public float interpVelocity;
		public float minDistance;
		public float followDistance;
		public Vector3 offset;
		Vector3 targetPos;
	
		public void SetTarget(Transform t) {
			target = t;
		}
	
		void LateUpdate(){
			/*
			if (target) {
	
				float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
				float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);
				transform.position = new Vector3(x,y,transform.position.z);
	
			}
			*/
	
			if (target)
			{
				Vector3 posNoZ = transform.position;
				posNoZ.z = target.position.z;
				
				Vector3 targetDirection = (target.position - posNoZ);
				
				interpVelocity = targetDirection.magnitude * 10f;
				
				targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
				
				transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);
				
			}
		}
	
		// Increase n towards target by speed
		private float IncrementTowards(float n, float target, float a) {
			if (n == target) {
				return n;	
			}
			else {
				float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
				n += a * Time.deltaTime * dir;
				return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
			}
		}
	}
}