using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionColorChange : MonoBehaviour
{

    //ゲージがMax状態のカラー
    [SerializeField]
    Color _gaugeMaxColor;

    //点滅する割合（0～1）
    [SerializeField]
    float _flashGaugeAmount = 0.3f;

    //点滅する間隔
    [SerializeField]
    float _intervalTime = 0.05f;

    Material _color;

    Color _startColor;

    float _time;

    Color _currentColor;

    float gauge;

    enum flashState
    {
        Bright,
        Dark
    }

    flashState _flashState;

    void Start()
    {
        _flashState = flashState.Bright;
        _time = _intervalTime;
        _color = GetComponent<Renderer>().material;
        _startColor = _gaugeMaxColor;
        gauge = SlowMotion._instance.slowTimeMax - count;
        _currentColor = new Color(_startColor.r * gauge, _startColor.g * gauge, _startColor.b * gauge);
        _color.EnableKeyword("_EMISSION");
        _color.SetColor("_EmissionColor", _currentColor);

    }


    void Update()
    {
        SlowMotionGaugeChange();
    }

    //スローモーションを使ってないときのゲージの量
    void CurrentSlowMotionGaugeColor()
    {
        _currentColor = new Color(_startColor.r * gauge, _startColor.g * gauge, _startColor.b * gauge);
        if (gauge >= 1)
        {
            gauge = 1;
        }

        _color.EnableKeyword("_EMISSION");
        _color.SetColor("_EmissionColor", _currentColor);
    }

    //スローモーション使用中にゲージが一定以下になると点滅する
    void FewSlowMotionGaugeFlash()
    {
        if (gauge > _flashGaugeAmount) return;
        _time -= Time.unscaledDeltaTime;

        if (_time > 0) return;
        if (_flashState == flashState.Bright)
        {
            _color.EnableKeyword("_EMISSION");
            _color.SetColor("_EmissionColor", _currentColor);
            _flashState = flashState.Dark;
        }
        else
        if (_flashState == flashState.Dark)
        {
            _color.EnableKeyword("_EMISSION");
            _color.SetColor("_EmissionColor", new Color(0, 0, 0));
            _flashState = flashState.Bright;
        }
        _time = _intervalTime;

    }


    void SlowMotionGaugeChange()
    {
        gauge = SlowMotion._instance.slowTime / SlowMotion._instance.slowTimeMax;

        if (SlowMotion._instance.isSlow)
        {
            FewSlowMotionGaugeFlash();
        }
        else
        if (!SlowMotion._instance.isSlow)
        {
            CurrentSlowMotionGaugeColor();
        }
    }

}
