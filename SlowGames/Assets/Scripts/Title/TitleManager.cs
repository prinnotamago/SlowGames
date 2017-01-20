using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    enum State
    {
        Title,
        Wait
    }

    /// <summary>
    /// シーンチェンジに必要なアイテム
    /// </summary>
    [SerializeField]
    ViveGrab[] _items;

    [SerializeField]
    private Light[] _spotLights = null;

    [SerializeField]
    private GameObject[] _gunStand = null;

    [SerializeField]
    private GameObject[] _desk = null;

    [SerializeField]
    private GameObject[] _gun = null;

    [SerializeField]
    private GameObject[] _cameraRig = null;

    [SerializeField]
    private Canvas _idCanvas = null;

    [SerializeField]
    private Canvas _descriptionPanel = null;
    private Text _descriptionText = null;

    private TurtrealEnemyManager _enemyManager = null;

    private PlayerShot[] _playerShot = null;

    //[SerializeField]
    //private GameObject _door = null; //ドア用のオブジェクト

    //[SerializeField]
    //private Light _afterShade = null; //後光用のライト

    [SerializeField, Range(1.0f, 3.0f)]
    private float END_TIME = 2.0f;

    [SerializeField]
    private float _moveSpeed = 1.0f;

    [SerializeField]
    private GameObject[] _viveControllerModel = null;
    private Material[] _viveMaterial = null; //0:body, 1:slowButton 2:trigger, 3:grip
    private Material[] _viveMaterial2 = null; //0:body, 1:slowButton 2:trigger, 3:grip

    [SerializeField]
    private GameObject _logo = null;

    [SerializeField]
    private Animator _arrowAnim = null;

    [SerializeField]
    private Vector3[] TURTREAL_POS;

    private float _time = 0.0f;
    private float _voiceTIme = 0.0f;

    /// <summary>
    /// trueになる前にEnemyが死んだら復活させるためのbool
    /// Turtrealが終わったらtrueにして、シーン遷移時、必ずfalseにすること
    /// </summary>
    public static bool isTurtreal
    {
        get; private set;
    }

    private Dictionary<State, Action> _stateUpdate = null;
    private State _state = State.Title;

    void Start()
    {
        _stateUpdate = new Dictionary<State, Action>();
        _stateUpdate.Add(State.Title, TitleUpdate);
        _stateUpdate.Add(State.Wait, () => { });

        _enemyManager = FindObjectOfType<TurtrealEnemyManager>();
        isTurtreal = false;

        _descriptionText = _descriptionPanel.GetComponentInChildren<Text>();

        _viveMaterial = new Material[4];
        _viveMaterial2 = new Material[4];
        for (int i = 0; i < _viveControllerModel[0].GetComponentInChildren<Renderer>().materials.Length; i++)
        {
            _viveMaterial[i] = _viveControllerModel[0].GetComponentInChildren<Renderer>().materials[i];
            _viveMaterial2[i] = _viveControllerModel[1].GetComponentInChildren<Renderer>().materials[i];
        }

        //コントローラーの向きを変える
        _viveControllerModel[0].transform.Rotate(0, 90, 0);
        _viveControllerModel[1].transform.Rotate(0, -90, 0);



        _viveControllerModel[0].SetActive(false);
        _viveControllerModel[1].SetActive(false);
        _arrowAnim.gameObject.SetActive(false);
    }

    void Update()
    {
        _stateUpdate[_state]();
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    AudioManager.instance.playSe(AudioName.SeName.Reload);
        //}
        ////デバック用
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    SceneChange.ChangeScene(SceneName.Name.MainGame, Color.white);
        //}
    }

    private IEnumerator NoiseCancel()
    {
        var time = 0.0f;
        while (NoiseSwitch.instance.noise.intensityMultiplier > 0.0f)
        {
            time += Time.unscaledDeltaTime;
            NoiseSwitch.instance.noise.intensityMultiplier = (float)Easing.InCubic(time, 2.0f, 0.0f, 10.0f);
            yield return null;
        }

        _logo.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Authentication());
    }

    /// <summary>
    /// TitleのUpdate(銃を台座からとるまで)
    /// </summary>
    void TitleUpdate()
    {
        _time += Time.deltaTime;
        _voiceTIme += Time.deltaTime;

        if (_time > 1.0f)
        {
            _viveMaterial[2].EnableKeyword("_EMISSION");
            _viveMaterial[2].SetColor("_EmissionColor", Color.black);
            _viveMaterial2[2].EnableKeyword("_EMISSION");
            _viveMaterial2[2].SetColor("_EmissionColor", Color.black);

            _time = 0.0f;
        }
        else if (_time > 0.5f)
        {
            _viveMaterial[2].EnableKeyword("_EMISSION");
            _viveMaterial[2].SetColor("_EmissionColor", Color.white);
            _viveMaterial2[2].EnableKeyword("_EMISSION");
            _viveMaterial2[2].SetColor("_EmissionColor", Color.white);
        }

        // 必要なアイテムを手に持っているか確かめる
        bool isChange = true;
        foreach (var item in _items)
        {
            // 持ってないなら
            if (!item.isPick)
            {
                isChange = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isChange = true;
        }

        if (_voiceTIme > 30.0f)
        {
            _voiceTIme = 0.0f;
            AudioManager.instance.playVoice(AudioName.VoiceName.V00);
        }

        // 持っていたらシーンを変える
        if (isChange)
        {
            _state = State.Wait;
            StartCoroutine(NoiseCancel());
        }
    }

    /// <summary>
    /// ID演出のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator Authentication()
    {
        //IDのキャンバスを表示
        _idCanvas.gameObject.SetActive(true);

        AudioManager.instance.playVoice(AudioName.VoiceName.V01a);
        AudioManager.instance.playSe(AudioName.SeName.Authentication);

        //アニメーションが終わるまで待つ
        while (_idCanvas.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        //Animationを待った後、UIの演出が終わるまで待つ
        yield return new WaitForSeconds(0.6f * 5 + 0.2f);

        AudioManager.instance.playVoice(AudioName.VoiceName.V01b);

        yield return new WaitForSeconds(2.0f);

        //消えるアニメーションに変更
        _idCanvas.GetComponentInChildren<Animator>().SetBool("End", true);
        //ちょっとだけ待つ
        yield return new WaitForSeconds(0.5f);
        //CanvasのAnimationが消えたら表示を消す
        _idCanvas.gameObject.SetActive(false);
        //チュートリアルに入るコルーチンを起動
        StartCoroutine(TurtrealProduction());
    }

    /// <summary>
    /// チュートリアルまでの演出
    /// </summary>
    /// <returns></returns>
    IEnumerator TurtrealProduction()
    {
        var time = 0.0f;
        var endTime = 2.0f;
        //銃のObjectを消す
        for (int i = 0; i < _gun.Length; i++)
        {
            Destroy(_gun[i]);
        }

        //Cameraをの切り替え
        _cameraRig[0].SetActive(false);
        _cameraRig[1].SetActive(true);

        while (time < 1.0f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        AudioManager.instance.playVoice(AudioName.VoiceName.V02);

        //Voiceが流れてる間、とめる
        while (time < 10.0f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        time = 0.0f;

        //弾を撃てるようにする
        foreach (var shot in FindObjectsOfType<PlayerShot>())
        {
            shot.isStart = true;
        }

        //ライトを少しずつ暗くしていく
        while (_spotLights[0].intensity != 0)
        {
            time += Time.unscaledDeltaTime;
            for (int i = 0; i < _spotLights.Length; i++)
            {
                _spotLights[i].intensity = Mathf.Lerp(_spotLights[i].intensity, 0, time / endTime);
                //_spotLights[i].intensity = (float)Easing.OutCirc(time, endTime, 0.0f, 1.0f);
            }
            yield return null;
        }
        //銃のスタンドを消す
        for (int i = 0; i < _gunStand.Length; i++)
        {
            Destroy(_gunStand[i]);
            Destroy(_desk[i]);
        }

        //Enemyくん起動
        //_enemyManager.SetTurtrealBulletActive(true);

        _viveControllerModel[0].SetActive(true);
        _viveControllerModel[1].SetActive(true);

        _viveControllerModel[0].transform.position = TURTREAL_POS[0];
        _viveControllerModel[1].transform.position = TURTREAL_POS[1];

        //コントローラーの向きを変える
        _viveControllerModel[0].transform.eulerAngles = Vector3.zero;
        _viveControllerModel[1].transform.eulerAngles = Vector3.zero;



        StartCoroutine(SlowDescription());
    }

    //IEnumerator WaitingState()
    //{
    //    var time = 0.0f;
    //    var endTime = 5.0f;
    //    AudioManager.instance.playVoice(AudioName.VoiceName.V03);
    //    while (time < endTime)
    //    {
    //        time += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    StartCoroutine(SlowDescription());
    //}

    /// <summary>
    /// スローのチュートリアル
    /// </summary>
    /// <returns></returns>
    IEnumerator SlowDescription()
    {
        _descriptionPanel.gameObject.SetActive(true);
        _descriptionText.text = "パッドを押し込んで\nスローを使ってみよう！";
        AudioManager.instance.playVoice(AudioName.VoiceName.V04); //Voice

        var time = 0.0f;
        var voiceTime = 0.0f;
        _viveMaterial[1].EnableKeyword("_EMISSION");
        _viveMaterial[1].SetColor("_EmissionColor", Color.black);
        _viveMaterial2[1].EnableKeyword("_EMISSION");
        _viveMaterial2[1].SetColor("_EmissionColor", Color.black);

        //スローを使うまでループ抜けない
        while (!SlowMotion._instance.isSlow)
        {
            time += Time.deltaTime;
            voiceTime += Time.unscaledDeltaTime;
            if (time > 1.0f)
            {
                _viveMaterial[1].EnableKeyword("_EMISSION");
                _viveMaterial[1].SetColor("_EmissionColor", Color.black);
                _viveMaterial2[1].EnableKeyword("_EMISSION");
                _viveMaterial2[1].SetColor("_EmissionColor", Color.black);

                time = 0.0f;
            }
            else if (time > 0.5f)
            {
                _viveMaterial[1].EnableKeyword("_EMISSION");
                _viveMaterial[1].SetColor("_EmissionColor", Color.white);
                _viveMaterial2[1].EnableKeyword("_EMISSION");
                _viveMaterial2[1].SetColor("_EmissionColor", Color.white);
            }

            if (voiceTime > 15.0f)
            {
                voiceTime = 0.0f;
                AudioManager.instance.playVoice(AudioName.VoiceName.V04b);
            }

            yield return null;
        }

        AudioManager.instance.stopVoice(AudioName.VoiceName.V04b);

        _viveMaterial[1].EnableKeyword("_EMISSION");
        _viveMaterial[1].SetColor("_EmissionColor", Color.black);
        _viveMaterial2[1].EnableKeyword("_EMISSION");
        _viveMaterial2[1].SetColor("_EmissionColor", Color.black);

        _descriptionText.text = "スロー中";

        //スローゲージがなくなったらループ抜ける
        while (SlowMotion._instance.isSlow)
        {
            yield return null;
        }
        AudioManager.instance.stopVoice(AudioName.VoiceName.V04);

        _descriptionText.text = "スロ―ゲージ回復中";

        AudioManager.instance.playVoice(AudioName.VoiceName.V05);

        //スローゲージが回復するまで待つ
        while (SlowMotion._instance.slowTime != SlowMotion._instance.slowTimeMax)
        {
            yield return null;
        }
        AudioManager.instance.stopVoice(AudioName.VoiceName.V05);

        yield return new WaitForSeconds(2.0f); //チャージ完了のセリフ待ってから進む

        //_descriptionText.text = "銃を縦にふって\nスローを回復しよう！";

        //var normalPos = _viveControllerModel[0].transform.position;
        //var normalPos2 = _viveControllerModel[1].transform.position;

        //var normalRotate = _viveControllerModel[0].transform.eulerAngles;

        //_arrowAnim.gameObject.SetActive(true);

        //iTween.MoveTo(_viveControllerModel[0], iTween.Hash("y", 0.30f, "time", 3.0f, "easeType", iTween.EaseType.easeOutCubic));
        //iTween.MoveTo(_viveControllerModel[1], iTween.Hash("y", 0.30f, "time", 3.0f, "easeType", iTween.EaseType.easeOutCubic));
        //iTween.RotateTo(_viveControllerModel[0], iTween.Hash("x", -75, "time", 2.0f, "easeType", iTween.EaseType.easeOutCirc));
        //iTween.RotateTo(_viveControllerModel[1], iTween.Hash("x", -75, "time", 2.0f, "easeType", iTween.EaseType.easeOutCirc));

        //AudioManager.instance.playNotSlowSe(AudioName.SeName.V05);
        //voiceTime = 0.0f;
        ////スローゲージが回復したらぬける
        //while (SlowMotion._instance.slowTime != SlowMotion._instance.slowTimeMax)
        //{
        //    voiceTime += Time.unscaledDeltaTime;
        //    if (voiceTime > 15.0f)
        //    {
        //        voiceTime = 0.0f;
        //        AudioManager.instance.playNotSlowSe(AudioName.SeName.V06);
        //    }

        //    _arrowAnim.gameObject.transform.Rotate(new Vector3(0, 75, 0) * Time.unscaledDeltaTime);
        //    if (_viveControllerModel[0].transform.position.y == 0.3f)
        //    {
        //        var pos = _viveControllerModel[0].transform.position;
        //        var pos2 = _viveControllerModel[1].transform.position;
        //        pos.y = normalPos.y;
        //        pos2.y = normalPos2.y;

        //        _viveControllerModel[0].transform.eulerAngles = normalRotate;
        //        _viveControllerModel[1].transform.eulerAngles = normalRotate;

        //        _viveControllerModel[0].transform.position = pos;
        //        _viveControllerModel[1].transform.position = pos2;

        //        //var animationHash = _arrowAnim.GetCurrentAnimatorStateInfo(0).shortNameHash;
        //        //_arrowAnim.Play(animationHash, 0, 0);

        //        iTween.MoveTo(_viveControllerModel[0], iTween.Hash("y", 0.30f, "time", 3.0f, "easeType", iTween.EaseType.easeOutCubic));
        //        iTween.MoveTo(_viveControllerModel[1], iTween.Hash("y", 0.30f, "time", 3.0f, "easeType", iTween.EaseType.easeOutCubic));
        //        iTween.RotateTo(_viveControllerModel[0], iTween.Hash("x", -75, "time", 2.0f, "easeType", iTween.EaseType.easeOutCirc));
        //        iTween.RotateTo(_viveControllerModel[1], iTween.Hash("x", -75, "time", 2.0f, "easeType", iTween.EaseType.easeOutCirc));

        //    }
        //    yield return null;
        //}
        //AudioManager.instance.stopNotSlowSe(AudioName.SeName.V05);
        //AudioManager.instance.stopNotSlowSe(AudioName.SeName.V06);
        //_arrowAnim.gameObject.SetActive(false);

        //iTween.Stop(_viveControllerModel[0], "move");
        //iTween.Stop(_viveControllerModel[1], "move");
        //iTween.Stop(_viveControllerModel[0], "rotate");
        //iTween.Stop(_viveControllerModel[1], "rotate");

        //yield return null;

        //_viveControllerModel[0].transform.position = normalPos;
        //_viveControllerModel[1].transform.position = normalPos2;

        //_viveControllerModel[0].transform.eulerAngles = normalRotate;
        //_viveControllerModel[1].transform.eulerAngles = normalRotate;


        StartCoroutine(TurtrealEnd());
    }

    /// <summary>
    /// Enemyを倒してチュートリアルを終える
    /// </summary>
    /// <returns></returns>
    IEnumerator TurtrealEnd()
    {
        _enemyManager.SetActive(true);

        //Enemyを殺させる
        TitleManager.isTurtreal = true;
        var enemyManager = FindObjectOfType<TurtrealEnemyManager>();
        _descriptionText.text = "ミサイルを打ち落とそう！";

        //光らせるマテリアルのEmissionを設定
        _viveMaterial[2].EnableKeyword("_EMISSION");
        _viveMaterial[2].SetColor("_EmissionColor", Color.black);
        _viveMaterial2[2].EnableKeyword("_EMISSION");
        _viveMaterial2[2].SetColor("_EmissionColor", Color.black);

        //光ってるか光ってないかの時間
        var time = 0.0f;

        //コントローラーの向きを変える
        _viveControllerModel[0].transform.Rotate(0, 90, 0);
        _viveControllerModel[1].transform.Rotate(0, -90, 0);

        AudioManager.instance.playVoice(AudioName.VoiceName.V07b);

        while (!enemyManager.isSceneChange)
        {
            time += Time.deltaTime;
            if (time > 1.0f)
            {
                _viveMaterial[2].EnableKeyword("_EMISSION");
                _viveMaterial[2].SetColor("_EmissionColor", Color.black);
                _viveMaterial2[2].EnableKeyword("_EMISSION");
                _viveMaterial2[2].SetColor("_EmissionColor", Color.black);

                time = 0.0f;
            }
            else if (time > 0.5f)
            {
                _viveMaterial[2].EnableKeyword("_EMISSION");
                _viveMaterial[2].SetColor("_EmissionColor", Color.white);
                _viveMaterial2[2].EnableKeyword("_EMISSION");
                _viveMaterial2[2].SetColor("_EmissionColor", Color.white);
            }

            yield return null;
        }
        AudioManager.instance.stopVoice(AudioName.VoiceName.V07b);
        AudioManager.instance.playVoice(AudioName.VoiceName.V07c);
        yield return new WaitForSeconds(2.0f);

        _descriptionPanel.gameObject.SetActive(false);

        _viveControllerModel[0].SetActive(false);
        _viveControllerModel[1].SetActive(false);

        time = 0.0f;
        var endTime = 9.0f;
        AudioManager.instance.playVoice(AudioName.VoiceName.V09);
        while (time < endTime)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        //シーン遷移
        TitleManager.isTurtreal = false;
        SceneChange.ChangeScene(SceneName.Name.MainGame, 1.0f, 1.0f, Color.white);

        //扉の演出
        //StartCoroutine(LightShine());
    }

    /// <summary>
    /// ライトをシーン遷移と同時に強くしていくコルーチン
    /// </summary>
    /// <returns></returns>
    //private IEnumerator LightShine()
    //{
    //    var time = 0.0f;
    //    var mat = _door.GetComponent<Renderer>().material;
    //    var color = new Color(1, 1, 1, 0);

    //    //光の強さとDoorのα値をを上げていく
    //    while (time < END_TIME)
    //    {
    //        time += Time.unscaledDeltaTime;
    //        _afterShade.intensity = Mathf.Lerp(_afterShade.intensity, 8, time / END_TIME);
    //        color.a = Mathf.Lerp(color.a, 1, time / END_TIME);
    //        mat.color = color;
    //        _door.transform.Translate(new Vector3(0, _moveSpeed, 0) * Time.unscaledDeltaTime);
    //        yield return null;
    //    }

    //    //StartCoroutine(DoorMove());
    //    ////シーン遷移
    //    //TitleManager.isTurtreal = false;
    //    //SceneChange.ChangeScene(SceneName.Name.MainGame, 1.0f, 1.0f, Color.white);

    //}

    /// <summary>
    /// ドアを動かし続ける
    /// </summary>
    /// <returns></returns>
    //private IEnumerator DoorMove()
    //{
    //    while (true)
    //    {
    //        _door.transform.Translate(new Vector3(0, _moveSpeed, 0) * Time.unscaledDeltaTime);
    //        yield return null;
    //    }
    //}
}
