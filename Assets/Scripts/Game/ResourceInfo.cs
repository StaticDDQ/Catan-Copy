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
    [SerializeField] private bool underKnight = false;

    [SerializeField] private GameObject particleEffect = null;
    [SerializeField] private GameObject knight;

    [SerializeField] private GameObject knightPrefab = null;

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
    private void SetPanelNumber(int random)
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
        if (!underKnight)
        {
            rChecker.DistributeResource();
            Instantiate(particleEffect, transform.position + new Vector3(0,1.75f,0), Quaternion.identity);
        }
    }

    [PunRPC]
    public void SetUnderKnight(bool underKnight)
    {
        this.underKnight = underKnight;

        if (underKnight)
        {
            knight = Instantiate(knightPrefab, transform);
            knight.transform.localPosition = new Vector3(0,0.5f,0);
            knight.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        } else
        {
            Destroy(knight);
        }
    }

    public bool IsUnderKnight()
    {
        return underKnight;
    }
}
