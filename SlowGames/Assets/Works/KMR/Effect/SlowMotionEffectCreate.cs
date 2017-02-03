using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionEffectCreate : MonoBehaviour
{

    [SerializeField]
    GameObject slowMotionEffect = null;
    
    void Start()
    {

    }

    
    void Update()
    {
        if (SlowMotion._instance.limiterFlag) return;

        if (!SlowMotion._instance.isSlow)
        {
            if (slowMotionEffect.activeInHierarchy == true)
            {
                slowMotionEffect.SetActive(false);
            }
            return;
        }


        if (slowMotionEffect.activeInHierarchy == false)
        {
            slowMotionEffect.SetActive(true);
        }

    }
}
