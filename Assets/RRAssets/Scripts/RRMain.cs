using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace RR {
	public class RRMain : MonoBehaviour {
	
		void Awake() {
			DontDestroyOnLoad(gameObject);
			
			gameObject.AddComponent<RRMessageQueue>();	
			gameObject.AddComponent<RRConnectionManager>();
	
			NetworkRequestTable.init();
			NetworkResponseTable.init();
		}
		
		// Use this for initialization
		void Start () {
			//Application.LoadLevel("RRLogin");
			RRConnectionManager cManager = gameObject.GetComponent<RRConnectionManager>();
			if (cManager) {
				StartCoroutine(RequestHeartbeat(1f));
			}
		}
		
		// Update is called once per frame
		void Update () {
		}
	
		public IEnumerator RequestHeartbeat(float time) {
			yield return new WaitForSeconds(time);
			
			RRConnectionManager cManager = gameObject.GetComponent<RRConnectionManager>();
			
			if (cManager) {
				RequestHeartbeat request = new RequestHeartbeat();
				request.Send();
				
				cManager.Send(request);
			}
			
	        //originially 1f changed to .1f
			StartCoroutine(RequestHeartbeat(1f));
		}
	}
}