using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    [SerializeField]
    float _bulletSpeed = 10;

    public Vector3 _targetDirection;
	
	// Update is called once per frame
	void Update ()
    {   
        //進行
        transform.position += _targetDirection * _bulletSpeed * Time.deltaTime;
	    //transform.Translate(_targetDirection * _bulletSpeed * Time.deltaTime,Space.World);
            
	}
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}

