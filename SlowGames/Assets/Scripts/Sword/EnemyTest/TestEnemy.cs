using UnityEngine;
using System.Collections;

public class TestEnemy : MonoBehaviour {

    // このオブジェクトのメッシュ
    MeshRenderer _mesh;

    // 攻撃を受けたかどうか
    bool _isHit = false;
    public bool isHit { get { return _isHit; } }

    NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start () {
        _mesh = GetComponent<MeshRenderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update () {
        if (_isHit)
        {
            _mesh.material.color = Color.red;
        }
        else
        {
            _mesh.material.color = new Color(255, 132, 0, 255);
        }

        _navMeshAgent.SetDestination(Vector3.forward * 22);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == TagName.Float)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.tag == TagName.Sword)
        {
            _isHit = true;
        }
    }

    public void Reset()
    {
        _isHit = false;
    }
}
