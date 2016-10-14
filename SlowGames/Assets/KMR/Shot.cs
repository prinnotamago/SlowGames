using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{

    [SerializeField]
    float bullet_speed = 100.0f;

    void Start()
    {
        ShotTest();
    }

    void Update() { }

    public void ShotTest()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bullet_speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet") return;

        Destroy(gameObject);
    }
}
