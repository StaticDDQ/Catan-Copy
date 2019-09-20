using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    private int playerID;

    public void SetPlayerID(int id)
    {
        playerID = id;
    }

    public int GetPlayerID()
    {
        return playerID;
    }
}
