using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{

    public enum State
    {
        GunPut,
        End,
        Wait
    }
    private Dictionary<State, Action> _stateUpdate = null;
    private State _state = State.Wait;

    [SerializeField]
    private PutGunStand[] _put = null;

    [SerializeField]
    private Image[] _logo = null;

    [SerializeField]
    private TextMessage _thankyouText = null;

    [SerializeField, Range(0.5f, 6.0f)]
    private float _logoMoveEndTime = 2.0f;

    [SerializeField]
    private GameObject[] _desk = null;

    [SerializeField]
    private GameObject[] _stand = null;

    [SerializeField]
    private GameObject[] _gun = null;

    [SerializeField]
    private float _deskMoveSpeed = 0.0f;

    [SerializeField]
    private GameObject[] _deskSpotLight = null;

    private Material[] _gunMaterial = null;

    [SerializeField]
    private RingEmission[] _ring = null;

    void Start()
    {
        _stateUpdate = new Dictionary<State, Action>();
        _stateUpdate.Add(State.GunPut, GunPutUpdate);
        _stateUpdate.Add(State.Wait, () => { });
        _stateUpdate.Add(State.End, EndUpdate);

        _gunMaterial = new Material[2];
        for (int i = 0; i < _gun.Length; i++)
        {
            _gunMaterial[i] = _gun[i].GetComponent<Renderer>().material;
        }
        _logo[3].material.color = new Color(_logo[3].material.color.r, _logo[3].material.color.g, _logo[3].material.color.b, 0);
        _logo[2].material.color = new Color(_logo[2].material.color.r, _logo[2].material.color.g, _logo[2].material.color.b, 0);
        VoiceNumberStorage.setVoice();
        AudioManager.instance.playVoice(AudioName.VoiceName.IV16);
        StartCoroutine(AudioMessage());
    }

    private IEnumerator AudioMessage()
    {
        var time = 0.0f;
        while (time < 4.0f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        //_desk[0].SetActive(true);
        //_desk[1].SetActive(true);


        for (int i = 0; i < _desk.Length; i++)
        {
            iTween.ScaleTo(_desk[i], iTween.Hash(
                "y", 0.05f,
                "time", 1.0f,
                "easeType", iTween.EaseType.easeOutCubic
                ));
        }

        time = 0.0f;
        while (time < 1.0f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        _stand[0].SetActive(true);
        _stand[1].SetActive(true);
        _deskSpotLight[0].SetActive(true);
        _deskSpotLight[1].SetActive(true);

        _state = State.GunPut;
    }

    void Update()
    {
        //デバッグ用:台座に置いてないときでも戻れるように
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneChange.ChangeScene(SceneName.Name.Title, Color.white);
        }

        //デバッグ用:台座に置いてないときでも戻れるように
        if (Input.GetKeyDown(KeyCode.V))
        {
            _put[0].Test(_stand[0]);
            _put[1].Test(_stand[1]);
        }

        _stateUpdate[_state]();
    }

    /// <summary>
    /// 銃を置くまでのUpdate
    /// </summary>
    void GunPutUpdate()
    {
        if (!_put[0].isPutGun) return;
        if (!_put[1].isPutGun) return;

        _stand[0].SetActive(false);
        _stand[1].SetActive(false);


        //for(int i = 0; i < _desk.Length; i++)
        //{
        //    _gun[i].transform.parent = _desk[i].transform;
        //}

        _state = State.Wait;
        StartCoroutine(Production());
    }

    [SerializeField]
    private GameObject[] _particle = null;


    /// <summary>
    /// 最後の演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator Production()
    {
        AudioManager.instance.playVoice(AudioName.VoiceName.IV17);
        var time = 0.0f;
        var fadeTime = 0.0f;

        AudioManager.instance.playSe(AudioName.SeName.TitleLogoEnding);

        //銃が光る演出
        while (time < 2.0f)
        {
            time += Time.unscaledDeltaTime;
            for (int i = 0; i < _gunMaterial.Length; i++)
            {
                //var emission = Mathf.Lerp(0, 4, time / 2.0f);
                var emission = (float)Easing.OutCubic(time, 2.0f, 4, 0);
                //var emission = (float)Easing.OutQuint(time, 2.0f, 4, 0);
                var color = new Color(emission, emission, emission);
                _gunMaterial[i].EnableKeyword("_EMISSION");
                _gunMaterial[i].SetColor("_EmissionColor", color);
            }
            yield return null;
        }
        _gun[0].GetComponent<Rigidbody>().useGravity = false;
        _gun[1].GetComponent<Rigidbody>().useGravity = false;
        //台座が消える演出
        for (int i = 0; i < _desk.Length; i++)
        {
            iTween.ScaleTo(_desk[i], iTween.Hash(
                "y", 0.0f,
                "time", 1.0f,
                "easeType", iTween.EaseType.easeOutCubic
                ));
        }

        for (int i = 0; i < _gun.Length; i++)
        {
            iTween.RotateTo(_gun[i], iTween.Hash(
                "x", 0.0f,
                "time", 3.0f,
                "easeType", iTween.EaseType.linear
                ));
        }


        //光切ったらぽわぽわを出す
        for (int i = 0; i < _particle.Length; i++)
        {
            _particle[i].SetActive(true); //ぽわぽわ出る
        }

        time = 0.0f;
        var fadeProduction = false;
        //音声終わるの待つ
        while (time < 5.5f)
        {
            time += Time.unscaledDeltaTime;
            if (time > 3.0f)
            {
                fadeTime += Time.unscaledDeltaTime;
                for (int i = 0; i < _gun.Length; i++)
                {
                    if (!fadeProduction)
                    {

                        iTween.RotateTo(_gun[i], iTween.Hash(
                            "z", 330.0f,
                            "time", 2.5f,
                            "easeType", iTween.EaseType.linear
                            ));

                        iTween.MoveTo(_gun[i], iTween.Hash(
                            "y", 1.0f,
                            "time", 2.5f,
                            "easeType", iTween.EaseType.linear
                            ));

                        //銃のマテリアルの設定
                        var mat = _gun[i].GetComponent<Renderer>().material;

                        mat.SetFloat("_Mode", 2);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.EnableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = 3000;

                        _gun[i].GetComponent<Renderer>().material = mat;
                    }
                    var a = (float)Easing.Linear(fadeTime, 5.5f - 3.0f, 0, 1);
                    _gunMaterial[i].color = new Color(_gunMaterial[i].color.r, _gunMaterial[i].color.g, _gunMaterial[i].color.b, a); //fadeしていく
                }
                if(!fadeProduction)fadeProduction = true;
            }
            yield return null;
        }

        for (int i = 0; i < _desk.Length; i++)
        {
            _desk[i].SetActive(false);
            _gun[i].SetActive(false);
        }

        for (int i = 0; i < _ring.Length; i++)
        {
            if (!_ring[i].gameObject.activeSelf) continue;
            _ring[i].EmissionColor(false);
        }

        yield return new WaitForSecondsRealtime(1.0f);


        //thank you for playing演出
        _thankyouText.isMoveText = true;
        while (!_thankyouText.isPopText)
        {
            yield return null;
        }

        time = 0.0f;
        while(time < 1.0f)
        {
            time += Time.unscaledDeltaTime;
            var a = (float)Easing.InOutQuad(time, 1.0f, -1.0f, 1.0f);
            _thankyouText.setColor(new Color(_thankyouText.text.color.r, _thankyouText.text.color.g, _thankyouText.text.color.b, a));
            yield return null;
        }

        _thankyouText.gameObject.SetActive(false);

        //Logo演出
        //_logo[3].gameObject.SetActive(true);
        //logoを出す
        time = 0.0f;
        while (time < _logoMoveEndTime)
        {
            time += Time.unscaledDeltaTime;
            //中心から見えるようにしていく
            //var size = _maskPos.sizeDelta;
            //size.x = (float)Easing.Linear(time, _logoMoveEndTime, 600.0f, 0.0f);
            //_maskPos.sizeDelta = size;

            //var a = (float)Easing.InExp(time, _logoMoveEndTime, 1, 3);
            //var color = new Color(_logo[0].material.color.r, _logo[0].material.color.g, _logo[0].material.color.b, a);
            //_logo[0].material.EnableKeyword("_EMISSION");
            //_logo[0].material.SetColor("_EmissionColor", color);
            var a = (float)Easing.InOutQuad(time, 1.0f, 1.0f * 2, 0.0f);
            //_logo[0].color = new Color(_logo[0].color.r, _logo[0].color.g, _logo[0].color.b, a);
            _logo[3].material.color = new Color(_logo[3].color.r, _logo[3].color.g, _logo[3].color.b, a);

            yield return null;
        }

        //logoの円の演出
        time = 0.0f;
        while(time < 1.0f)
        {
            time += Time.unscaledDeltaTime;
            _logo[1].fillAmount = (float)Easing.OutExp(time, 1.0f, 1.0f, 0.0f);
            yield return null;
        }

        //logoの線の演出
        time = 0.0f;
        _logo[2].material.color = new Color(_logo[2].color.r, _logo[2].color.g, _logo[2].color.b, 1.0f);

        time = 0.0f;
        while (time < 0.3f)
        {
            time += Time.unscaledDeltaTime;
            var a = (float)Easing.OutExp(time, 1.0f, 3.0f, 1.247135f);
            var color = new Color(a, a, a, a);
            for (int i = 0; i < _logo.Length; i++)
            {
                if (_logo[i].material == null) continue;
                _logo[i].material.EnableKeyword("_EMISSION");
                _logo[i].material.SetColor("_EmissionColor", color);
            }
            yield return null;
        }

        _logo[0].color = new Color(_logo[0].color.r, _logo[0].color.g, _logo[0].color.b, 1.247135f);

        time = 0.0f;
        while (time < 0.3f)
        {
            time += Time.unscaledDeltaTime;
            var a = (float)Easing.OutExp(time, 1.0f, 1.247135f, 3.0f);
            var color = new Color(a, a, a, a);
            for (int i = 0; i < _logo.Length; i++)
            {
                if (_logo[i].material == null) continue;
                _logo[i].material.EnableKeyword("_EMISSION");
                _logo[i].material.SetColor("_EmissionColor", color);
            }
            yield return null;
        }

        //ざらざら演出
        NoiseSwitch.instance.noise.enabled = true;
        NoiseSwitch.instance.bloom.enabled = false;

        time = 0.0f;
        while (time < _logoMoveEndTime)
        {
            time += Time.unscaledDeltaTime;
            NoiseSwitch.instance.noise.intensityMultiplier = (float)Easing.OutCubic(time, _logoMoveEndTime * 2, 10.0f, 0.0f);
            yield return null;
        }
        yield return null;
        _state = State.End;
    }


    /// <summary>
    /// 演出が終わった後、タイトルに戻れるようにする処理
    /// </summary>
    void EndUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ChangeScene(SceneName.Name.Title, Color.white);
        }
    }


}
