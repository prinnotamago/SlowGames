using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtrealEnemyManager : MonoBehaviour {

    [SerializeField]
    private TutorialEnemy[] _enemy = null;

    /// <summary>
    /// Enemyが弾を発射するかどうかの管理
    /// </summary>
    /// <param name="value"></param>
    public void SetTurtrealBulletActive(bool value)
    {
        foreach (var enemys in _enemy)
        {
            enemys.isShot = value;
        }
    }

    /// <summary>
    /// EnemyクンのSetActiveを一括で切り替える関数
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        foreach(var enemys in _enemy)
        {
            enemys.gameObject.SetActive(value);
        }
    }

}
