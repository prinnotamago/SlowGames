using UnityEngine;
using System.Collections;


public class SceneChange : MonoBehaviour
{

    public static void ChangeScene(SceneName name, float time, Color color)
    {
        SteamVR_LoadLevel.Begin(name.ToString(), false, time, color.r, color.g, color.b, color.a);
    }


    public static void ChangeScene(SceneName name, float time)
    {
        ChangeScene(name, time, Color.black);
    }

    public static void ChangeScene(SceneName name, Color color)
    {
        ChangeScene(name, 1.0f, color);
    }

    public static void ChangeScene(SceneName name)
    {
        ChangeScene(name, 1.0f, Color.black);
    }
}
