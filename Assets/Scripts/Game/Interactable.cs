using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    public abstract void Interact(int player, bool planning);
}
