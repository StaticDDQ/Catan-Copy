using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameState : MonoBehaviourPunCallbacks
{
    public static GameState instance;

    [SerializeField] private PlayerState playerPrefab = null;
    private Player currPlayer;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            SpawnPlayer();

            currPlayer = PhotonNetwork.PlayerList[0];

            if(PhotonNetwork.IsMasterClient)
                StartCoroutine(LoadGrid());
        }
    }

    private IEnumerator LoadGrid()
    {
        yield return new WaitForSeconds(5);

        photonView.RPC("NextPlayer", RpcTarget.All, -1);
    }

    [PunRPC]
    public void NextPlayer(int diceResult)
    {
        Debug.Log("Start player's turn");
        
        if(diceResult == 7)
        {
            playerPrefab.DropHalve();

            playerPrefab.MoveKnight(currPlayer.ActorNumber);
        }
        else if(diceResult != -1)
        {
            foreach (Transform panel in transform)
            {
                if (panel.GetComponent<ResourceInfo>().GetRandom() == diceResult)
                {
                    panel.GetComponent<ResourceInfo>().DistributeResource();
                }
            }
        }

        playerPrefab.StartTurn(currPlayer.ActorNumber);

        currPlayer = currPlayer.GetNext();
    }

    private void SpawnPlayer()
    {
        Debug.Log("Creating Player " + PhotonNetwork.LocalPlayer.ActorNumber);
        playerPrefab.SetID(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
