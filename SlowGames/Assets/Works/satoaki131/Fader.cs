using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Fader{

    private static Image _fadeObject = null;

    public static Image fadeObject
    {
        set { _fadeObject = value; }
    }

    public static Color fadeColor
    {
        set { _fadeObject.color = value; }
    }

    private static float _alpha = 0.0f;
    public static float alpha
    {
        get { return _alpha; }
        set { _alpha = value; }
    }

    public static void setAlpha(float alpha, float r = 0.0f, float g = 0.0f, float b = 0.0f)
    {
        _fadeObject.color = new Color(r, g, b, _alpha);
        _alpha = alpha;
    }

    public static void FadeAdjustment(float alpha, float r = 0.0f, float g = 0.0f, float b = 0.0f)
    {
        _alpha = alpha;
        _fadeObject.color = new Color(r, g, b, _alpha);
    }
}
