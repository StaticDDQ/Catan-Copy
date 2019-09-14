using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSet : MonoBehaviour
{
    [SerializeField] private bool isConnected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Placed"))
        {
            isConnected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Placed"))
        {
            isConnected = false;
        }
    }

    public bool GetIsConnected()
    {
        return isConnected;
    }
}
