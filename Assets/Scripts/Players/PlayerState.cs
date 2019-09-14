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
    
    [SerializeField] private Color playerColor;

    public static PlayerState instance;

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

    public void SetColor(int id, Color newColor)
    {
        if(id == PhotonNetwork.LocalPlayer.ActorNumber)
            playerColor = newColor;
    }

    public Color GetColor()
    {
        return playerColor;
    }

    public void StartTurn(int ingoingID, bool setupPhase)
    {
        if(ingoingID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("Start turn");
            turnsButtons.SetActive(true);

            if (obtainedDevCards)
            {
                foreach (DevelopmentCards card in devCards)
                {
                    card.SetUse(true);
                }
                obtainedDevCards = false;
            }

            hasUsedDevCard = false;

            if (setupPhase)
            {
                setupSettlement = 1;
                setupRoad = 2;  
            }

            GetComponent<Selector>().SetSetupPhase(setupPhase);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPlacingRoad = false;
            isPlacingSettlement = false;
            isPlacingCity = false;
        }
    }

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
                Debug.Log("Win");
            }

            return true;
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

            return true;
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
                Debug.Log("Win");
            }

            return true;
        }

        return false;
    }

    public void GetResource(int resourceID)
    {
        resources[resourceID] += 1;
        resourceAmnt[resourceID].text = resources[resourceID].ToString();
    }

    public void BuyDevCard()
    {
        if(resources[2] > 0 && resources[3] > 0 && resources[4] > 0)
        {
            resources[2]--;
            resources[3]--;
            resources[4]--;
            resourceAmnt[2].text = resources[2].ToString();
            resourceAmnt[3].text = resources[3].ToString();
            resourceAmnt[4].text = resources[4].ToString();
            // generate dev card
        }
    }

    public void AddDevCard(DevelopmentCards devCard)
    {
        devCards.Add(devCard);
        obtainedDevCards = true;
    }

    public void UseDevCard(DevelopmentCards devCard)
    {
        if (!hasUsedDevCard)
        {
            devCard.Effect();
            hasUsedDevCard = true;
        }
    }

    public void DropHalve()
    {
        int sum = 0;
        for (int i = 0; i < resources.Length; i++)
        {
            sum += resources[i];
        }

        if(sum >= 7)
        {

        }
    }

    public void MoveKnight(int id)
    {
        if(id == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            movingKnight = true;
        }
    }

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
        if (setupRoad > 0 || setupSettlement > 0)
            return;

        isPlacingRoad = false;
        isPlacingCity = false;
        isPlacingSettlement = false;

        int result = RollDice.Roll();
        GameState.instance.photonView.RPC("NextPlayer", RpcTarget.All, result);

        turnsButtons.SetActive(false);
    }

    public bool IsMovingKnight()
    {
        return movingKnight;
    }

    public void HasMoveKnight(ResourceInfo prevPanel)
    {
        movingKnight = false;

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
        this.prevPanel.photonView.RPC("SetUnderKnight", RpcTarget.All, false);
    }

    public void SetResources(int[] newResources)
    {
        for (int i = 0; i < 5; i++)
        {
            resources[i] = newResources[i];
            resourceAmnt[i].text = newResources[i].ToString();
        }
    }
}
