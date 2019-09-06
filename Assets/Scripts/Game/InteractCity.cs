using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractCity : Interactable
{
    private bool hasSettlement = false;
    private bool hasUpgraded = false;

    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Transform settlement = null;
    [SerializeField] private Transform city = null;

    private Transform currObj;

    [PunRPC]
    public override void Interact(PlayerState player)
    {
        if(!hasSettlement && player.PlaceSettlement())
        {
            hasSettlement = true;
            currObj = Instantiate(settlement, transform);
            currObj.localPosition = Vector3.zero + offset;
            currObj.localRotation = Quaternion.identity;
            currObj.GetComponent<ObjectHolder>().SetOwner(player);
        }
        else if(hasSettlement && !hasUpgraded && player.UpgradeSettlement())
        {
            hasUpgraded = true;
            Destroy(currObj.gameObject);
            currObj = Instantiate(city, transform);
            currObj.localPosition = Vector3.zero + offset;
            currObj.localRotation = Quaternion.identity;
            currObj.GetComponent<ObjectHolder>().SetOwner(player);
        }
    }

    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {

    }
}
