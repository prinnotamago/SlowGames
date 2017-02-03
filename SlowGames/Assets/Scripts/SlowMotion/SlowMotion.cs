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
    //private float _remainingTime = 0.0f;
    //public float remainingTime { get { return _remainingTime; } }
    //public float RemainingTime { get; set; }

    /// <summary>
    /// 今スロー中かどうか
    /// </summary>
    bool _isSlow = false;
    /// <summary>
    /// 今スロー中かどうか
    /// </summary>
    public bool isSlow { get { return _isSlow; } }

    /// <summary>
    /// スローに制限をつけるかどうか
    /// </summary>
    [SerializeField]
    bool _isLimit = false;
    public bool isLimit
    {
        get { return _isLimit; }
        set {
            _isLimit = value;
            if (!_isLimit)
            {
                slowTime = _slowTimeMax;
            }
        }
    }

    /// <summary>
    /// スローの最大時間
    /// </summary>
    [SerializeField]
    float _slowTimeMax = 10.0f;
    public float slowTimeMax { get { return _slowTimeMax; } }

    /// <summary>
    /// スローがだんだん切れていく時間
    /// </summary>
    [SerializeField]
    float _slowDownLimitTime = 5.0f;

    /// <summary>
    /// スローがだんだん切れていく時間になったらどのくらいの速さで戻るのかを計算で求める
    /// </summary>
    float _slowDownSpeed = 0.0f;

    /// <summary>
    /// スローの速さ
    /// </summary>
    float _slowSpeed = 0.0f;

    /// <summary>
    ///  スローがだんだん切れていく時間になったらエフェクトの範囲を狭める速さを計算で求める
    /// </summary>
    float _effectownSpeed = 0.0f;

    float _effectRangeMax = 0.8f;
    float _effectRangeSize = 0.8f;

    bool _isEffectStart = false;

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

    bool _limiterFlag = false;
    public bool limiterFlag
    {
        get { return _limiterFlag; }
        set { _limiterFlag = value; }
    }

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
            if (slowTime > _slowDownLimitTime)
            {
                _slowTime -= Time.unscaledDeltaTime;
            }
            else if (slowTime > 0.0f)
            {
                _slowTime -= Time.unscaledDeltaTime;
                _slowDownSpeed = (Time.timeScale - 1.0f) / _slowDownLimitTime * Time.unscaledDeltaTime;
                float speed = Time.timeScale - _slowDownSpeed;
                GameSpeed(speed);

                _effectRangeSize = (slowTime / _slowDownLimitTime) * _effectRangeMax;

                //float size = (_effectRangeSize - _v.intensity) / _slowDownLimitTime;

                //_v.intensity -= Time.unscaledDeltaTime * size;

                if (_isEffectStart)
                {
                    _v.intensity = _effectRangeSize;
                }
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
        _slowSpeed = 1.0f;

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
        _slowSpeed = speed;
        _slowDownSpeed = (1.0f - speed) / _slowDownLimitTime;

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
        _isEffectStart = true;
        if (slowTime > _slowDownLimitTime)
        {
            _effectRangeSize = _effectRangeMax;
        }
        while (_v.intensity < _effectRangeSize)
        {
            if (!SlowMotion._instance.isSlow) { break; }
            _v.intensity += 0.1f;
            yield return 0;
        }
        _isEffectStart = false;
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
