using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractRoad : Interactable
{
    private bool hasRoad = false;

    [PunRPC]
    public override void Interact(PlayerState player)
    {
        if (!hasRoad && player.PlaceRoad())
        {
            hasRoad = true;
            GetComponent<Renderer>().material.SetColor("_BaseColor", player.GetColor());
        }
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseExit()
    {
        
    }
}
