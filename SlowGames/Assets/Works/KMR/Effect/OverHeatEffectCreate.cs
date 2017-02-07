using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeatEffectCreate : MonoBehaviour
{

    [SerializeField]
    GameObject overHeatEffect = null;

    void Start()
    {

    }

    void Update()
    {
        if (!SlowMotion._instance.limiterFlag)
        {
            //if (overHeatEffect.activeInHierarchy == true)
            //{
            //    overHeatEffect.SetActive(false);
            //}
            return;
        }


        if (overHeatEffect.activeInHierarchy == false)
        {
            overHeatEffect.SetActive(true);
        }
    }
}
