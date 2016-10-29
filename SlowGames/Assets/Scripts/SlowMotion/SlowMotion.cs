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

    /// <summary>
    /// 今スロー中かどうか
    /// </summary>
    bool _isSlow = false;
    /// <summary>
    /// 今スロー中かどうか
    /// </summary>
    public bool isSlow { get { return _isSlow; } }

    [SerializeField]
    bool _isLimit = false;
    [SerializeField]
    float _slowTimeMax = 5.0f;
    //[SerializeField]
    float _slowTime = 0.0f;
    public float slowTime {
        get
        {
            return _slowTime;
        }
        set
        {
            _slowTime += value;
            if(_slowTime < 0)
            {
                _slowTime = 0;
            }
            else if(_slowTime > _slowTimeMax)
            {
                _slowTime = _slowTimeMax;
            }
        }
    }

    [SerializeField]
    bool _isEffect = false;
    [SerializeField]
    UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration _v;

    // Use this for initialization
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        slowTime = _slowTimeMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isLimit) { return; }
        if (SlowMotion._instance.isSlow)
        {
            if (slowTime > 0.0f)
            {
                _slowTime -= Time.unscaledDeltaTime;
            }
            else
            {
                _slowTime = 0.0f;
                SlowMotion._instance.ResetSpeed();
            }
        }
    }

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

        if (!_isEffect) { return; }
        StartCoroutine(SlowEnd());
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
            if (!_isEffect) { return; }
            StartCoroutine(SlowStart());
        }
        else
        {
            _isSlow = false;
            if (!_isEffect) { return; }
            StartCoroutine(SlowEnd());
        }
    }

    /// <summary>
    /// スロー時に通常のスピードになるような値を返す。
    /// かけてあげれば通常のスピードになる
    /// </summary>
    /// <returns></returns>
    public float RealSpeed()
    {
        // 1.0f  1.0f
        // 0.75f 1.5f
        // 0.5f  2.0f
        // 0.2f  5.0f
        // 0.1f  10.0f
        return 1.0f / Time.timeScale;
    }

    IEnumerator SlowStart()
    {
        while (_v.intensity < 0.8f)
        {
            if (!SlowMotion._instance.isSlow) { break; }
            _v.intensity += 0.1f;
            yield return 0;
        }
    }

    IEnumerator SlowEnd()
    {
        while (_v.intensity > 0.0f)
        {
            if (SlowMotion._instance.isSlow) { break; }
            _v.intensity -= 0.1f;
            yield return 0;
        }
    }
}
