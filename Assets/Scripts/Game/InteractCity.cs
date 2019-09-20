using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class InteractCity : Interactable
{
    private bool hasSettlement = false;
    private bool hasUpgraded = false;
    private int reservedID = -1;

    [SerializeField] private Vector3 offset = Vector3.zero;

    public bool canPlace = false;
    public bool occupiedPlace = false;
    private List<int> allowedIDs = new List<int>();

    private GameObject currObj;

    [PunRPC]
    public override void Interact(int player, bool planning)
    {
        if (!canPlace && !planning || occupiedPlace)
            return;

        if(reservedID == -1 && !hasSettlement && PhotonNetwork.LocalPlayer.ActorNumber == player && PlayerState.instance.PlaceSettlement())
        {
            currObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab","settlement"), this.transform.position + offset, Quaternion.Euler(-90f,0,0));
            currObj.GetComponent<ObjectHolder>().photonView.RPC("SetOwner", RpcTarget.All, player);

            photonView.RPC("ReservePanel", RpcTarget.All, player);
        }
        else if(hasSettlement && !hasUpgraded && PhotonNetwork.LocalPlayer.ActorNumber == player && reservedID == player && PlayerState.instance.UpgradeSettlement())
        {
            hasUpgraded = true;
            PhotonNetwork.Destroy(currObj);
            currObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "city"), this.transform.position + offset, Quaternion.Euler(-90f,0,0));
            currObj.GetComponent<ObjectHolder>().photonView.RPC("SetOwner", RpcTarget.All, player);
        }
    }

    [PunRPC]
    private void ReservePanel(int player)
    {
        hasSettlement = true;
        reservedID = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanPlace"))
        {
            if(other.transform.parent.GetComponent<ObjectHolder>().GetOwner() != null)
                canPlace = true;
        }
    }

    [PunRPC]
    public void DisallowPlacement(bool disallow)
    {
        occupiedPlace = disallow;
    } 

    private void OnMouseOver()
    {
        
    }

    private void OnMouseExit()
    {

    }
}
