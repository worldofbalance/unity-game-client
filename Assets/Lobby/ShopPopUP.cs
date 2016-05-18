using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopPopUP : MonoBehaviour {
	public Text animalQty;
	public Text CreditsRemaining;
	public GameObject ShopPopUp;

	// Use this for initialization
	void Start () {
		
	}

	public void UpdateTextFields( ){
		Debug.Log ("UPDATING TEXT FIELD");
		animalQty.text = "25";
		CreditsRemaining.text = "5000";
	}
	// Update is called once per frame
	void Update () {
	
	}
}
