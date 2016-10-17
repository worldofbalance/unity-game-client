using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ChatPanel : MonoBehaviour {

	// Use this for initialization
  Button btnSend;
  ChatMessage messagePrefab;
  RectTransform messages;
  RectTransform scrollRect;
  RectTransform rectTransform;
  bool allowEnter;
  InputField txtMessage;
	void Start () {
    btnSend = this.transform.Find("BtnSend").GetComponent<Button>();
    txtMessage = this.transform.Find("TxtInput").GetComponent<InputField>();
    messagePrefab = this.transform.Find("MessagePrefab").GetComponent<ChatMessage>();
    scrollRect = this.transform.Find("ScrollContent").GetComponent<RectTransform>();
    messages = scrollRect.Find("Messages").GetComponent<RectTransform>();
    txtMessage.ActivateInputField();
    btnSend.onClick.AddListener(sendChatMessage);
	}

  void Awake() {
    Game.networkManager.Listen(
        NetworkCode.CHAT,
        onChatMessage
		);
  }

  public void onChatMessage(NetworkResponse response) {
    Debug.Log("Received Chat message");
  }

  void sendChatMessage() {
    Game.networkManager.Send(ChatProtocol.Prepare (GameState.player.GetName(), this.txtMessage.text));
    this.txtMessage.text = "";
  }
	
	// Update is called once per frame
	void Update () {
    // check if the enter key is pressed, if it is and the focused control is the input field, we'll send the chat message.
    if (allowEnter && (txtMessage.text.Length > 0) && (Input.GetKey (KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))) {
      sendChatMessage();
      allowEnter = false;
    } else {
      allowEnter = txtMessage.isFocused;
    }
  }
}
