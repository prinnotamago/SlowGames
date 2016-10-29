using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEnemy : MonoBehaviour {

    [SerializeField]
    SlashSword.SlashPattern _pattern;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == TagName.Sword)
        {
            var pattern = col.GetComponent<SlashSword>().pattern;
            if(pattern == _pattern)
            {
                var length = (transform.position - col.transform.position).normalized;
                GetComponent<Rigidbody>().velocity = (length + (Vector3.up * 1.0f)) * 10.0f;
            }
        }
    }
}
