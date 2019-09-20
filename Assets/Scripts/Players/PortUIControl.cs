using UnityEngine;
using UnityEngine.UI;

public class PortUIControl : MonoBehaviour
{
    private int[] resourcesUI;
    [SerializeField] private Text[] resourcesText = null;

    [SerializeField] private Text resourceCounterText = null;
    [SerializeField] private ResourceUI rUI = null;

    [SerializeField] private GameObject portUI = null;
    [SerializeField] private PlayerState ps = null;

    private int index;
    private int resourceCounter;
    private int counter;
    private int reducer;

    private bool selectRsourceExchange = false;

    public void PortUISetup(int portID)
    {
        if (portID == 5)
        {
            reducer = 3;
            rUI.enabled = true;
            selectRsourceExchange = false;
        } else
        {
            reducer = 2;
            rUI.AssignImage(portID);
            rUI.enabled = false;

            SetResource(portID);
        }

        portUI.SetActive(true);
    }

    public void SetResource(int resourceIndex)
    {
        // amount of that resource the player haves
        resourceCounter = ps.GetResourceAmount(resourceIndex);
        // resource index
        index = resourceIndex;
        // amount of that resource to be exchanged
        counter = 0;
        // how much is gained
        resourcesUI = new int[] { 0, 0, 0, 0, 0 };

        foreach(Text resourceText in resourcesText)
        {
            resourceText.text = "0";
        }

        resourceCounterText.text = "0";

        selectRsourceExchange = true;
    }

    public void CountUp(int resource)
    {
        if (counter + reducer <= resourceCounter)
        {
            resourcesUI[resource]++;
            resourcesText[resource].text = resourcesUI[resource].ToString();

            counter += reducer;
            resourceCounterText.text = resourceCounter.ToString();
        }
    }

    public void CountDown(int resource)
    {
        if (resourcesUI[resource] > 0)
        {
            resourcesUI[resource]--;
            resourcesText[resource].text = resourcesUI[resource].ToString();

            counter -= reducer;
            resourceCounterText.text = resourceCounter.ToString();
        }
    }

    public void FinishSelecting()
    {
        if (selectRsourceExchange)
        {
            resourcesUI[index] -= counter;

            transform.parent.GetComponent<PlayerState>().ExchangeResource(resourcesUI);
        }
        
        portUI.SetActive(false);
    }

    public void ClosePanel()
    {
        portUI.SetActive(false);
    }
}
