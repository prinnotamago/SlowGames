using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoShotSlowHeal : MonoBehaviour {

    [SerializeField]
    PlayerShot[] _shots;

    [SerializeField]
    float _healTimeLimit = 1.0f;
    float _healTimeCount = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SlowMotion._instance.isSlow)
        {
            _healTimeCount = 0.0f;
            return;
        }

        bool isShot = true;
        foreach(var shot in _shots)
        {
            //Debug.Log(shot.isShot);
            if (shot.isShot)
            {
                isShot = false;
            }
        }

        if (isShot)
        {
            _healTimeCount += Time.unscaledDeltaTime;
        }
        else
        {
            _healTimeCount = 0.0f;
        }

        if (_healTimeLimit < _healTimeCount)
        {
            Debug.Log("回復");
            SlowMotion._instance.slowTime = SlowMotion._instance.slowTimeMax;
            _healTimeCount = 0.0f;
        }
	}
}
