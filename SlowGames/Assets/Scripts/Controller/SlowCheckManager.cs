using UnityEngine;
using System.Linq;
using System.Collections;

public class SlowCheckManager : MonoBehaviour
{
    /// <summary>
    /// スローモードになれるか
    /// </summary>
    public bool canSlowMode
    {
        get
        {
            if (!(_viveControllerVisualizers.Length >= 2)) return false;
            return _viveControllerVisualizers.All(viveControllerVisualizer => viveControllerVisualizer.isHit);
        }
    }

    SlowChecker[] _viveControllerVisualizers = new SlowChecker[2];

    void Start()
    {
        _viveControllerVisualizers = FindObjectsOfType<SlowChecker>();
    }

    void Update()
    {
        if (!(_viveControllerVisualizers.Length >= 2))
        {
            _viveControllerVisualizers = FindObjectsOfType<SlowChecker>();
            return;
        }

        foreach (var viveControllerVisualizer in _viveControllerVisualizers)
        {
            if (viveControllerVisualizer != null) continue;
            _viveControllerVisualizers = FindObjectsOfType<SlowChecker>();
            break;
        }
    }
}
