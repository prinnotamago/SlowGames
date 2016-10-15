using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    [SerializeField]
    float _bulletSpeed = 10;
    [SerializeField]
    GameObject _deathEffect;

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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bullet")
        {
            if (other.gameObject.tag == "Bullet")
            {
                //エフェクト
                var effect = Instantiate(_deathEffect);
                effect.transform.position = transform.position;
                //音
                AudioManager.instance.play3DSe(effect, AudioName.SeName.Thunder);
            }

            Destroy(this.gameObject);
           
        }
    }
}

