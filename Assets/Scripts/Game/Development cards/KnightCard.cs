using Photon.Pun;

public class KnightCard : DevelopmentCards
{
    private bool isUsed = false;

    public override void Effect()
    {
        if (!isUsed)
        {
            PlayerState.instance.MoveKnight(PhotonNetwork.LocalPlayer.ActorNumber);
            isUsed = true;
            MessageManager.instance.photonView.RPC("SendMessageText", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + "has used a knight");
            RewardManager.instance.IncreaseKnights();
        }
    }
}
