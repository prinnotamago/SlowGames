using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneChange : MonoBehaviour {

    /// <summary>
    /// シーンチェンジに必要なアイテム
    /// </summary>
    [SerializeField]
    ViveGrab[] _items;

    /// <summary>
    /// フェードアウトの時間
    /// </summary>
    [SerializeField]
    float _fadeOutTime = 1.0f;

    /// <summary>
    /// 次のシーンのフェードインの時間
    /// </summary>
    [SerializeField]
    float _fadeInTime = 1.0f;

    /// <summary>
    /// シーンチェンジのときの色
    /// </summary>
    [SerializeField]
    Color _changeColor = Color.white;

	// Use this for initialization
	//void Start () {
    //    Debug.Log(_fadeInTime);
    //}
	
	// Update is called once per frame
	void Update() {
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
            SceneChange.ChangeScene(SceneName.Name.MainGame, _fadeOutTime, _fadeInTime, _changeColor);
        }
	}
}
