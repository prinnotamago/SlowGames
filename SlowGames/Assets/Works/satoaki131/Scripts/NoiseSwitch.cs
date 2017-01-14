using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSwitch : MonoBehaviour {

    private UnityStandardAssets.ImageEffects.NoiseAndGrain _noise = null;
    public UnityStandardAssets.ImageEffects.NoiseAndGrain noise
    {
        get { return _noise; }
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
    }

    /// <summary>
    /// じりじりをOnにする
    /// </summary>
    public void OnNoise()
    {
        _noise.enabled = true;
    }

    /// <summary>
    /// じりじりをOffにする
    /// </summary>
    public void OffNoise()
    {
        _noise.enabled = false;
    }

    /// <summary>
    /// じりじりの量を変える
    /// </summary>
    /// <param name="intensity"></param>
    public void setIntensity(float intensity)
    {
        intensity = intensity > 10.0f ? 10.0f : intensity < 0.0f ? 0.0f : intensity; //intensityが数値0～１以内じゃなければ上限値にする
        _noise.intensityMultiplier = intensity;
    }

}
