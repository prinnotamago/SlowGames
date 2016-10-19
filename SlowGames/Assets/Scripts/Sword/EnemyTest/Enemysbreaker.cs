using UnityEngine;
using System.Collections;

public class Enemysbreaker : MonoBehaviour {

    // プレイヤー
    [SerializeField]
    GameObject _player;

    // 敵のオブジェクトが入ったオブジェクト
    [SerializeField]
    GameObject _enemyObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Breaker()
    {
        var enemys = _enemyObj.GetComponentsInChildren<TestEnemy>();
    }
}
