using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject rules = null;
    [SerializeField] private GameObject devCardPage = null;
    [SerializeField] private GameObject dropResourcePage = null;
    [SerializeField] private GameObject tradePanel = null;
    [SerializeField] private Animator blockPanel = null;

    [SerializeField] private Text[] resourceMaxText = null;
    [SerializeField] private Text[] resourceDropText = null;

    [SerializeField] private Transform devPanel = null;
    [SerializeField] private GameObject knightCard = null;
    [SerializeField] private GameObject victoryCard = null;

    private int[] resourceMax;
    private int[] resourceDrop;

    private int halveResourcesAmnt = 0;

    private bool rulesShowing = false;
    private bool displayBlockPanel = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && rulesShowing)
        {
            rules.SetActive(false);
            rulesShowing = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && !displayBlockPanel)
        {
            blockPanel.Play("showBlock");
            displayBlockPanel = true;
        } else if(Input.GetKeyUp(KeyCode.LeftAlt) && displayBlockPanel)
        {
            blockPanel.Play("hideBlock");
            displayBlockPanel = false;
        }
    }

    public void ShowRules()
    {
        rules.SetActive(true);
        rulesShowing = true;
    }

    public void OpenDevCardPage()
    {
        devCardPage.SetActive(true);
    }

    public void CloseDevCardPage()
    {
        devCardPage.SetActive(false);
    }

    public void OpenTradePanel()
    {
        tradePanel.SetActive(true);
    }

    public void CloseTradePanel()
    {
        tradePanel.SetActive(false);
    }

    //setup for drop resource page, calculate how much resources must be dropped
    public void SetDropPage(int[] resources)
    {
        resourceMax = resources;

        for(int i = 0; i < 5; i++)
        {
            resourceMaxText[i].text = resourceMax[i].ToString();
            halveResourcesAmnt += resourceMax[i];
        }

        halveResourcesAmnt /= 2;

        dropResourcePage.SetActive(true);

        resourceDrop = new int[] { 0, 0, 0, 0, 0 };
    }

    // UI button functions
    public void ResourceAdd(int index)
    {
        if(resourceDrop[index] < resourceMax[index])
        {
            resourceDropText[index].text = (++resourceDrop[index]).ToString();
        }
    }

    public void ResourceSubtract(int index)
    {
        if(resourceDrop[index] > 0)
        {
            resourceDropText[index].text = (--resourceDrop[index]).ToString();
        }
    }

    public void DropResources()
    {
        int sum = 0;
        int[] newResources = new int[5];

        for(int i = 0; i < 5; i++)
        {
            sum += resourceDrop[i];
            newResources[i] = -resourceDrop[i];
        }

        if(sum == halveResourcesAmnt)
        {
            PlayerState.instance.ExchangeResource(newResources);
            dropResourcePage.SetActive(false);
        }
    }

    public void AddDevCard(DevelopmentCards devCard)
    {
        if(devCard.GetType() == typeof(RewardCard))
        {
            Instantiate(victoryCard, devPanel);
        }
        else
        {
            Instantiate(knightCard, devPanel);
        }
    }
}
