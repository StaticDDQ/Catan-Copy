using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TradeController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject tradePanel = null;
    [SerializeField] private GameObject replyPrompt = null;

    [SerializeField] private Text replyPromptText = null;
    [SerializeField] private Text giveAmntText = null;
    [SerializeField] private Text tradeAmntText = null;
    public static TradeController instance;
    private bool isSelected = false;
    private int[] resourceChange;
    private int senderID;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    [PunRPC]
    public void ReceiveTradeRequest(int[] ids, int giveIndex, int giveAmnt, int tradeIndex, int tradeAmnt, int sender)
    {
        isSelected = false;

        for (int i = 0; i < ids.Length; i++)
        {
            if(ids[i] == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                isSelected = true;
                break;
            }
        }

        if (isSelected)
        {

            tradePanel.SetActive(true);
            giveAmntText.text = giveAmnt.ToString();
            tradeAmntText.text = tradeAmnt.ToString();

            resourceChange = new int[] { 0, 0, 0, 0, 0 };
            resourceChange[giveIndex] = -giveAmnt;
            resourceChange[tradeIndex] = tradeAmnt;

            senderID = sender;
        }
    }

    public void Decline()
    {
        tradePanel.SetActive(false);
        photonView.RPC("TradeReply", RpcTarget.All, senderID, false);
    }

    public void Accept()
    {
        PlayerState.instance.ExchangeResource(resourceChange);
        tradePanel.SetActive(false);

        photonView.RPC("TradeReply", RpcTarget.All, senderID, true);
    }

    [PunRPC]
    private void TradeReply(int index, bool isAccepted)
    {
        // sender received the reverse deal
        if (index == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (isAccepted)
            {
                for (int i = 0; i < 5; i++)
                {
                    resourceChange[i] = -resourceChange[i];
                }

                PlayerState.instance.ExchangeResource(resourceChange);

                replyPrompt.SetActive(true);
                replyPromptText.text = "Trade accepted";
            }
        }
        else
        {
            // someone else has accepted the trade earlier
            if (isAccepted && isSelected)
            {
                replyPrompt.SetActive(true);
                replyPromptText.text = "Trade accepted by someone else";
            }
        }
    }

    public void CloseReplyPrompt()
    {
        replyPrompt.SetActive(false);
    }
}
