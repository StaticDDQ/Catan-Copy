using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    private int playerID;
    [SerializeField] private Text nameText = null;

    public void SetPlayerID(int id, string nickname)
    {
        playerID = id;
        nameText.text = nickname;
    }

    public int GetPlayerID()
    {
        return playerID;
    }
}
