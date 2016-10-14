using UnityEngine;
using System.Collections;

public class SlowMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// ゲーム速度を元に戻す。
    /// １回だけ呼ぶようにしてください。
    /// </summary>
    static void ResetSpeed()
    {
        Time.timeScale = 1.0f;
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
    static void GameSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
