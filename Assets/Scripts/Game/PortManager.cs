using UnityEngine;
using Photon.Pun;

public class PortManager : MonoBehaviourPunCallbacks
{
    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep, 5-random
    private int resourceID = -1;
    private bool isAccessible = false;

    [PunRPC]
    public void SetResourceID(int index)
    {
        resourceID = index;
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
