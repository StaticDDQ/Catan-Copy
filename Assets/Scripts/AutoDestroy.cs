﻿using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (ps && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
