using UnityEngine;
using System.Collections;

namespace RR {
	public class ButtonLogic : MonoBehaviour {
	
		public void Ready(){
			//for now just go to the game scene
			Application.LoadLevel ("RRSelectionScene");
		}
	
	}
}