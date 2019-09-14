using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviourPunCallbacks
{
    public static GameState instance;

    private List<Color> playerColors = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow };

    [SerializeField] private PlayerState playerPrefab = null;
    [SerializeField] private SetText diceText = null;
    private int index = 0;
    private Player currPlayer;

    private bool setupPhase = true;
    private bool reverseOrder = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            Debug.Log("Creating Player " + PhotonNetwork.LocalPlayer.ActorNumber);

            currPlayer = PhotonNetwork.PlayerList[index];

            if(PhotonNetwork.IsMasterClient)
                StartCoroutine(LoadGrid());
        }
    }

    private IEnumerator LoadGrid()
    {
        yield return new WaitForSeconds(5);

        photonView.RPC("NextPlayer", RpcTarget.All, -1);
    }

    private Color GetColor()
    {
        Color getColor = playerColors[0];
        playerColors.Remove(getColor);

        return getColor;
    }

    private IEnumerator SetupPhase()
    {
        yield return new WaitForSeconds(1);

        if (!reverseOrder)
        {
            Color newColor = GetColor();
            playerPrefab.SetColor(currPlayer.ActorNumber, newColor);
        }
        
        playerPrefab.StartTurn(currPlayer.ActorNumber, true);

        index = (reverseOrder) ? index - 1 : index + 1;
 
        if (index == PhotonNetwork.PlayerList.Length && !reverseOrder)
        {
            index -= 1;
            reverseOrder = true;
        } else if(reverseOrder && index < 0)
        {
            index = 0;
            setupPhase = false;
        }

        currPlayer = PhotonNetwork.PlayerList[index];
    }

    private IEnumerator RegularPhase(int diceRoll)
    {
        yield return new WaitForSeconds(1);
        
        diceText.photonView.RPC("SetGivenText", RpcTarget.All, diceRoll.ToString());

        if (diceRoll == 7)
        {
            playerPrefab.DropHalve();

            playerPrefab.MoveKnight(currPlayer.ActorNumber);
        }
        else
        {
            foreach (Transform panel in transform)
            {
                if (panel.GetComponent<ResourceInfo>().GetRandom() == diceRoll)
                {
                    panel.GetComponent<ResourceInfo>().DistributeResource();
                }
            }
        }

        playerPrefab.StartTurn(currPlayer.ActorNumber, false);

        currPlayer = currPlayer.GetNext();
    }

    [PunRPC]
    public void NextPlayer(int diceResult)
    {
        if (setupPhase)
        {
            StartCoroutine(SetupPhase());
        } else
        {
            StartCoroutine(RegularPhase(diceResult));
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
