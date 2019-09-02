using UnityEngine;

public static class RollDice
{
    public static int Roll()
    {
        int d1 = Random.Range(1, 7);

        int d2 = Random.Range(1, 7);

        return d1 + d2;
    }
}
