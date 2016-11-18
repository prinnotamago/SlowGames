using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReseter : MonoBehaviour {

    //[SerializeField]
    Vector3 _initPos;
    Vector3 _initRot;
    Rigidbody _rigidbody;

	// Use this for initialization
	void Start () {
        _initPos = transform.position;
        _initRot = transform.eulerAngles;
        _rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col)
    {
        if(col.tag == TagName.UI)
        {
            //Debug.Log("とおった");
            transform.position = _initPos;
            transform.eulerAngles = _initRot;
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
