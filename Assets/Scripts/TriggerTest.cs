using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("settlement.") && other.CompareTag("Block"))
        {
            other.GetComponent<InteractCity>().photonView.RPC("DisallowPlacement", Photon.Pun.RpcTarget.All, true);
        }
    }
}
