using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool isStarting = false;
    private float timeLeft;
    public static Timer instance;

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    public void StartTimer(float amount)
    {
        timeLeft = amount;
        isStarting = true;
    }

    private void Update()
    {
        if (isStarting)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                TradeController.instance.TimesUp();
                isStarting = false;
            }
        }
    }
}
