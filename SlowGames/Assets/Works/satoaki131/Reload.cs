using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {

    [SerializeField]
    private float _reloadTime = 0.5f;

    /// <summary>
    /// trueならリロード中
    /// falseならリロードしてない状態
    /// </summary>
    public bool isReload
    {
        get; private set;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) //あとでリロードボタンに変更
        {
            StartCoroutine(ShotReload());
        }
    }

    /// <summary>
    /// リロード中の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotReload()
    {
        var time = 0.0f;
        isReload = true;
        //Audioを追加する(カシャッ)
        while(time < _reloadTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isReload = false;
        //音を追加する(カチッ)
    }
}
