using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtrealEnemyManager : MonoBehaviour {

    [SerializeField]
    private TutorialMissile[] _enemy = null;
    [SerializeField]
    private GameObject _enemyPrefab = null;

    private Vector3[] _enemyPos = null;

    public bool isSceneChange
    {
        get; private set;
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
    
    void Start()
    {
        _enemyPos = new Vector3[_enemy.Length];
        for(int i = 0; i < _enemy.Length; i++)
        {
            _enemyPos[i] = _enemy[i].transform.position;
        }
        isSceneChange = false;
    }

    void Update()
    {
        if (_enemy[0] == null && _enemy[1] == null && TitleManager.isTurtreal) { isSceneChange = true; }
        if (TitleManager.isTurtreal) return;
        if (isSceneChange) return;
        for(int i = 0; i < _enemy.Length; i++)
        {
            if (_enemy[i] != null) continue;
            var enemy = Instantiate(_enemyPrefab);
            _enemy[i] = enemy.GetComponent<TutorialMissile>();
            _enemy[i].transform.position = _enemyPos[i];
        }
    }

}
