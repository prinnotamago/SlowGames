using UnityEngine;
using System.Collections;


public class SceneChange : MonoBehaviour
{

    public static void ChangeScene(SceneName name, float fadeOutTime, float fadeInTime, Color color)
    {
        SteamVR_LoadLevel.Begin(name.ToString(), false, fadeOutTime, fadeInTime, color.r, color.g, color.b, color.a);
    }

    /// <summary>
    /// シーンの遷移先とFadeと色の設定
    /// FadeOutとFadeInの時間が一緒でよいときに使用
    /// </summary>
    /// <param name="name">遷移先の名前</param>
    /// <param name="time">FadeOutとFadeInの時間</param>
    /// <param name="color">Fadeの色</param>
    public static void ChangeScene(SceneName name, float time, Color color)
    {
        ChangeScene(name, time, time, color);
    }

    /// <summary>
    /// シーン遷移先とFade時間の設定
    /// FadeInとFadeOutの時間が一緒でよいときに使用
    /// 色：黒
    /// </summary>
    /// <param name="name">遷移先の名前</param>
    /// <param name="time">FadeOutとFadeInの時間</param>
    public static void ChangeScene(SceneName name, float time)
    {
        ChangeScene(name, time, time, Color.black);
    }

    /// <summary>
    /// シーン先とFade時間の設定
    /// FadeInとFadeOutの時間を別にしたいときに使用
    /// 色：黒
    /// </summary>
    /// <param name="name">遷移先の名前</param>
    /// <param name="fadeOutTime">FadeOut時間</param>
    /// <param name="fadeInTime">FadeIn時間</param>
    public static void ChangeScene(SceneName name, float fadeOutTime, float fadeInTime)
    {
        ChangeScene(name, fadeOutTime, fadeInTime, Color.black);
    }

    /// <summary>
    /// シーン先と色の設定
    /// FadeTime：1.0f
    /// </summary>
    /// <param name="name">遷移先の名前</param>
    /// <param name="color">Fadeの色</param>
    public static void ChangeScene(SceneName name, Color color)
    {
        ChangeScene(name, 1.0f, 1.0f, color);
    }

    /// <summary>
    /// シーン先のみ設定
    /// fadeTime：1.0f
    /// Color：黒
    /// </summary>
    /// <param name="name">遷移先の名前</param>
    public static void ChangeScene(SceneName name)
    {
        ChangeScene(name, 1.0f, 1.0f, Color.black);
    }

}
