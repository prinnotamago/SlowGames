using UnityEngine;
using System.Linq;

/// <summary>
/// ViveControllerManager
/// </summary>
public class ViveControllerManager : MonoBehaviour
{
    /// <summary>
    /// スローモードになれるか
    /// </summary>
    public bool canSlowMode
    {
        get
        {
            return _viveControllerVisualizers.All(viveControllerVisualizer => viveControllerVisualizer.isVisualize);
        }
    }

    ViveControllerVisualizer[] _viveControllerVisualizers = new ViveControllerVisualizer[2];

    void Start()
    {
        _viveControllerVisualizers = FindObjectsOfType<ViveControllerVisualizer>();
    }

    void Update()
    {
        //Debug.Log(canSlowMode);
    }
}
