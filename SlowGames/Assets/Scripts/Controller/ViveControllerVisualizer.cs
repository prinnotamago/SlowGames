using UnityEngine;
using System.Collections;

/// <summary>
///　Vivecontrollerがカメラに映っているかの機能
/// </summary>
public class ViveControllerVisualizer : MonoBehaviour
{
    readonly int MAIN_CAMERA_TAG_NAME_HASH = "MainCamera".GetHashCode();

    /// <summary>
    /// カメラに映っているか
    /// </summary>
    public bool isVisualize
    {
        get; private set;
    }

    public void OnWillRenderObject()
    {
        if (Camera.current.tag.GetHashCode() != MAIN_CAMERA_TAG_NAME_HASH) return;
        isVisualize = true;
    }

    void Update()
    {
        isVisualize = false;
    }
}
