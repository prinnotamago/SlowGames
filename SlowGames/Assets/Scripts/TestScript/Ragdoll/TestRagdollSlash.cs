using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRagdollSlash : MonoBehaviour {

    [SerializeField]
    RagdollGenerator _generator;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == TagName.Sword)
        {
            if (!col.GetComponent<SlashSword>().IsAttack) { return; }

            var obj = GetComponentInChildren<Rigidbody>();

            var length = (obj.transform.position - col.transform.position).normalized;
            obj.velocity = (length + (Vector3.up * 1.0f)) * 10.0f;

            _generator.Generate(transform, GetComponent<Rigidbody>().velocity);
            Destroy(gameObject);
        }
    }
}
