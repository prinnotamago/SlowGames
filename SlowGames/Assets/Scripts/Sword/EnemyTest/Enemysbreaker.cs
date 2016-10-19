using UnityEngine;
using System.Collections;

public class Enemysbreaker : MonoBehaviour {

    // プレイヤー
    [SerializeField]
    GameObject _player;

    // 敵のオブジェクトが入ったオブジェクト
    [SerializeField]
    GameObject _enemyObj;

    [SerializeField]
    float _power = 100.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Breaker();
        }
	}

    public void Breaker()
    {
        var enemys = _enemyObj.GetComponentsInChildren<TestEnemy>();
        
        foreach(var enemy in enemys)
        {
            if (enemy.isHit)
            {
                var rigidBody = enemy.gameObject.GetComponent<Rigidbody>();

                var vector = (enemy.transform.position - _player.transform.position).normalized;

                rigidBody.velocity = vector * _power;
            }
        }
    }
}
