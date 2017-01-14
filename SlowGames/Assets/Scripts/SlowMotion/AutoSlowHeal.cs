using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSlowHeal : MonoBehaviour {

    [SerializeField]
    float _slowHealTimeMax = 5.0f;  // 回復する時間
    float _slowHealTime = 0.0f;     // 実際にカウントする部分

	// Use this for initialization
	//void Start () {
		
	//}

    // Update is called once per frame
    void Update()
    {
        // スロー中でないなら
        if (!SlowMotion._instance.isSlow)
        {
            // スローゲージが最大値なら抜ける
            if (SlowMotion._instance.slowTime == SlowMotion._instance.slowTimeMax) { return; }

            // 一定時間が過ぎたらゲージを回復
            if (_slowHealTimeMax > _slowHealTime)
            {
                SlowMotion._instance.slowTime = SlowMotion._instance.slowTimeMax;
                _slowHealTime = 0.0f;

                // チャージ完了ボイス
                AudioManager.instance.playSe(AudioName.SeName.V09);
            }
            else
            {
                // 時間経過
                _slowHealTime += Time.deltaTime;
            }
        }
        // スロー中ならカウントを０にする
        else
        {
            _slowHealTime = 0.0f;
        }
    }
}
