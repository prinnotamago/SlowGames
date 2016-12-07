using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{

    [SerializeField]
    private PutGunStand[] _put = null;

    void Start()
    {
        _put = GetComponentsInChildren<PutGunStand>();
    }

    void Update()
    {
        //デバッグ用:台座に置いてないときでも戻れるように
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneChange.ChangeScene(SceneName.Name.Title, Color.white);
        }

        if (!_put[0].isPutGun) return;
        if (!_put[1].isPutGun) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ChangeScene(SceneName.Name.Title, Color.white);
        }
    }

}
