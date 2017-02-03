using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSpeedChange : MonoBehaviour
{

    ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }


    void Update()
    {

        if (!SlowMotion._instance.isSlow)
        {
            _particleSystem.playbackSpeed = 1;
            return;
        }


        if (SlowMotion._instance.isSlow)
        {
            _particleSystem.playbackSpeed = 1  * SlowMotion._instance.RealSpeed();
        }

        //Debug.Log(_particleSystem.playbackSpeed);

    }
}
