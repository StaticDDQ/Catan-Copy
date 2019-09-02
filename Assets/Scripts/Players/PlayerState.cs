using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private int roadsRemaining = 10;
    private int settlementsRemaining = 5;

    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep
    public int[] resources;
    // game score
    private int score;

    private List<DevelopmentCards> devCards;
    private bool obtainedDevCards = false;

    private void Start()
    {
        score = 0;
        //resources = new int[] {0,0,0,0,0};
        devCards = new List<DevelopmentCards>();
    }

    public void StartTurn()
    {
        if (obtainedDevCards)
        {
            foreach(DevelopmentCards card in devCards)
            {
                card.SetUse(true);
            }
            obtainedDevCards = false;
        }
    }

    public bool UpgradeSettlement()
    {
        if(resources[3] >= 3 && resources[2] >= 2)
        {
            settlementsRemaining++;
            score++;

            resources[3] -= 3;
            resources[2] -= 2;

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
        if(roadsRemaining > 0 && resources[0] > 0 && resources[1] > 0)
        {
            roadsRemaining--;
            resources[0]--;
            resources[1]--;

            return true;
        }

        return false;
    }

    public bool PlaceSettlement()
    {
        if (settlementsRemaining > 0 && resources[0] > 0 && resources[1] > 0 &&
            resources[2] > 0 && resources[4] > 0)
        {
            settlementsRemaining--;

            resources[0]--;
            resources[1]--;
            resources[2]--;
            resources[4]--;

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
    }

    public void AddDevCard(DevelopmentCards devCard)
    {
        devCards.Add(devCard);
        obtainedDevCards = true;
    }

    public void DropHalve()
    {

    }
}
