using Photon.Pun;
using UnityEngine;


public class Selector : MonoBehaviour
{
    private bool placingRoad = false;
    private bool placingSettlement = false;
    private bool placingCity = false;

    private PlayerState ps;
    private PortUIControl portUIControl;
    private Camera cam;
    
    private bool setupPhase = false;
    [SerializeField] private LayerMask layermask = new LayerMask();

    private void Start()
    {
        ps = GetComponent<PlayerState>();
        cam = GetComponent<Camera>();
        portUIControl = transform.GetChild(0).GetComponent<PortUIControl>();
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
                    hit.transform.GetComponent<Interactable>().photonView.RPC("Interact",RpcTarget.All,PhotonNetwork.LocalPlayer.ActorNumber, setupPhase);
                }
                else if (hit.transform.CompareTag("Panel") && !hit.transform.parent.GetComponent<ResourceInfo>().IsUnderKnight() && !ps.HasPlacedKnight())
                {
                    hit.transform.parent.GetComponent<ResourceInfo>().photonView.RPC("SetUnderKnight", RpcTarget.All, true);
                    ps.SetPlacedKnight();
                }
                else if(hit.transform.CompareTag("Knight") && ps.IsMovingKnight())
                {
                    ps.HasMoveKnight(hit.transform.parent.GetComponent<ResourceInfo>());

                }
                else if (hit.transform.CompareTag("Port") && hit.transform.GetComponent<PortManager>().GetIsAccessible())
                {
                    portUIControl.PortUISetup(hit.transform.GetComponent<PortManager>().GetResourceID());
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

    public void SetSetupPhase(bool isSetup)
    {
        this.setupPhase = isSetup;
    }
}
