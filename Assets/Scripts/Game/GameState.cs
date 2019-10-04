using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameState : MonoBehaviourPunCallbacks
{
    public static GameState instance;

    private List<Color> playerColors = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow };

    [SerializeField] private SetDice dice = null;
    [SerializeField] private GameObject winEffect = null;
    private int index = 0;
    private Player currPlayer;

    private bool setupPhase = true;
    private bool reverseOrder = false;
    private bool gameFinished = false;
    [SerializeField] private GameObject winPrompt = null;

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

        photonView.RPC("NextPlayer", RpcTarget.All, -1, -1);
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
            PlayerState.instance.SetColor(currPlayer.ActorNumber, newColor);
        }
        
        PlayerState.instance.StartTurn(currPlayer.ActorNumber, true);

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

    private IEnumerator RegularPhase(int d1, int d2)
    {
        yield return new WaitForSeconds(1);
        
        dice.photonView.RPC("DisplayDice", RpcTarget.All, d1, d2);

        if (d1+d2 == 7)
        {
            PlayerState.instance.DropHalve();

            PlayerState.instance.MoveKnight(currPlayer.ActorNumber);
        }
        else
        {
            foreach (Transform panel in transform)
            {
                if (panel.GetComponent<ResourceInfo>().GetRandom() == d1+d2)
                {
                    panel.GetComponent<ResourceInfo>().DistributeResource();
                }
            }
        }

        PlayerState.instance.StartTurn(currPlayer.ActorNumber, false);

        currPlayer = currPlayer.GetNext();
    }

    [PunRPC]
    public void NextPlayer(int dice1, int dice2)
    {
        if (setupPhase)
        {
            StartCoroutine(SetupPhase());
        } else
        {
            if (!gameFinished)
            {
                StartCoroutine(RegularPhase(dice1, dice2));
            }
        }
    }

    [PunRPC]
    public void WinGame(string playerName)
    {
        var effect = Instantiate(winEffect, transform);
        effect.transform.localPosition = new Vector3(3.5f, 4f, 0);
        winPrompt.SetActive(true);
        winPrompt.transform.GetChild(0).GetComponent<Text>().text = playerName + " won!";
        gameFinished = true;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
