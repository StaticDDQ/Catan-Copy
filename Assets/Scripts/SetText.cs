using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetText : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text givenText = null;

    [PunRPC]
    public void SetGivenText(string txt)
    {
        Debug.Log("Dice roll: " + txt);
        givenText.text = txt;
    }
}
