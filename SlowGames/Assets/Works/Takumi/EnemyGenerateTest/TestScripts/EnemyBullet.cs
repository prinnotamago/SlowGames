using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    [SerializeField]
    float _bulletSpeed = 10;
    [SerializeField]
    GameObject _deathEffect;

    public Vector3 _targetDirection;
    bool _isBlow  = false;

	// Update is called once per frame
	void Update()
    {   
        if (_isBlow)
        {
            return;
        }
        //進行
        transform.position += _targetDirection * _bulletSpeed * Time.deltaTime;
        //transform.Translate(_targetDirection * _bulletSpeed * Time.deltaTime,Space.World);


	}
    
    void OnTriggerEnter(Collider other)
    {
        //敵キャラ自信にあたってもスルー Todo : 見栄えが悪かったら調整
        if (other.gameObject.tag == TagName.Enemy)
        {
            return;
        }
        
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bullet")
        {
            if (other.gameObject.tag == "Bullet")
            {
                //エフェクト
                var effect = Instantiate(_deathEffect);
                effect.transform.position = transform.position;
                //音
                //
                //AudioManager.instance.play3DSe(effect, AudioName.SeName.Thunder);

                //判定消す
                _isBlow = true;
                this.GetComponent<Collider>().enabled = false;
            }

            StartCoroutine(RandomBlow());

            return;
        }

        Destroy(gameObject);
    }

    //ランダムに弾けます
    IEnumerator RandomBlow()
    {
        Vector3 randomDirec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        float deathTime   = 1.0f;
        float acceraition = 2.0f;
        float unacceration = 3.0f;
        float firstTime = 300.0f;

        //初速
        transform.position += randomDirec * (_bulletSpeed + firstTime) * Time.deltaTime;

        //加速
        while (true)
        {   

            transform.position += randomDirec * _bulletSpeed * acceraition * Time.deltaTime;
            acceraition -= Time.deltaTime * unacceration;
            deathTime -= Time.deltaTime;

            if (acceraition < 0)
            {
                break;
            }

            yield return null;
        }

        //Todo :消す時にパッと消えるのいくないかも
        //消す
        Destroy(this.gameObject);


    }


}

