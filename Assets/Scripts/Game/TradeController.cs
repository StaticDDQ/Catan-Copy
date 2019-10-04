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

    [SerializeField] private Image tradeIcon = null;
    [SerializeField] private Image giveIcon = null;

    public static TradeController instance;
    private bool isSelected = false;
    private int[] resourceChange;
    private int senderID;

    private int giveIndex;
    private int giveAmnt;

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

        if (isSelected && PlayerState.instance.GetResourceAmount(giveIndex) >= giveAmnt)
        {
            Debug.Log("Give: " + giveAmnt + " + Trade: " + tradeAmnt);
            tradePanel.SetActive(true);
            giveAmntText.text = giveAmnt.ToString();
            tradeAmntText.text = tradeAmnt.ToString();

            this.giveIndex = giveIndex;
            this.giveAmnt = giveAmnt;

            resourceChange = new int[] { 0, 0, 0, 0, 0 };
            resourceChange[giveIndex] = -giveAmnt;
            resourceChange[tradeIndex] = tradeAmnt;

            senderID = sender;

            tradeIcon.sprite = SpriteIndex.instance.GetSprite(tradeIndex);
            giveIcon.sprite = SpriteIndex.instance.GetSprite(giveIndex);

            Timer.instance.StartTimer(5f);
        }
    }

    public void Decline()
    {
        tradePanel.SetActive(false);
        photonView.RPC("TradeReply", RpcTarget.Others, senderID, false, resourceChange);
    }

    public void Accept()
    {
        PlayerState.instance.ExchangeResource(resourceChange);
        tradePanel.SetActive(false);

        photonView.RPC("TradeReply", RpcTarget.Others, senderID, true, resourceChange);
    }

    [PunRPC]
    private void TradeReply(int index, bool isAccepted, int[] newResources)
    {
        // sender received the reverse deal
        if (index == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (isAccepted)
            {
                for (int i = 0; i < 5; i++)
                {
                    newResources[i] = -newResources[i];
                }

                PlayerState.instance.ExchangeResource(newResources);

                replyPrompt.SetActive(true);
                replyPromptText.text = "Trade accepted";
            }
        }
        else
        {
            // someone else has accepted the trade earlier
            if (isAccepted && isSelected)
            {
                tradePanel.SetActive(false);

                replyPrompt.SetActive(true);
                replyPromptText.text = "Trade accepted by someone else";
            }
        }
    }

    public void CloseReplyPrompt()
    {
        replyPrompt.SetActive(false);
    }

    public void TimesUp()
    {
        tradePanel.SetActive(false);
        replyPrompt.SetActive(true);
        replyPromptText.text = "Times up";
    }
}
