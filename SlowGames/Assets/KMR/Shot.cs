using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{

    [SerializeField]
    float bullet_speed = 100.0f;

    Vector3 a;

    void Start()
    {
        
         
        //a = gameObject.transform.position;

    }

    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        ShotTest();
    }


    public void ShotTest()
    {
        gameObject.transform.Translate(0, -1 * bullet_speed, 1 * bullet_speed);
    }

}
