using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRagdollSlash : MonoBehaviour {

    [SerializeField]
    RagdollGenerator _generator;

    [SerializeField]
    GameObject _children;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = _children.transform.position;
        //transform.localRotation = _children.transform.localRotation;

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == TagName.Sword)
        {
            if (!col.GetComponent<SlashSword>().IsAttack) { return; }

            //var obj = GetComponentInChildren<Rigidbody>();

            //var length = (transform.position - col.transform.position).normalized;
            //GetComponent<Rigidbody>().velocity = (length + (Vector3.up * 1.0f)) * 10.0f;

            _generator.Generate(_children.transform, _children.GetComponent<Rigidbody>().velocity);
            Destroy(gameObject);
        }
    }
}
