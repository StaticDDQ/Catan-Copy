public abstract class DevelopmentCards
{
    private bool canUse = false;

    public abstract bool Effect();

    public void SetUse(bool use)
    {
        canUse = use;
    }
}