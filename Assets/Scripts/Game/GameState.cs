using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private List<PlayerState> players;

    private void Start()
    {
        players = new List<PlayerState>();
    }

    public void NextPlayer()
    {
        int result = RollDice.Roll();

        if(result == 7)
        {
            foreach(PlayerState player in players)
            {
                player.DropHalve();
            }
        }
        else
        {
            foreach (Transform panel in transform)
            {
                if (panel.GetComponent<ResourceInfo>().GetRandom() == result)
                {

                }
            }
        }
    }

    public void JoinedPlayer(PlayerState newPlayer)
    {
        players.Add(newPlayer);
    }
}
