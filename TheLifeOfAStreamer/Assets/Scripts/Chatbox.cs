using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour
{
    private string username = Globals.username;
    public int maxMessages = 100;
    
    [SerializeField]
    private List<Message> messageList = new List<Message>();
    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color message, info, streamerMessage, trollMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chatBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SendChatMessage(username + ": " + chatBox.text, Message.MessageType.streamerMessage);
                chatBox.text = "";
            }
        }

        if (!chatBox.isFocused) {
            if (Input.GetKeyDown("]")) {
                SendChatMessage("System: Testing", Message.MessageType.info);
            }
        }
    }

    public void SendChatMessage(string text, Message.MessageType messageType, Color color) {
        if (messageList.Count >= maxMessages) {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = color;
        newMessage.messageType = messageType;

        messageList.Add(newMessage);
    }

    public void SendChatMessage(string text, Message.MessageType messageType) {
        SendChatMessage(text, Message.MessageType.message, MessageTypeColor(messageType));
    }

    public void SendChatMessage(string text, Color color) {
        SendChatMessage(text, Message.MessageType.message, color);
    }

    public void SendChatMessage(string text) {
        SendChatMessage(text, Message.MessageType.message, message);
    }

    public Message getMessageAt(int index) {
        return messageList[index];
    }

    public Message getLastMessage() {
        return getMessageAt(messageList.Count - 1);
    }

    public Message getLastPlayerMessage() {
        for (int i = messageList.Count - 1; i >= 0; i--) {
            if (getMessageAt(i).messageType == Message.MessageType.streamerMessage) return getMessageAt(i);
        }
        return new Message();
    }

    public Message getLastTrollMessage() {
        for (int i = messageList.Count - 1; i >= 0; i--) {
            if (getMessageAt(i).messageType == Message.MessageType.trollMessage) return getMessageAt(i);
        }
        return new Message();
    }

    Color MessageTypeColor (Message.MessageType messageType) {
        Color color;

        switch (messageType) {
            case Message.MessageType.info:
                color = info;
                break;
            case Message.MessageType.streamerMessage:
                color = streamerMessage;
                break;
            case Message.MessageType.trollMessage:
                float intensity = Mathf.Max(-1 * Mathf.Min(Globals.attitude, 0), 0) / 50f;
                color = Color.Lerp(message, trollMessage, intensity);
                // TO DO: FX on Intensity >= 30
                break;
            default:
            case Message.MessageType.message:
                color = message;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message {
    public string text = "";
    public Text textObject = null;
    public MessageType messageType = MessageType.None;

    public enum MessageType {
        None,
        message,            //AI messages
        info,
        streamerMessage,    // Streamer (the real player)'s messages
        trollMessage        // Troll messages
    }
}