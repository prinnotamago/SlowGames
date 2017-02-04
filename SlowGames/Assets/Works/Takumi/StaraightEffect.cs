using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaraightEffect : MonoBehaviour {

	[SerializeField]
    float _moveSpeed = 100.0f;
    [SerializeField]
    float _deathTime = 3;

    void Start()
    {
        Destroy(gameObject,_deathTime);
        StartCoroutine(LeaveParent());
    }

	// Update is called once per frame


    IEnumerator LeaveParent()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.parent = null;
        yield return null;
    }
    
	void Update ()
    {
		    transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * _moveSpeed;
	}
}
