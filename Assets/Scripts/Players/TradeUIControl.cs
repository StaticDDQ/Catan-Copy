using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TradeUIControl : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPanel = null;
    [SerializeField] private Transform playerPanelParent = null;

    private List<PlayerPanel> selectedPlayers = null;

    [SerializeField] private Text tradeText = null;
    [SerializeField] private Text gainText = null;

    [SerializeField] private GameObject tradeResourcesPanel = null;
    [SerializeField] private GameObject giveResourcesPanel = null;

    private int tradeResourceIndex;
    private int tradeAmnt = 0;

    private int gainResourceIndex;
    private int gainAmnt = 0;

    public static TradeUIControl instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;

            selectedPlayers = new List<PlayerPanel>();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.IsLocal)
                {
                    var playerUI = Instantiate(playerPanel, playerPanelParent);
                    playerUI.GetComponent<PlayerPanel>().SetPlayerID(player.ActorNumber);
                    playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SelectPlayer(playerUI.GetComponent<PlayerPanel>()); });
                }
            }
        }
    }

    public void SelectPlayer(PlayerPanel player)
    {
        if (selectedPlayers.Contains(player))
        {
            selectedPlayers.Remove(player);
        }
        else
        {
            selectedPlayers.Add(player);
        }
    }

    public void SelectTrade(int index)
    {
        tradeResourceIndex = index;
        tradeAmnt = 0;

        tradeResourcesPanel.SetActive(false);
    }

    public void SelectGain(int index)
    {
        gainResourceIndex = index;
        gainAmnt = 0;

        giveResourcesPanel.SetActive(false);
    }

    public void TradeMore()
    {
        tradeAmnt++;
        tradeText.text = tradeAmnt.ToString();
    }

    public void TradeLess()
    {
        if (tradeAmnt > 0)
        {
            tradeAmnt--;
            tradeText.text = tradeAmnt.ToString();
        }
    }

    public void GainMore()
    {
        gainAmnt++;
        gainText.text = gainAmnt.ToString();
    }

    public void GainLess()
    {
        if (gainAmnt > 0)
        {
            gainAmnt--;
            gainText.text = gainAmnt.ToString();
        }
    }

    public void SelectTradeResource()
    {
        tradeResourcesPanel.SetActive(true);
    }

    public void SelectGiveResource()
    {
        giveResourcesPanel.SetActive(true);
    }

    public void BeginTrade()
    {
        int[] sentIDs = new int[selectedPlayers.Count];

        for (int i = 0; i < selectedPlayers.Count; i++)
        {
            sentIDs[i] = selectedPlayers[i].GetPlayerID();
        }

        TradeController.instance.photonView.RPC("ReceiveTradeRequest", RpcTarget.All, sentIDs, gainResourceIndex, gainAmnt, tradeResourceIndex, tradeAmnt, PhotonNetwork.LocalPlayer.ActorNumber);
    }
}
