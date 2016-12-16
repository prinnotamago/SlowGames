using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutGunStand : MonoBehaviour {

    [SerializeField]
    private GameObject _gunObj = null;

    private bool _isPut = false;

    public bool isPutGun
    {
        get { return _isPut; }
    }

    void Start()
    {
        _gunObj.transform.parent = transform; //自分の子供にする
        _gunObj.gameObject.transform.position = transform.position;
        _gunObj.gameObject.transform.rotation = transform.rotation;
        _gunObj.gameObject.transform.Rotate(235, 0, 180);
    }

    void Update()
    {
        if(!_isPut)
        {
            _gunObj.transform.parent = transform; //自分の子供にする
            _gunObj.gameObject.transform.position = transform.position;
            _gunObj.gameObject.transform.rotation = transform.rotation;
            _gunObj.gameObject.transform.Rotate(235, 0, 180);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == TagName.Stand)
        {
            _gunObj.transform.parent = null; //子供解除
            var localPosition = col.transform.position;
            localPosition.y = 0.7f;
            _gunObj.transform.localPosition = localPosition;
            _gunObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<SphereCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            var localRotation = col.transform.eulerAngles;
            localRotation.z = col.transform.position.x < 0 ? 72.89301f : -89.953f;
            _gunObj.transform.eulerAngles = localRotation; 
            _isPut = true;
        }
    }

}
