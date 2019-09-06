using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingController : MonoBehaviourPunCallbacks
{
    private PhotonView pView;

    public int sceneIndex;
    public int menuSceneIndex;

    private int playerCount;
    private int roomSize;

    public int minPlayersToStart;
    public Text timerDisplay;
    public Text roomCountDisplay;

    private bool readyToCount;
    private bool readyToStart;
    private bool startGame;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    public float maxWaitTime;
    public float maxFullGameTime; 

    // Start is called before the first frame update
    void Start()
    {
        pView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;

        roomCountDisplay.text = playerCount + ":" + roomSize;

        if(playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if(playerCount >= minPlayersToStart)
        {
            readyToCount = true;
        }
        else
        {
            readyToCount = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
        {
            pView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if(timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    private void Update()
    {
        WaitingForMorePlayer();
    }

    void WaitingForMorePlayer()
    {
        if(playerCount <= 1)
        {
            ResetTimer();
        }

        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCount)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerDisplay.text = tempTimer;

        if(timerToStartGame <= 0f)
        {
            if (startGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameTime;
    }

    public void StartGame()
    {
        startGame = true;

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(sceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
