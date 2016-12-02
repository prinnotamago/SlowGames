using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPurgeParts : MonoBehaviour {

    // パーツの体力
    //[SerializeField]
    //int _hp = 5;

    // パージしたかどうか
    bool _isPurge = false;
    public bool isPurge { get { return _isPurge; } }

    Rigidbody _rigidbody = null;
    Collider _collider = null;

	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    //void OnTriggerEnter(Collider col)
    //{
    //    // 弾が当たったら体力を減らす
    //    if(col.gameObject.tag == TagName.Bullet)
    //    {
    //        --_hp;
    //        if(_hp == 0)
    //        {
    //            _collider.isTrigger = false;
    //            _rigidbody.useGravity = true;
    //            var vector = transform.position - transform.parent.transform.position;
    //            //Debug.Log(vector);
    //            _rigidbody.velocity = vector.normalized * 25 + Vector3.down * 25;
    //            transform.parent = null;

    //            _isPurge = true;
    //        }
    //    }
    //}

    public void Purge()
    {
        _collider.isTrigger = false;
        _rigidbody.useGravity = true;
        var vector = transform.position - transform.parent.transform.position;
        //Debug.Log(vector);
        _rigidbody.velocity = vector.normalized * 25 + Vector3.down * 25;
        transform.parent = null;

        _isPurge = true;
    }
}
