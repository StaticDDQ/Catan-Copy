using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject rules = null;
    [SerializeField] private GameObject devCardPage = null;
    [SerializeField] private GameObject dropResourcePage = null;

    [SerializeField] private Text woodRemain = null;
    [SerializeField] private Text clayRemain = null;
    [SerializeField] private Text wheatRemain = null;
    [SerializeField] private Text stoneRemain = null;
    [SerializeField] private Text sheepRemain = null;

    [SerializeField] private Text wood = null;
    [SerializeField] private Text clay = null;
    [SerializeField] private Text wheat = null;
    [SerializeField] private Text stone = null;
    [SerializeField] private Text sheep = null;

    private int woodDrop = 0;
    private int clayDrop = 0;
    private int wheatDrop = 0;
    private int stoneDrop = 0;
    private int sheepDrop = 0;

    private int woodMax = 0;
    private int clayMax = 0;
    private int wheatMax = 0;
    private int stoneMax = 0;
    private int sheepMax = 0;

    private int halveResourcesAmnt = 0;

    [SerializeField] private PlayerState player = null;

    private bool rulesShowing = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && rulesShowing)
        {
            rules.SetActive(false);
            rulesShowing = false;
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

    public void SetDropPage(int[] resources)
    {
        woodMax = resources[0];
        clayMax = resources[1];
        wheatMax = resources[2];
        stoneMax = resources[3];
        sheepMax = resources[4];

        woodRemain.text = woodMax.ToString();
        clayRemain.text = clayMax.ToString();
        wheatRemain.text = wheatMax.ToString();
        stoneRemain.text = stoneMax.ToString();
        sheepRemain.text = sheepMax.ToString();

        foreach (int resource in resources)
        {
            halveResourcesAmnt += resource;
        }
        halveResourcesAmnt /= 2;

        dropResourcePage.SetActive(true);
    }

    public void WoodAdd()
    {
        if (woodDrop < woodMax)
        {
            woodDrop++;
            wood.text = woodDrop.ToString();
        }
    }

    public void WoodSubtract()
    {
        if (woodDrop > 0)
        {
            woodDrop--;
            wood.text = woodDrop.ToString();
        }
    }

    public void ClayAdd()
    {
        if (clayDrop < clayMax)
        {
            clayDrop++;
            clay.text = clayDrop.ToString();
        }
    }

    public void ClaySubtract()
    {
        if (clayDrop > 0)
        {
            clayDrop--;
            clay.text = clayDrop.ToString();
        }
    }

    public void WheatAdd()
    {
        if (wheatDrop < wheatMax)
        {
            wheatDrop++;
            wheat.text = wheatDrop.ToString();
        }
    }

    public void WheatSubtract()
    {
        if (wheatDrop > 0)
        {
            wheatDrop--;
            wheat.text = wheatDrop.ToString();
        }
    }

    public void StoneAdd()
    {
        if (stoneDrop < stoneMax)
        {
            stoneDrop++;
            stone.text = stoneDrop.ToString();
        }
    }

    public void StoneSubtract()
    {
        if (stoneDrop > 0)
        {
            stoneDrop--;
            stone.text = stoneDrop.ToString();
        }
    }

    public void SheepAdd()
    {
        if (sheepDrop < sheepMax)
        {
            sheepDrop++;
            sheep.text = sheepDrop.ToString();
        }
    }

    public void SheepSubtract()
    {
        if (sheepDrop > 0)
        {
            sheepDrop--;
            sheep.text = sheepDrop.ToString();
        }
    }

    public void DropResources()
    {
        if(woodDrop + clayDrop + wheatDrop + stoneDrop + sheepDrop == halveResourcesAmnt)
        {
            int[] newResources = { woodMax - woodDrop, clayMax - clayDrop, wheatMax - wheatDrop, stoneMax - stoneDrop, sheepMax - sheepDrop };
            player.SetResources(newResources);
            dropResourcePage.SetActive(false);
        }
    }
}
