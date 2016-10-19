using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{

    [SerializeField]
    float _bulletSpeed = 100.0f;

    public void FixedUpdate()
    {
        ShotTest();
    }


    public void ShotTest()
    {
        gameObject.transform.Translate(0, -1 * _bulletSpeed, 1 * _bulletSpeed);
    }

}
