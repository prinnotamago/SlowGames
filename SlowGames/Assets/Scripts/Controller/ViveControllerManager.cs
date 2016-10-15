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
            if (_viveControllerVisualizers.Length >= 2)
            {
                return _viveControllerVisualizers.All(viveControllerVisualizer => viveControllerVisualizer.isVisualize);
            }
            else
            {
                return false;
            }
        }
    }

    ViveControllerVisualizer[] _viveControllerVisualizers = new ViveControllerVisualizer[2];

    void Start()
    {
        _viveControllerVisualizers = FindObjectsOfType<ViveControllerVisualizer>();
    }

    void Update()
    {
        if (!(_viveControllerVisualizers.Length >= 2))
        {
            _viveControllerVisualizers = FindObjectsOfType<ViveControllerVisualizer>();
        }

        foreach (var viveControllerVisualizer in _viveControllerVisualizers)
        {
            if (viveControllerVisualizer == null)
            {
                _viveControllerVisualizers = FindObjectsOfType<ViveControllerVisualizer>();
                break;
            }
        }
        //Debug.Log(canSlowMode);
    }
}
