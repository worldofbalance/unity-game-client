using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour {

	// Use this for initialization
  Button btnSend;
  InputField txtMessage;
	void Start () {
    btnSend = this.transform.Find("BtnSend").GetComponent<Button>();
    txtMessage = this.transform.Find("TxtInput").GetComponent<InputField>();
    btnSend.onClick.AddListener(sendChatMessage);
	}

  void sendChatMessage() {
    Debug.Log(this.txtMessage.text);
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
