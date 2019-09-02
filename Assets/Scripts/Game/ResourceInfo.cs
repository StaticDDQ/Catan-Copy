using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResourceInfo : MonoBehaviour
{
    private int randomNum = -1;

    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep, 5-desert
    [SerializeField] private int resourceID = -1;
    [SerializeField] private TextMeshProUGUI numTxt = null;
    private List<ObjectHolder> currPlaced;

    private void Start()
    {
        currPlaced = new List<ObjectHolder>();
    }

    public int GetResourceID() {
        return this.resourceID;
    }

    public int GetRandom()
    {
        return this.randomNum;
    }

    public void SetRandom(int random)
    {
        randomNum = random;
        numTxt.text = random.ToString();
        if(random == 8 || random == 6)
        {
            numTxt.color = Color.red;
        }
    }

    public void DistributeResource()
    {
        foreach(ObjectHolder placed in currPlaced)
        {
            placed.GetOwner().GetResource(resourceID);
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
