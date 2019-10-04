using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetDice : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image d1 = null;
    [SerializeField] private Image d2 = null;

    [SerializeField] private Sprite[] diceFaces = null;

    [PunRPC]
    public void DisplayDice(int d1, int d2)
    {
        Debug.Log("Dice roll: " + d1 + " and " + d2);

        this.d1.sprite = diceFaces[d1 - 1];
        this.d2.sprite = diceFaces[d2 - 1];
    }
}
