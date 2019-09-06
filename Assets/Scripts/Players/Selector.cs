﻿using UnityEngine;

public class Selector : MonoBehaviour
{
    private bool placingRoad = false;
    private bool placingSettlement = false;
    private bool placingCity = false;

    private PlayerState ps;
    private Camera cam;
    [SerializeField] private LayerMask layermask = new LayerMask();

    private void Start()
    {
        ps = GetComponent<PlayerState>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            bool isHit = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit,Mathf.Infinity, layermask);

            if (isHit)
            {
                if (hit.transform.CompareTag("Block"))
                {
                    hit.transform.GetComponent<Interactable>().photonView.RPC("Interact",Photon.Pun.RpcTarget.All,ps);
                }
            }
        }
    }

    public void SetPlacingRoad(bool isPlacing)
    {
        placingRoad = isPlacing;
    }

    public void SetPlacingSettlement(bool isPlacing)
    {
        placingSettlement = isPlacing;
    }

    public void SetPlacingCity(bool isPlacing)
    {
        placingCity = isPlacing;
    }

    public bool GetPlacingRoad()
    {
        return placingRoad;
    }

    public bool GetPlacingSettlement()
    {
        return placingSettlement;
    }

    public bool GetPlacingCity()
    {
        return placingCity;
    }
}