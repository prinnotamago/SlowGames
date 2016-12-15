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

    [SerializeField]
    private GameObject _door = null; //ドア用のオブジェクト

    [SerializeField]
    private Light _afterShade = null; //後光用のライト

    [SerializeField, Range(1.0f, 3.0f)]
    private float END_TIME = 2.0f;

    [SerializeField]
    private float _moveSpeed = 1.0f;

    [SerializeField]
    private GameObject[] _viveControllerModel = null;
    private Material[] _viveMaterial = null; //0:body, 1:slowButton 2:trigger, 3:grip
    private Material[] _viveMaterial2 = null; //0:body, 1:slowButton 2:trigger, 3:grip

    [SerializeField]
    private Animator _arrowAnim = null;

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

        _viveControllerModel[0].SetActive(false);
        _viveControllerModel[1].SetActive(false);
        _arrowAnim.gameObject.SetActive(false);
    }

    void Update()
    {
        _stateUpdate[_state]();

        //デバック用
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneChange.ChangeScene(SceneName.Name.MainGame, Color.white);
        }
    }

    /// <summary>
    /// TitleのUpdate(銃を台座からとるまで)
    /// </summary>
    void TitleUpdate()
    {
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

        // 持っていたらシーンを変える
        if (isChange)
        {
            _state = State.Wait;
            StartCoroutine(Authentication());
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
        //アニメーションが終わるまで待つ
        while (_idCanvas.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        //Animationを待った後、UIの演出が終わるまで待つ
        yield return new WaitForSeconds(0.6f * 5 + 2.0f);
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
            }
            yield return null;
        }
        //銃のスタンドを消す
        for (int i = 0; i < _gunStand.Length; i++)
        {
            Destroy(_gunStand[i]);
        }

        //Enemyくん起動
        _enemyManager.SetActive(true);
        _enemyManager.SetTurtrealBulletActive(true);

        _viveControllerModel[0].SetActive(true);
        _viveControllerModel[1].SetActive(true);


        StartCoroutine(SlowDescription());
    }

    /// <summary>
    /// スローのチュートリアル
    /// </summary>
    /// <returns></returns>
    IEnumerator SlowDescription()
    {
        _descriptionPanel.gameObject.SetActive(true);
        _descriptionText.text = "スローを使ってみよう！";

        var time = 0.0f;

        _viveMaterial[1].EnableKeyword("_EMISSION");
        _viveMaterial[1].SetColor("_EmissionColor", Color.black);
        _viveMaterial2[1].EnableKeyword("_EMISSION");
        _viveMaterial2[1].SetColor("_EmissionColor", Color.black);

        //スローを使うまでループ抜けない
        while (!SlowMotion._instance.isSlow)
        {
            time += Time.deltaTime;
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
            yield return null;
        }

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
        _descriptionText.text = "銃を縦にふって\nスローを回復しよう！";

        time = 0.0f;
        var normalPos = _viveControllerModel[0].transform.position;
        var normalPos2 = _viveControllerModel[1].transform.position;
        _arrowAnim.gameObject.SetActive(true);

        //スローゲージが回復したらぬける
        while (SlowMotion._instance.slowTime != SlowMotion._instance.slowTimeMax)
        {
            _viveControllerModel[0].transform.Translate(new Vector3(0, -0.2f, 0) * Time.unscaledDeltaTime);
            _viveControllerModel[1].transform.Translate(new Vector3(0, -0.2f, 0) * Time.unscaledDeltaTime);
            if (_viveControllerModel[0].transform.position.y < 0.45f)
            {
                time += Time.unscaledDeltaTime;
                if (time > 1.0f)
                {
                    time = 0.0f;
                    var pos = _viveControllerModel[0].transform.position;
                    var pos2 = _viveControllerModel[1].transform.position;
                    pos.y = normalPos.y;
                    pos2.y = normalPos2.y;
                    _viveControllerModel[0].transform.position = pos;
                    _viveControllerModel[1].transform.position = pos2;

                    var animationHash = _arrowAnim.GetCurrentAnimatorStateInfo(0).shortNameHash;
                    _arrowAnim.Play(animationHash, 0, 0);

                }
            }
            yield return null;
        }
        _arrowAnim.gameObject.SetActive(false);

        _viveControllerModel[0].transform.position = normalPos;
        _viveControllerModel[1].transform.position = normalPos2;

        StartCoroutine(TurtrealEnd());
    }

    /// <summary>
    /// Enemyを倒してチュートリアルを終える
    /// </summary>
    /// <returns></returns>
    IEnumerator TurtrealEnd()
    {
        //Enemyを殺させる
        TitleManager.isTurtreal = true;
        var enemyManager = FindObjectOfType<TurtrealEnemyManager>();
        _descriptionText.text = "敵を倒そう！";

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
        _descriptionPanel.gameObject.SetActive(false);

        _viveControllerModel[0].SetActive(false);
        _viveControllerModel[1].SetActive(false);

        //扉の演出
        StartCoroutine(LightShine());
    }

    /// <summary>
    /// ライトをシーン遷移と同時に強くしていくコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator LightShine()
    {
        var time = 0.0f;
        var mat = _door.GetComponent<Renderer>().material;
        var color = new Color(1, 1, 1, 0);

        //光の強さとDoorのα値をを上げていく
        while (time < END_TIME)
        {
            time += Time.unscaledDeltaTime;
            _afterShade.intensity = Mathf.Lerp(_afterShade.intensity, 8, time / END_TIME);
            color.a = Mathf.Lerp(color.a, 1, time / END_TIME);
            mat.color = color;
            _door.transform.Translate(0, _moveSpeed, 0);
            yield return null;
        }

        StartCoroutine(DoorMove());
        //シーン遷移
        TitleManager.isTurtreal = false;
        SceneChange.ChangeScene(SceneName.Name.MainGame, 1.0f, 1.0f, Color.white);

    }

    /// <summary>
    /// ドアを動かし続ける
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoorMove()
    {
        while (true)
        {
            _door.transform.Translate(0, _moveSpeed, 0);
            yield return null;
        }
    }
}
