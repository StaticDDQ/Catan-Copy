using UnityEngine;
using Photon.Pun;

public class PortManager : MonoBehaviourPunCallbacks
{
    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep, 5-random
    [SerializeField] private int resourceID = -1;
    [SerializeField] private SpriteRenderer sr = null;
    private bool isAccessible = false;

    public void SetResourceID(int index)
    {
        photonView.RPC("SetupPort", RpcTarget.All, index);
    }

    [PunRPC]
    private void SetupPort(int index)
    {
        resourceID = index;
        Debug.Log(index);
        Debug.Log(SpriteIndex.instance);
        sr.sprite = SpriteIndex.instance.GetSprite(index);
    }

    public int GetResourceID()
    {
        return resourceID;
    }

    public bool GetIsAccessible()
    {
        return isAccessible;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanPlace") && other.transform.parent.name.Contains("settlement")) 
        {
            isAccessible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanPlace") && other.transform.parent.name.Contains("settlement"))
        {
            isAccessible = false;
        }
    }
}
