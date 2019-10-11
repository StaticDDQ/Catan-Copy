using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RewardManager : MonoBehaviourPunCallbacks
{
    public static RewardManager instance;
    private int highestKnights = 3;
    private int highestRoads = 6;

    [SerializeField] private int currKnights = 0;
    [SerializeField] private int currRoads = 0;

    private int rewardHolder = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void IncreaseKnights()
    {
        currKnights++;
        if(currKnights > highestKnights)
        {
            photonView.RPC("SetHighestKnights", RpcTarget.All, currKnights, PhotonNetwork.LocalPlayer.ActorNumber);
            PlayerState.instance.IncreasePoint();
            PlayerState.instance.IncreasePoint();
            MessageManager.instance.photonView.RPC("SendMessageText", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + ": has gotten largest army");
        }
    }

    public void IncreaseRoads()
    {
        currRoads++;
        if(currRoads > highestRoads)
        {
            photonView.RPC("SetHighestRoads", RpcTarget.All, currRoads, PhotonNetwork.LocalPlayer.ActorNumber);
            PlayerState.instance.IncreasePoint();
            PlayerState.instance.IncreasePoint();
            MessageManager.instance.photonView.RPC("SendMessageText", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + ": has gotten longest road");
        }
    }

    [PunRPC]
    private void SetHighestKnights(int knights, int newOwner)
    {
        highestKnights = knights;
        PlayerState.instance.DecreasePoint(rewardHolder);
        PlayerState.instance.DecreasePoint(rewardHolder);
        rewardHolder = newOwner;
    }

    [PunRPC]
    private void SetHighestRoads(int roads, int newOwner)
    {
        highestRoads = roads;
        PlayerState.instance.DecreasePoint(rewardHolder);
        PlayerState.instance.DecreasePoint(rewardHolder);
        rewardHolder = newOwner;
    }
}
