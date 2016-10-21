using UnityEngine;
using System.Collections;

/// <summary>
/// ゲームのスピードを変える。
/// スローモーション
/// </summary>
public class SlowMotion : MonoBehaviour {

    //[SerializeField]
    //UnityStandardAssets.ImageEffects.MotionBlur blur = null;
    // UnityStandardAssets.ImageEffects

    public static SlowMotion _instance = null;

    /// <summary>
    /// 残り時間
    /// </summary>
    private float _remainingTime = 0.0f;
    //public float RemainingTime { get; set; }

    // 今スロー中かどうか
    bool _isSlow = false;
    public bool isSlow { get { return _isSlow; } }

    // Use this for initialization
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    TimeElapsed();
    //}

    /// <summary>
    /// 残り時間を減らして、ゲームスピードを元に戻す処理
    /// </summary>
    //public void TimeElapsed()
    //{
    //    if (remainingTime > 0.0f && Time.timeScale != 1.0f)
    //    {
    //        remainingTime -= Time.unscaledDeltaTime;

    //        if (remainingTime <= 0.0f)
    //        {
    //            Time.timeScale = 1.0f;
    //            //if(blur != null)
    //            //{
    //            //    blur.enabled = false;
    //            //}
    //        }
    //    }
    //}

    /// <summary>
    /// ゲームの速度とその速度にする時間(秒)を決める。
    /// </summary>
    /// <param name="speed">ゲームの速度</param>
    /// <param name="time">時間(秒)</param>
    //public void SetMotionTime(float speed, float time)
    //{
    //    Time.timeScale = speed;
    //    remainingTime = time;
    //    //if (blur != null)
    //    //{
    //    //    blur.enabled = true;
    //    //}
    //}

    /// <summary>
    /// ゲーム速度を元に戻す。
    /// １回だけ呼ぶようにしてください。
    /// </summary>
    public void ResetSpeed()
    {

        Time.timeScale = 1.0f;

        AudioManager.instance.changeSourceAllPitch(1.0f);

        _isSlow = false;
    }

    /// <summary>
    /// ゲームの速さを変える
    /// １回だけ呼ぶようにしてください。
    /// </summary>
    /// <param name="speed">
    /// 1.0f で通常速度、
    /// 1.0f 以上で早く、
    /// 1.0f 以下で遅くなる
    /// </param>
    public void GameSpeed(float speed)
    {

        Time.timeScale = speed;

        AudioManager.instance.changeSourceAllPitch(speed);

        if (speed != 1.0f)
        {
            _isSlow = true;
        }
        else
        {
            _isSlow = false;
        }
    }
}
