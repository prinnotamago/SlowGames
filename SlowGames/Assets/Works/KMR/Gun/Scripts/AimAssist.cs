using UnityEngine;
using System.Collections;

public class AimAssist : MonoBehaviour
{

    RaycastHit _hit;

    bool _enemyHit;

    bool isHit;

    Vector3 _enemyDirection;

    [SerializeField]
    float radius = 1;

    public Vector3 enemyDirection
    {
        get { return _enemyDirection; }

    }

    public bool enemyHit
    {
        get { return _enemyHit; }
        set { _enemyHit = value; }
    }

    void Start()
    {
        _enemyHit = false;
    }

    void OnDrawGizmos()
    {

        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * _hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * (_hit.distance), radius);

        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * 100);
        }
    }

    //void Update()
    //{
    //    //OrientationCorrection();
    //}

    public void OrientationCorrection()
    {
        _enemyHit = false;
        int layerMask = 1 << (int)LayerName.Enemy | 1 << (int)LayerName.Boss; ;//LayerMask.GetMask(new string[] { "Enemy" ,});
        isHit = Physics.SphereCast(transform.position, radius, transform.forward, out _hit, 100, layerMask);


        if (isHit)
        {
            if (_hit.transform.tag == "Enemy" && _enemyHit == false || _hit.transform.tag == "EnemyBullet" && _enemyHit == false || _hit.transform.tag == TagName.Boss && _enemyHit == false)
            {
                _enemyHit = true;
                var distance = _hit.transform.gameObject.GetComponentInChildren<Renderer>().bounds.center - gameObject.transform.position;
                _enemyDirection = distance.normalized;

            }
            else
            {
                _enemyHit = false;
            }

        }
    }
}
