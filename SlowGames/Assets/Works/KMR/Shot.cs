using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{

    [SerializeField]
    float _bulletSpeed = 100.0f;

    Rigidbody _rigidbody;
    Vector3 _direction = Vector3.zero;

    AimAssist[] _aimAssist;

    public void Start(Vector3 bulletBirection)
    {
        //_aimAssist =  FindObjectsOfType<AimAssist>();
        //float b = 1000;
        //int count = -1;
        //for(int i = 0; i < _aimAssist.Length; i++)
        //{
        //    float a = (_aimAssist[i].transform.position - gameObject.transform.position).magnitude;
        //    if(b >= a)
        //    {
        //        b = a;
        //        count++;
        //    }
        //}

        //if (_aimAssist[count].enemyHit == false)
        //{
        //    _direction = transform.forward - transform.up;
        //}else
        //if (_aimAssist[count].enemyHit == true)
        //{
        //    _direction = _aimAssist[count].enemyDirection;
        //}

        //_direction = transform.forward - transform.up;
        _direction = bulletBirection;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        ShotTest();
    }


    public void ShotTest()
    {
        //gameObject.transform.Translate(0, -1 * _bulletSpeed * Time.unscaledDeltaTime, 1 * _bulletSpeed * Time.unscaledDeltaTime);
        _rigidbody.velocity = _direction * Time.unscaledDeltaTime * SlowMotion._instance.RealSpeed() * _bulletSpeed;
    }

    public void OnCollisionEnter(Collision col) //子供のあたり判定のときも呼んでくれる
    {
        if (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Bullet" || col.gameObject.tag == "Player") return;
        Debug.Log(col.gameObject.name);
        Destroy(gameObject);
    }
}
