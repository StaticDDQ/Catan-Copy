using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerState : MonoBehaviour
{
    // number of blocks you can place in the game
    private int roadsRemaining = 15;
    private int settlementsRemaining = 5;
    private int citiesRemaining = 4;

    // visual amount of remaining blocks
    public Text roadsAmnt;
    public Text settlementsAmnt;
    public Text citiesAmnt;

    public GameObject turnsButtons;

    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep
    public int[] resources;
    public Text[] resourceAmnt;

    // can place a block or not
    private bool isPlacingRoad = false;
    private bool isPlacingSettlement = false;
    private bool isPlacingCity = false;

    // game score
    private int score;

    // dev cards storage
    private List<DevelopmentCards> devCards;
    private bool obtainedDevCards = false;
    // can only use 1 dev card per turn
    private bool hasUsedDevCard = false;

    private int setupRoad = 2;
    private int setupSettlement = 1;

    private bool movingKnight = false;
    private bool hasPlacedKnight = true;
    private ResourceInfo prevPanel;
    
    [SerializeField] private Color playerColor = Color.white;
    [SerializeField] private GameObject notification = null;
    [SerializeField] private Text notifText = null;

    public static PlayerState instance;

    // require only 1 instance of player each local computer
    private void Start()
    {
        if(instance == null)
        {
            instance = this;

            score = 0;
            resources = new int[] {0,0,0,0,0};
            devCards = new List<DevelopmentCards>();

            roadsAmnt.text = roadsRemaining.ToString();
            settlementsAmnt.text = settlementsRemaining.ToString();
            citiesAmnt.text = citiesRemaining.ToString();
        }
    }

    // assign color to local player
    public void SetColor(int id, Color newColor)
    {
        if(id == PhotonNetwork.LocalPlayer.ActorNumber)
            playerColor = newColor;
    }

    public Color GetColor()
    {
        return playerColor;
    }

    // check if id matches with local player and start turn
    public void StartTurn(int ingoingID, bool setupPhase)
    {
        if(ingoingID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("Start turn");
            turnsButtons.SetActive(true);

            // dev cards can now be used
            if (obtainedDevCards)
            {
                foreach (DevelopmentCards card in devCards)
                {
                    card.SetUse(true);
                }
                obtainedDevCards = false;
            }

            // limit to only 1 dev card use per turn
            hasUsedDevCard = false;

            // setup phase
            if (setupPhase)
            {
                setupSettlement = 1;
                setupRoad = 1;  
            }

            GetComponent<Selector>().SetSetupPhase(setupPhase);
        }
    }

    // cancel action when pressing escape
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPlacingRoad = false;
            isPlacingSettlement = false;
            isPlacingCity = false;
        }
    }

    #region Block Placing
    public bool UpgradeSettlement()
    {
        if(isPlacingCity && citiesRemaining > 0 && resources[3] >= 3 && resources[2] >= 2)
        {
            settlementsRemaining++;
            citiesRemaining--;

            score++;

            resources[3] -= 3;
            resources[2] -= 2;

            settlementsAmnt.text = settlementsRemaining.ToString();
            citiesAmnt.text = citiesRemaining.ToString();
            
            resourceAmnt[2].text = resources[2].ToString();
            resourceAmnt[3].text = resources[3].ToString();

            if (score == 10)
            {
                GameState.instance.photonView.RPC("WinGame", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
            }
            isPlacingCity = false;

            return true;
        } else
        {
            notification.SetActive(true);
            notifText.text = "Not enough resources!";
        }

        return false;
    }

    public bool PlaceRoad()
    {
        if(isPlacingRoad && roadsRemaining > 0 && ((resources[0] > 0 && resources[1] > 0) || setupRoad > 0))
        {
            roadsRemaining--;

            roadsAmnt.text = roadsRemaining.ToString();

            if (setupRoad > 0)
            {
                setupRoad--;
            } else
            {
                resources[0]--;
                resources[1]--;
                resourceAmnt[0].text = resources[0].ToString();
                resourceAmnt[1].text = resources[1].ToString();
            }
            isPlacingRoad = false;

            RewardManager.instance.IncreaseRoads();
            return true;
        } else
        {
            notification.SetActive(true);
            notifText.text = "Not enough resources!";
        }

        return false;
    }

    public bool PlaceSettlement()
    {
        if (isPlacingSettlement && settlementsRemaining > 0 && ((resources[0] > 0 && resources[1] > 0 &&
            resources[2] > 0 && resources[4] > 0) || setupSettlement > 0))
        {
            settlementsRemaining--;

            settlementsAmnt.text = settlementsRemaining.ToString();

            if (setupSettlement > 0)
            {

                setupSettlement--;
                
            } else
            {
                resources[0]--;
                resources[1]--;
                resources[2]--;
                resources[4]--;
                resourceAmnt[0].text = resources[0].ToString();
                resourceAmnt[1].text = resources[1].ToString();
                resourceAmnt[2].text = resources[2].ToString();
                resourceAmnt[4].text = resources[4].ToString();
            }

            score++;
            if(score == 10)
            {
                GameState.instance.photonView.RPC("WinGame", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
            }
            isPlacingSettlement = false;

            return true;
        }
        else
        {
            notification.SetActive(true);
            notifText.text = "Not enough resources!";
        }

        return false;
    }

    #endregion

    // receive 1 resource at a time
    public void GetResource(int resourceID)
    {
        resources[resourceID] += 1;
        resourceAmnt[resourceID].text = resources[resourceID].ToString();
    }

    #region Dev Cards
    public void BuyDevCard()
    {
        if(resources[2] > 0 && resources[3] > 0 && resources[4] > 0 && DevelopmentCardGenerator.instance.HasCards())
        {
            resources[2]--;
            resources[3]--;
            resources[4]--;
            resourceAmnt[2].text = resources[2].ToString();
            resourceAmnt[3].text = resources[3].ToString();
            resourceAmnt[4].text = resources[4].ToString();
            // generate dev card

            AddDevCard(DevelopmentCardGenerator.instance.GetDevelopment());
        }
        else if (!DevelopmentCardGenerator.instance.HasCards())
        {
            notification.SetActive(true);
            notifText.text = "Out of development cards!";
        }
        else
        {
            notification.SetActive(true);
            notifText.text = "Not enough resources!";
        }
    }

    private void AddDevCard(DevelopmentCards devCard)
    {
        devCards.Add(devCard);
        obtainedDevCards = true;

        transform.GetChild(0).GetComponent<UIControl>().AddDevCard(devCard);

        if(devCard.GetType() == typeof(RewardCard))
        {
            devCard.Effect();
        }
    }

    public void UseDevCard(DevelopmentCards devCard)
    {
        if (!hasUsedDevCard && devCard.GetType() != typeof(RewardCard))
        {
            devCard.Effect();
            hasUsedDevCard = true;
        }
    }

    #endregion

    #region Button Functions

    public void CreateRoad()
    {
        isPlacingRoad = true;
        isPlacingCity = false;
        isPlacingSettlement = false;
    }

    public void CreateSettlement()
    {
        isPlacingRoad = false;
        isPlacingCity = false;
        isPlacingSettlement = true;
    }

    public void CreateCity()
    {
        isPlacingRoad = false;
        isPlacingCity = true;
        isPlacingSettlement = false;
    }

    public void EndTurn()
    {
        // dont end turn if there is still something to place in setup phase
        if (setupRoad > 0 || setupSettlement > 0)
            return;

        isPlacingRoad = false;
        isPlacingCity = false;
        isPlacingSettlement = false;

        int d1 = RollDice.Roll();
        int d2 = RollDice.Roll();
        GameState.instance.photonView.RPC("NextPlayer", RpcTarget.All, d1, d2);

        turnsButtons.SetActive(false);
    }

    public void CloseNotification()
    {
        notification.SetActive(false);
    }

    #endregion

    #region Knight

    // someone rolled a 7, move the knight
    public void MoveKnight(int id)
    {
        if (id == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            movingKnight = true;
        }
    }

    // check if player can move the knight
    public bool IsMovingKnight()
    {
        return movingKnight;
    }

    // began moving knight
    public void HasMoveKnight(ResourceInfo prevPanel)
    {
        movingKnight = false;
        // is now moving the knight
        hasPlacedKnight = false;

        this.prevPanel = prevPanel;
    }

    public bool HasPlacedKnight()
    {
        return hasPlacedKnight;
    }

    public void SetPlacedKnight()
    {
        hasPlacedKnight = true;
        prevPanel.photonView.RPC("SetUnderKnight", RpcTarget.All, false);
    }

    public void DropHalve()
    {
        int sum = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            sum += resources[i];
        }

        if (sum >= 7)
        {
            // open UI to drop x resources
            transform.GetChild(0).GetComponent<UIControl>().SetDropPage(resources);
        }
    }

    #endregion

    public void ExchangeResource(int[] gainedAmnt)
    {
        for(int i = 0; i < 5; i++)
        {
            resources[i] += gainedAmnt[i];
            resourceAmnt[i].text = resources[i].ToString();
        }
    }

    // get amount of a certain resource index
    public int GetResourceAmount(int index)
    {
        return resources[index];
    }

    public void IncreasePoint()
    {
        score++;

        if(score == 10)
        {
            GameState.instance.photonView.RPC("WinGame", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
        }
    }

    public void DecreasePoint(int id)
    {
        if(id == PhotonNetwork.LocalPlayer.ActorNumber)
            score--;
    }
}
