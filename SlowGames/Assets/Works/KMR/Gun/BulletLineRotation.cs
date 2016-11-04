using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineRotation : MonoBehaviour
{

    bool a = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 0, 90);
        ScaleChange();

    }

    void ScaleChange()
    {

        {
            transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            if (transform.localScale.x <= 0.05f)
            {
                transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
        }

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        //    a = false;
        //}

    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    Destroy(gameObject);
    //}
    
}
