using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MessageManager : MonoBehaviourPunCallbacks
{
    public static MessageManager instance;
    public int maxMessages = 25;

    public GameObject chatSystem, chatPanel, textObject;
    public InputField chatBox;

    public bool isTyping = false;

    [SerializeField]
    private List<Message> messageList = new List<Message>();

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                photonView.RPC("SendMessageText", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + ": " + chatBox.text);
                chatBox.text = "";
            }
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
        }

        if (!chatBox.isFocused)
        {
            isTyping = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                chatSystem.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                chatSystem.SetActive(false);
            }
        } else
        {
            isTyping = true;
        }

        
    }

    [PunRPC]
    public void SendMessageText(string text)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
