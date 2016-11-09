using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FPSをはじめに固定するスクリプト
/// </summary>
public class ChangeFrameRate : MonoBehaviour {

    [SerializeField]
    int _fps = 120;

	void Awake () {
        Application.targetFrameRate = _fps;
    }

    //void Update()
    //{
    //    float fps = 1f / Time.deltaTime;
    //    Debug.LogFormat("{0}fps", fps);
    //}
}
