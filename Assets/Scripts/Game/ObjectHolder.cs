using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    private PlayerState owner;

    public void SetOwner(PlayerState newOwner)
    {
        owner = newOwner;
        GetComponent<Renderer>().material.SetColor("_BaseColor", owner.GetColor());
    }

    public PlayerState GetOwner()
    {
        return owner;
    }
}
