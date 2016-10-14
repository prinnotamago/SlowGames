using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CanvasTest : MonoBehaviour {

    [SerializeField]
    private Canvas test;

    void Awake()
    {
        var a = Instantiate(test);
        var localPosition = transform.localPosition;
        localPosition.z += 3.0f;
        a.transform.localPosition = localPosition;
        a.worldCamera = GetComponent<Camera>();
    }

}
