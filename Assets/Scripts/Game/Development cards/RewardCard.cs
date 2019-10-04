public class RewardCard : DevelopmentCards
{
    public override void Effect()
    {
        PlayerState.instance.IncreasePoint();
    }
}
