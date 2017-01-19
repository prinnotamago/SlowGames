using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMissile : MonoBehaviour
{

    [SerializeField]
    float _bulletSpeed = 5;
    [SerializeField]
    float _rotateSpeed = 100;

    [SerializeField]
    float _resizeTime = 0.8f;

    [SerializeField]
    GameObject _deathEffect;

    [SerializeField]
    float _moveTimeMax;

    Vector3 _basePosition;
    Vector3 _targetPosition;
    bool _isBlow;

    void Start()
    {
       _basePosition = transform.position;
       StartCoroutine(AutoReloadMissile());

    }

    IEnumerator AutoReloadMissile()
    {
        while (true)
        {
            transform.localScale = Vector3.zero;
            transform.position = _basePosition;
            iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "time", _resizeTime));

            yield return new WaitForSeconds(_resizeTime);

            float timeCount = _moveTimeMax;
        
            while (timeCount > 0)
            {
                
                timeCount -= Time.deltaTime * 1.0f;
                //全身
                transform.position += transform.TransformDirection(Vector3.forward) * _bulletSpeed * Time.deltaTime;



                yield return null;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {

        //玉にあった時は弾かせて消す
        if(other.gameObject.tag == TagName.Bullet)
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
            StartCoroutine(RandomBlow()); 

            //プレイヤーの弾を消す
            Destroy(other.gameObject);
                return;
               
        }
      /*else if(other.gameObject.tag == TagName.Player)
        {
            Destroy(gameObject);
        }
        */
        //ScoreManager.instance.AddFlipEnemyBulletCount();
        //プレイヤーに当たった時は、そのまま消す
        //Destroy(gameObject);
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

        //消す
        Destroy(this.gameObject);

    }


}
