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
    }

    void Start()
    {
        _enemyHit = false;
    }

    void OnDrawGizmos()
    {

        //var radius = transform.lossyScale.x * 0.5f;

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

    void Update()
    {
        //OrientationCorrection();
    }

    public void OrientationCorrection()
    {
        //var radius = transform.lossyScale.x * 0.5f;

        isHit = Physics.SphereCast(transform.position, radius, transform.forward * 10, out _hit);

        if (isHit)
        {
            if (_hit.transform.tag == "Enemy" || _hit.transform.tag == "EnemyBullet")
            {
                _enemyHit = true;
                var a = _hit.transform.position - gameObject.transform.position;
                _enemyDirection = a.normalized;
                //_enemyDirection.x = _enemyDirection.x * 360;
                //_enemyDirection.y = _enemyDirection.y * 360;
                //_enemyDirection.z = _enemyDirection.z * 360;
            }
            else
            {
                _enemyHit = false;
            }

        }
    }
}
