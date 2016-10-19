using UnityEngine;
using System.Collections;

public class TestEnemy : MonoBehaviour {

    // このオブジェクトのメッシュ
    MeshRenderer mesh;

    // 攻撃を受けたかどうか
    bool _isHit = false;
    public bool isHit { get { return _isHit; } }

	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_isHit)
        {
            mesh.material.color = Color.red;
        }
        else
        {
            mesh.material.color = new Color(255, 132, 0, 255);
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
