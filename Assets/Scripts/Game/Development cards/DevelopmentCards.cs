using UnityEngine;

public abstract class DevelopmentCards : MonoBehaviour
{
    private bool canUse = false;

    public abstract void Effect();

    public void SetUse(bool use)
    {
        canUse = use;
    }
}