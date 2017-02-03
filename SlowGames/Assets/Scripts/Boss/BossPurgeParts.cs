using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPurgeParts : MonoBehaviour
{

    // パーツの体力
    //[SerializeField]
    //int _hp = 5;

    // パージしたかどうか
    bool _isPurge = false;
    public bool isPurge { get { return _isPurge; } }

    [SerializeField]
    GameObject _sparkParticle;

    [SerializeField]
    GameObject _particlePos;

    [SerializeField]
    Vector3 _particleRotate;

    Rigidbody _rigidbody = null;
    Collider _collider = null;

    bool _hitFlag = true;

    [SerializeField]
    BossAI _ai;

    //static bool _oneHitFrame = true;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    _oneHitFrame = true;
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
        var particle = Instantiate(_sparkParticle);
        particle.transform.position = _particlePos.transform.position;
        //particle.transform.parent = transform.parent;
        particle.transform.eulerAngles = _particleRotate;

        _collider.isTrigger = false;
        _rigidbody.useGravity = true;
        var vector = transform.position - transform.parent.transform.position;
        //Debug.Log(vector);
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.velocity = vector.normalized * 25 + Vector3.down * 25;
        transform.parent = null;

        _isPurge = true;

        _hitFlag = false;
    }

    void OnTriggerEnter(Collider col)
    {
        //if (!_oneHitFrame) { return; }
        if (!_hitFlag) { return; }
        if (_ai == null) { return; }
        _ai.PargeDamage(col);
        //_oneHitFrame = false;
    }
}