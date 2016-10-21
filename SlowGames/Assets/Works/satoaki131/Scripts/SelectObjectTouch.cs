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

    void OnCollisionEnter(Collision col)
    {
        _renderer.material.color = Color.red;
    }

    void OnCollisionExit(Collision col)
    {
        _renderer.material.color = Color.white;
    }
}
