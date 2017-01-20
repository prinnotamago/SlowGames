using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestA : MonoBehaviour
{

    [SerializeField]
    GameObject a;

    void Start()
    {

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var b = Instantiate(a);
        }
    }
}
