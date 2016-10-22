using UnityEngine;
using System.Collections;

public class GunSlow : MonoBehaviour {

    // 両手が前にあるか確認するのに使う
    [SerializeField]
    ViveControllerManager _viveCon;

    // 弾の残段を見るのに使う
    [SerializeField]
    PlayerShot _right;
    [SerializeField]
    PlayerShot _left;

    // スローモーション
    [SerializeField]
    SlowMotion _slowMot;

    [SerializeField]
    float _speed = 0.1f;

    [SerializeField]
    Reload _reloadRight;
    [SerializeField]
    Reload _reloadLeft;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (_right.bulletsNumber > 0 && _left.bulletsNumber > 0
            && !_reloadRight.isReload && !_reloadLeft.isReload)
        {
            if (_viveCon.canSlowMode)
            {
                if (!_slowMot.isSlow)
                {
                    _slowMot.GameSpeed(_speed);
                    Debug.Log("スロースタート");
                }
                Debug.Log("スロー中");
            }
            else
            {
                if (_slowMot.isSlow)
                {
                    _slowMot.ResetSpeed();
                    Debug.Log("スロー終了");
                }
            }
        }
        else
        {
            if (_slowMot.isSlow)
            {
                _slowMot.ResetSpeed();
                Debug.Log("スロー終了");
            }
            Debug.Log("通常");
        }
    }
}
