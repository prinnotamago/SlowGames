using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCreateTest : MonoBehaviour {

    [SerializeField]
    GameObject _createObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var obj = Instantiate(_createObj);
            obj.transform.position = transform.position;
        }
	}
}
