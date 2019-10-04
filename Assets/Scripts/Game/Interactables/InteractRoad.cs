using UnityEngine;
using Photon.Pun;
using System.IO;

public class InteractRoad : Interactable
{
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0.05f);
    public bool canPlace = false;
    private Color defaultCol;

    [PunRPC]
    public override void Interact(int player, bool planning)
    {
        if (canPlace && PhotonNetwork.LocalPlayer.ActorNumber == player && PlayerState.instance.PlaceRoad())
        {
            GameObject currObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "road"), this.transform.position + offset, transform.rotation);
            currObject.GetComponent<ObjectHolder>().photonView.RPC("SetOwner", RpcTarget.All, player);
            photonView.RPC("ChangeTag", RpcTarget.All);
        }
    }

    [PunRPC]
    private void ChangeTag()
    {
        gameObject.tag = "Untagged";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanPlace") && other.transform.parent.GetComponent<ObjectHolder>().GetID() == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            canPlace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanPlace") && other.transform.parent.GetComponent<ObjectHolder>().GetID() == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            canPlace = false;
        }
    }

    private void OnMouseOver()
    {
        defaultCol = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = defaultCol;
    }
}
