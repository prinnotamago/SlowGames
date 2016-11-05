using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineEnabled : MonoBehaviour
{

    [SerializeField]
    GameObject _child;

    void Start()
    {
        _child.SetActive(false);
    }

    void Update()
    {
        if (SlowMotion._instance.isSlow)
        {
            _child.SetActive(true);
        }
        else
            if (SlowMotion._instance.isSlow)
        {
            _child.SetActive(false);
        }


    }
}
