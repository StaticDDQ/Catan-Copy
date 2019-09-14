using Photon.Pun;
using UnityEngine;

public class ObjectHolder : MonoBehaviourPunCallbacks
{
    private PlayerState owner;

    [PunRPC]
    public void SetOwner(int newOwner)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newOwner)
        {
            owner = PlayerState.instance;
            Debug.Log(owner);
            photonView.RPC("SetColor", RpcTarget.All, owner.GetColor().r, owner.GetColor().g, owner.GetColor().b);
        } else
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    [PunRPC]
    private void SetColor(float r, float g, float b)
    {
        Color setColor = new Color(r, g, b);
        GetComponent<Renderer>().material.SetColor("_BaseColor", setColor);
    }

    public PlayerState GetOwner()
    {
        return owner;
    }
}
