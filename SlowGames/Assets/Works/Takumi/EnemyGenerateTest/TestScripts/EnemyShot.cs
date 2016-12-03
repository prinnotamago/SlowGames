using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

enum GunType
{
    Single,
    Double,
    Tutorial,

}

//弾を撃ちます
public class EnemyShot : MonoBehaviour
{
    [SerializeField]
    EnemyBullet _enemyBullet;
    [SerializeField]
    EnemyBullet _doubleBullet;
   
    [SerializeField]
    Vector3 _randomShotRange = new Vector3(2,2,2);

    [SerializeField]
    float _shotDelay = 3.0f;

    [SerializeField]
    GunType _gunType;

    [SerializeField]
    GameObject _muzzleFlush;

    public bool _isShotStart;


    Dictionary<GunType,EnemyBullet> _bulletTypeList = new Dictionary<GunType, EnemyBullet>();
    Dictionary<GunType,Action> _shotTypeList = new Dictionary<GunType,Action>();

    void Start()
    {
        _isShotStart = false;
        //StartCoroutine(ShotDelay());

        _bulletTypeList.Add(GunType.Single,_enemyBullet);
        _bulletTypeList.Add(GunType.Double,_doubleBullet);

        _shotTypeList.Add(GunType.Single,Shot);
        _shotTypeList.Add(GunType.Double,DoubleShot);
        _shotTypeList.Add(GunType.Tutorial,TutorialShot);

    }


    // Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DoubleShot();
        }
	}

    public void Shot()
    {
        
        Vector3 playerPos = GameObject.FindGameObjectWithTag(TagName.Player).transform.position;

        //playerの位置を意志的にずらす
        float randomXPos = _randomShotRange.x - _randomShotRange.x;
        float randomYPos = _randomShotRange.y - _randomShotRange.y;
        float randomZPos = _randomShotRange.z - _randomShotRange.z;

        playerPos +=  new Vector3(randomXPos,randomYPos,randomZPos);

        //打つ方向の基準を設定
        Vector3 targetDirection = (playerPos - transform.position).normalized;

        //玉を生成
        GameObject bullet = Instantiate(_enemyBullet.gameObject);
        bullet.GetComponent<EnemyBullet>()._targetDirection = targetDirection;
        bullet.transform.position = transform.position;

        //マズルフラッシュエフェクト
        var flashEffect = Instantiate(_muzzleFlush);
        flashEffect.transform.position = transform.position;


        //bullet.transform.LookAt(targetDirection);
        //向きを調整
        bullet.transform.LookAt(playerPos);
        flashEffect.transform.LookAt(playerPos);
    }

    public void DoubleShot()
    {

        for (int i = 0; i < 2; i++)
        {
            
            //PLAYERの位置を取得
            Vector3 playerPos = GameObject.FindGameObjectWithTag(TagName.Player).transform.position;
            
            //打つ方向の基準を設定
            Vector3 targetDirection = (playerPos - transform.position).normalized;
            //玉を生成
            //GameObject bullet = Instantiate(_enemyBullet);
            var bullet = Instantiate(_enemyBullet);

            bullet._targetDirection = targetDirection;
            bullet.transform.position = transform.position;

            if (i % 2 == 0)
            {
                bullet.transform.LookAt(-transform.forward + transform.position);
            }
            else
            {
                bullet.transform.LookAt(transform.forward + transform.position);
            }
           // bullet.transform.Rotate(bullet.transform.up,90);

        
        }

    }


    //指定方向にうつ
    void TutorialShot()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag(TagName.Player).transform.position;

        //playerの位置を意志的にずらす
        float randomXPos = _randomShotRange.x - _randomShotRange.x;
        float randomYPos = _randomShotRange.y - _randomShotRange.y;
        float randomZPos = _randomShotRange.z - _randomShotRange.z;

        int pluss = UnityEngine.Random.Range(0,2) == 1 ? -1 : 1;

        //設定したプラスか
        playerPos +=  new Vector3(randomXPos * pluss,randomYPos,randomZPos);

        //打つ方向の基準を設定
        Vector3 targetDirection = (playerPos - transform.position).normalized;
        //玉を生成
        GameObject bullet = Instantiate(_enemyBullet.gameObject);

        bullet.GetComponent<EnemyBullet>()._targetDirection = targetDirection;
        bullet.transform.position = transform.position;

        bullet.transform.LookAt(playerPos);
    }

    float ToRadian(float value)
    {
        return value * 3.14f / 180.0f;
    }

    public void DoShot()
    {
        //typeにあわせたショットをする
        _shotTypeList[_gunType]();
        
    }


    //test:一定感覚で打ち続けます
    IEnumerator ShotDelay()
    {
        float delayMax = _shotDelay;
        float count = _shotDelay;


        while (true)
        {
            if (_isShotStart)
            {
                count -= Time.deltaTime;

                if (count < 0)
                {

                    //typeにあわせたショットをする
                    _shotTypeList[_gunType]();

                    count = delayMax;
                }
            }
            yield return null;
        }

    }

}
