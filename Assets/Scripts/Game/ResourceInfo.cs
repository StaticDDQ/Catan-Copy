using UnityEngine;
using TMPro;
using Photon.Pun;

public class ResourceInfo : MonoBehaviourPunCallbacks
{
    private int randomNum = -1;

    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep, 5-desert
    [SerializeField] private int resourceID = -1;
    [SerializeField] private TextMeshProUGUI numTxt = null;
    [SerializeField] private RangeChecker rChecker = null;

    public int GetResourceID() {
        return this.resourceID;
    }

    public int GetRandom()
    {
        return this.randomNum;
    }

    public override void OnEnable()
    {
        transform.SetParent(GameState.instance.GetTransform());    
    }

    public void SetRandom(int random)
    {
        photonView.RPC("SetPanelNumber", RpcTarget.All, random);
    }

    [PunRPC]
    void SetPanelNumber(int random)
    {
        randomNum = random;
        numTxt.text = random.ToString();
        if (random == 8 || random == 6)
        {
            numTxt.color = Color.red;
        }
    }

    public void DistributeResource()
    {
        rChecker.DistributeResource();
    }
}
