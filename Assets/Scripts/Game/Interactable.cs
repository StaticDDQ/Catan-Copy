using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    public abstract void Interact(PlayerState player);
}
