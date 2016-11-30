using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenParticleFinish : MonoBehaviour
{
    ParticleSystem[] _particleSystem;

    void Start()
    {
        _particleSystem = GetComponentsInChildren<ParticleSystem>();
        Debug.Log(_particleSystem.Length);
    }

    void Update()
    {
        ChildrenParticleFinishDestroy();
    }

    void ChildrenParticleFinishDestroy()
    {

        int count = 0;
             
        for(int i = 0; i< _particleSystem.Length; i++)
        {
            if (!_particleSystem[i].IsAlive())
            {
                count++;
            }
            
        }

        if(count == _particleSystem.Length)
        {
            Debug.Log("homo");
            Destroy(gameObject);
        }

    }



}
