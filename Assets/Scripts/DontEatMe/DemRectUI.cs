using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DemRectUI : MonoBehaviour {

	GameObject canvasObject;

	void Awake(){
		canvasObject = GameObject.Find ("Canvas");

	}

	public GameObject createRectUI(string name, float xPos, float yPos, float width, float height){
		GameObject RectUI = new GameObject ();
		RectUI.name = name;
		RectUI.AddComponent<RectTransform> ();
		RectUI.transform.SetParent(canvasObject.transform);
		RectUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
		RectUI.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, height);
		RectUI.AddComponent<Image> ();
		RectUI.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("white");
		return RectUI;
	}

	public void setPosition(GameObject UI, float xPos, float yPos){
		UI.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
	}

	public void setSize(GameObject UI, float width, float height){
		UI.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width, height);
	}

	public void setUIText(GameObject UI, string text){
		GameObject UIText = new GameObject("UITxt");
		UIText.transform.SetParent(UI.transform);

		// Set the layer to UI layer
		UIText.layer = 5;

		//Set text and its position on the button
		UIText.AddComponent<Text>();
		UIText.GetComponent<Text>().font = Resources.Load<Font>("Fonts/Chalkboard");
		UIText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		UIText.GetComponent<Text>().color = Color.black;
		UIText.GetComponent<Text> ().fontSize = (int)(Screen.width/42);
		UIText.GetComponent<RectTransform>().anchoredPosition = 
			new Vector2 (0.0f, UI.GetComponent<RectTransform> ().sizeDelta.y/5.0f);
		UIText.GetComponent<RectTransform> ().sizeDelta =
			new Vector2 (UI.GetComponent<RectTransform> ().sizeDelta.x/5.0f*3,
						UI.GetComponent<RectTransform> ().sizeDelta.y/5.0f*3);

		UIText.GetComponent<Text>().text = text;
	}
		
}


