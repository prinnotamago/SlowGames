using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineRotation : MonoBehaviour
{

    bool a = false;

    [SerializeField]
    float _rotation = 30;

    [SerializeField]
    float _minScaleSize = 0.1f;

    [SerializeField]
    float _changeScaleSpeed = 0.02f;
    // Use this for initialization
    void Start()
    {

    }

    public void Update()
    {
        transform.Rotate(0, 0, _rotation);
    }

    // Update is called once per frame

    void ScaleChange()
    {

        {
            transform.localScale -= new Vector3(_changeScaleSpeed, _changeScaleSpeed, _changeScaleSpeed);
            if (transform.localScale.x <= _minScaleSize)
            {
                transform.localScale = new Vector3(_minScaleSize, _minScaleSize, _minScaleSize);
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
