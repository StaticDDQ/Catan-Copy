using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject rules = null;
    [SerializeField] private GameObject devCardPage = null;
    [SerializeField] private GameObject dropResourcePage = null;

    //[SerializeField] private PlayerState player;

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

    public void DropPlayersResources()
    {
        dropResourcePage.SetActive(true);
    }

    public void OpenDevCardPage()
    {
        devCardPage.SetActive(true);
    }

    public void CloseDevCardPage()
    {
        devCardPage.SetActive(false);
    }
}
