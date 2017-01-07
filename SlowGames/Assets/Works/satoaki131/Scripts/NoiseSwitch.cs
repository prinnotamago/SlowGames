using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSwitch : MonoBehaviour {

    private GlitchFx _glitchFx = null;

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
        _glitchFx = GetComponent<GlitchFx>();
    }

    /// <summary>
    /// じりじりをOnにする
    /// </summary>
    public void OnGlitch()
    {
        _glitchFx.enabled = true;
    }

    /// <summary>
    /// じりじりをOffにする
    /// </summary>
    public void OffGlitch()
    {
        _glitchFx.enabled = false;
    }

    /// <summary>
    /// じりじりの量を変える
    /// </summary>
    /// <param name="intensity"></param>
    public void setIntensity(float intensity)
    {
        intensity = intensity > 1.0f ? 1.0f : intensity < 0.0f ? 0.0f : intensity; //intensityが数値0～１以内じゃなければ上限値にする
        _glitchFx.intensity = intensity;
    }

}
