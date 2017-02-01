using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathPart : MonoBehaviour {

    [SerializeField]
    float _slowStopTime = 3.0f;

    [SerializeField]
    GameObject _deathParticle = null;

    // Use this for initialization
    void Start () {
        //// スローじゃなかったらスローにする
        //if (!SlowMotion._instance.isSlow)
        //{
        //    SlowMotion._instance.GameSpeed(0.1f);
        //    SlowMotion._instance.isLimit = false;
        //}

        var particle = Instantiate(_deathParticle);
        particle.transform.position = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //_slowStopTime -= Time.unscaledDeltaTime;
        //if (_slowStopTime <= 0)
        //{
        //    // スローだったら解除
        //    if (SlowMotion._instance.isSlow)
        //    {
        //        SlowMotion._instance.ResetSpeed();
        //        SlowMotion._instance.isLimit = true;
        //    }
        //}
    }
}
