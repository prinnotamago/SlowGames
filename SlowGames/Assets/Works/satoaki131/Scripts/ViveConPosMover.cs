using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveConPosMover : MonoBehaviour {

    [SerializeField]
    private GameObject ViveCon = null;

    void Update()
    {
        var localPos = ViveCon.transform.position;
        var localRotate = ViveCon.transform.eulerAngles;
        if(localPos.x > 0.0f)
        {
            localPos.x += 0.4f;
            localRotate = Vector3.up * 90;
        }
        else
        {
            localPos.x -= 0.4f;
            localRotate = Vector3.down * 90;
        }
        localPos.z -= 0.2f;
        transform.localPosition = localPos;
        transform.eulerAngles = localRotate;
    }

}
