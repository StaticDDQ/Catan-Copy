using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    private PlayerState owner;

    public void SetOwner(PlayerState newOwner)
    {
        owner = newOwner;
    }

    public PlayerState GetOwner()
    {
        return owner;
    }
}
