using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{

    [SerializeField]
    float _bulletSpeed = 100.0f;

    [SerializeField]
    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        ShotTest();
    }


    public void ShotTest()
    {

        //gameObject.transform.Translate(0, -1 * _bulletSpeed * Time.unscaledDeltaTime, 1 * _bulletSpeed * Time.unscaledDeltaTime);
        _rigidbody.velocity = new Vector3(0, -1 * _bulletSpeed, 1 * _bulletSpeed) * Time.unscaledDeltaTime * SlowMotion._instance.RealSpeed();
    }

    public void OnCollisionEnter(Collision col) //子供のあたり判定のときも呼んでくれる
    {
        if (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Bullet" || col.gameObject.tag == "Player") return;
        Debug.Log(col.gameObject.name);
        Destroy(gameObject);
    }
}
