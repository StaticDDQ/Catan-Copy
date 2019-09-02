using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRoad : Interactable
{
    private bool hasRoad = false;

    public override void Interact(PlayerState player)
    {
        if (!hasRoad && player.PlaceRoad())
        {
            hasRoad = true;
            GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
        }
        else
        {

        }
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseExit()
    {
        
    }
}
