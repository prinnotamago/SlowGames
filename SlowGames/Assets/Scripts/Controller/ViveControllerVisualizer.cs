using UnityEngine;
using System.Collections;

/// <summary>
///　Vivecontrollerがカメラに映っているかの機能
/// </summary>
public class ViveControllerVisualizer : MonoBehaviour
{
    /// <summary>
    /// カメラに映っているか
    /// </summary>
    public bool isVisualize
    {
        get; private set;
    }

    public void OnBecameInvisible()
    {
        isVisualize = true;
    }

    public void OnBecameVisible()
    {
        isVisualize = false;
    }
}
