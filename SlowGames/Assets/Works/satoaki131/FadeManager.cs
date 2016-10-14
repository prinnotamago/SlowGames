using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{

    static GameObject _instance = null;

    /// <summary>
    /// Fade中ならtrue
    /// </summary>
    public bool isFade
    {
        get; private set;
    }

    /// <summary>
    /// フェイドアウトの状態(画面真っ黒)ならtrue
    /// </summary>
    public bool isFadeOut
    {
        get; private set;
    }

    /// <summary>
    /// フェイドインした状態(画面がゲーム画面)ならtrue
    /// </summary>
    public bool isFadeIn
    {
        get; private set;
    }


    void Awake()
    {
        if (_instance == null)
        {
            Fader.fadeObject = GetComponentInChildren<Image>();
            Fader.fadeColor = new Color(0.0f, 0.0f, 0.0f, Fader.alpha);

            DontDestroyOnLoad(gameObject);
            _instance = gameObject;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// FadeOutの処理
    /// </summary>
    /// <param name="time">Fade時間</param>
    /// <param name="name">遷移先のシーン名</param>
    /// <param name="sceneChange">シーンを切り替えるかどうか</param>
    /// <param name="fadeInTime">シーンを切り替えた際のFadeInのFade時間</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time, SceneName name, bool sceneChange = true, float fadeInTime = 1.0f)
    {
        isFade = true;
        isFadeIn = false;

        GetComponent<Canvas>().sortingOrder = 10;
        yield return null;
        var t = 0.0f;
        while (Fader.alpha < 1.0f)
        {
            t += Time.deltaTime;
            Fader.setAlpha(t / time);
            yield return null;
        }

        Fader.FadeAdjustment(1.0f);

        isFadeOut = true;
        isFade = false;

        if (!sceneChange) yield break;
        SceneChange(name);
        StartCoroutine(FadeIn(fadeInTime));
    }

    /// <summary>
    /// Fadeの色を変えるタイプ
    /// </summary>
    /// <param name="time">Fade時間</param>
    /// <param name="name">遷移先のシーン名</param>
    /// <param name="r">FadeのR値</param>
    /// <param name="g">FadeのG値</param>
    /// <param name="b">FadeのB値</param>
    /// <param name="sceneChange">シーンを切り替えるかどうか</param>
    /// <param name="fadeInTime">シーンを切り替えた際のFadeInのFade時間</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time, SceneName name, float r, float g, float b, bool sceneChange = true, float fadeInTime = 1.0f)
    {
        isFade = true;
        isFadeIn = false;

        GetComponent<Canvas>().sortingOrder = 10;
        yield return null;
        var t = 0.0f;
        while (Fader.alpha < 1.0f)
        {
            t += Time.deltaTime;
            Fader.setAlpha(t / time, r, g, b);
            yield return null;
        }

        Fader.FadeAdjustment(1.0f);

        isFadeOut = true;
        isFade = false;

        if (!sceneChange) yield break;
        SceneChange(name);
        StartCoroutine(FadeIn(fadeInTime, r, g, b));
    }


    /// <summary>
    /// FadeInの処理
    /// </summary>
    /// <param name="time">Fade時間</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time)
    {
        isFade = true;
        isFadeOut = false;

        var t = time;
        while (Fader.alpha > 0.0f)
        {
            t -= Time.deltaTime;
            Fader.setAlpha(t / time);
            yield return null;
        }
        Fader.FadeAdjustment(0.0f);
        GetComponent<Canvas>().sortingOrder = -10;
        yield return null;

        isFade = false;
        isFadeIn = true;
    }

    public IEnumerator FadeIn(float time, float r, float g, float b)
    {
        isFade = true;
        isFadeOut = false;

        var t = time;
        while (Fader.alpha > 0.0f)
        {
            t -= Time.deltaTime;
            Fader.setAlpha(t / time, r, g, b);
            yield return null;
        }
        Fader.FadeAdjustment(0.0f);
        GetComponent<Canvas>().sortingOrder = -10;
        yield return null;

        isFade = false;
        isFadeIn = true;
    }


    public static void SceneChange(SceneName name)
    {
        SceneManager.LoadScene(name.ToString());
    }
}
