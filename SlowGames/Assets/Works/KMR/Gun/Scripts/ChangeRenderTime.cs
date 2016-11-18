using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderTime : MonoBehaviour
{

    TrailRenderer _trailRenderer;

    [SerializeField]
    float time = 2;

    public void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.time = 0;
    }



    // Update is called once per frame
    void Update()
    {
        if (SlowMotion._instance.isSlow)
        {
            _trailRenderer.time = time;
            
        }
        else
    if (!SlowMotion._instance.isSlow)
        {
            _trailRenderer.time = 0;
            _trailRenderer.Clear();
        }
    }
}
