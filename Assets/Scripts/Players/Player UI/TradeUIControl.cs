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

    [SerializeField] private Image tradeIcon = null;
    [SerializeField] private Image giveIcon = null;

    private int tradeResourceIndex = 0;
    private int tradeAmnt = 0;

    private int gainResourceIndex = 0;
    private int gainAmnt = 0;

    public static TradeUIControl instance;

    private bool canTrade = true;
    private float timeLeft;

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
                    playerUI.GetComponent<PlayerPanel>().SetPlayerID(player.ActorNumber, player.NickName);
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
        tradeText.text = tradeAmnt.ToString();

        tradeIcon.sprite = SpriteIndex.instance.GetSprite(index);

        tradeResourcesPanel.SetActive(false);
    }

    public void SelectGain(int index)
    {
        gainResourceIndex = index;
        gainAmnt = 0;
        gainText.text = gainAmnt.ToString();

        giveIcon.sprite = SpriteIndex.instance.GetSprite(index);

        giveResourcesPanel.SetActive(false);
    }

    public void TradeMore()
    {
        if(tradeAmnt + 1 <= PlayerState.instance.GetResourceAmount(tradeResourceIndex))
        {
            tradeAmnt++;
            tradeText.text = tradeAmnt.ToString();
        }
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

    private void Update()
    {
        if (!canTrade)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                canTrade = true;
            }
        }
    }

    public void BeginTrade()
    {
        if(canTrade && selectedPlayers.Count > 0 && (gainAmnt > 0 || tradeAmnt > 0))
        {
            int[] sentIDs = new int[selectedPlayers.Count];

            for (int i = 0; i < selectedPlayers.Count; i++)
            {
                sentIDs[i] = selectedPlayers[i].GetPlayerID();
            }

            TradeController.instance.photonView.RPC("ReceiveTradeRequest", RpcTarget.All, sentIDs, gainResourceIndex, gainAmnt, tradeResourceIndex, tradeAmnt, PhotonNetwork.LocalPlayer.ActorNumber);

            canTrade = false;
            timeLeft = 6f;
        }
    }
}
