using UnityEngine;
using System.Collections;

public class SelectObjectTouch : MonoBehaviour {

    private Renderer _renderer = null;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("OK");
        _renderer.material.color = Color.red;
    }

    void OnTriggerExit(Collider col)
    {
        _renderer.material.color = Color.white;
    }
}
