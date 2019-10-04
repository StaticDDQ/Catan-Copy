using UnityEngine;
using Photon.Pun;

public class DevelopmentCardGenerator : MonoBehaviourPunCallbacks
{
    public static DevelopmentCardGenerator instance;

    private int knightCount = 20;
    private int victoryPointCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public DevelopmentCards GetDevelopment()
    {
        int index = Random.Range(0, knightCount + victoryPointCount);
        if(index <= knightCount)
        {
            photonView.RPC("ReduceKnight", RpcTarget.All);
            return new KnightCard();
        } else
        {
            photonView.RPC("ReduceVictoryPoint", RpcTarget.All);
            return new RewardCard();
        }
    }

    [PunRPC]
    private void ReduceKnight()
    {
        knightCount--;
    }

    [PunRPC]
    private void ReduceVictoryPoint()
    {
        victoryPointCount--;
    }

    public bool HasCards()
    {
        return knightCount + victoryPointCount > 0;
    }
}
