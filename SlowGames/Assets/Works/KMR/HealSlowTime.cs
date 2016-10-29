using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSlowTime : MonoBehaviour {

    [SerializeField]
    float _bulletHeal = 0.5f;

    [SerializeField]
    float _enemyHeal = 1.0f;

	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public void OnCollisionEnter(Collision col) //子供のあたり判定のときも呼んでくれる
    {
        if (col.gameObject.tag == TagName.Player) return;
        if (col.gameObject.tag == TagName.Weapon) return;
        if (SlowMotion._instance.isSlow) { return; }
        if(col.gameObject.tag == TagName.Enemy)
        {
            SlowMotion._instance.slowTime += _enemyHeal;
        }
        if(col.gameObject.tag == TagName.Bullet)
        {
            SlowMotion._instance.slowTime += _bulletHeal;
        }
    }
}
