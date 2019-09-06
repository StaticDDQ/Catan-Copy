using System.Collections.Generic;
using UnityEngine;

public class RangeChecker : MonoBehaviour
{
    private List<ObjectHolder> currPlaced;

    private void Start()
    {
        currPlaced = new List<ObjectHolder>();
    }

    public void DistributeResource()
    {
        foreach (ObjectHolder placed in currPlaced)
        {
            placed.GetOwner().GetResource(transform.parent.GetComponent<ResourceInfo>().GetResourceID());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Placed"))
        {
            currPlaced.Add(other.GetComponent<ObjectHolder>());
        }
    }
}
