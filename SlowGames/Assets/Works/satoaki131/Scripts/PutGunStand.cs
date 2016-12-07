using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutGunStand : MonoBehaviour {

    [SerializeField]
    private GameObject _gunObj = null;

    [SerializeField]
    private Vector3 _putPos = Vector3.zero;

    private Quaternion _rotation;

    void Start()
    {
        _rotation = _gunObj.transform.rotation;
        _gunObj.transform.parent = transform; //自分の子供にする
        _gunObj.gameObject.transform.position = transform.position;
        _gunObj.gameObject.transform.rotation = transform.rotation;
        _gunObj.gameObject.transform.Rotate(235, 0, 180);

        Debug.Log(_rotation);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == TagName.Weapon)
        {
            _gunObj.transform.parent = null; //子供解除
            _gunObj.transform.position = _putPos;
            _gunObj.transform.rotation = _rotation;
        }
    }

}
