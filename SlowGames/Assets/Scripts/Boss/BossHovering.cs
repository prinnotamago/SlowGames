using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHovering : MonoBehaviour {

    [SerializeField]
    float _speed = 5.0f;

    float _timeCount = 0.0f;

    [SerializeField]
    float length = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _timeCount += Time.deltaTime * _speed;
        transform.position += Vector3.up * Mathf.Sin(_timeCount) * length;
    }
}
