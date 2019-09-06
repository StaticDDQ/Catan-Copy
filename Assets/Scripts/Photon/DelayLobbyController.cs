using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DelayLobbyController : MonoBehaviourPunCallbacks
{
    public GameObject startBtn;
    public GameObject cancelBtn;
    public int roomSize;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startBtn.SetActive(true);
    }

    public void QuickStart()
    {
        startBtn.SetActive(false);
        cancelBtn.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room");
        int rand = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + rand, roomOps);
        Debug.Log(rand);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room");
        CreateRoom();
    }

    public void QuickCancel()
    {
        cancelBtn.SetActive(false);
        startBtn.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
