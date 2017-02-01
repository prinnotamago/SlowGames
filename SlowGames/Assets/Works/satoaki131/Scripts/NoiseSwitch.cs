using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSwitch : MonoBehaviour {

    private UnityStandardAssets.ImageEffects.NoiseAndGrain _noise = null;
    public UnityStandardAssets.ImageEffects.NoiseAndGrain noise
    {
        get { return _noise; }
    }

    private UnityStandardAssets.ImageEffects.Bloom _bloom = null;
    public UnityStandardAssets.ImageEffects.Bloom bloom
    {
        get { return _bloom; }
    }

    public static NoiseSwitch instance
    {
        get; private set;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _noise = GetComponent<UnityStandardAssets.ImageEffects.NoiseAndGrain>();
        _bloom = GetComponent<UnityStandardAssets.ImageEffects.Bloom>();
    }
}
