using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

enum GunType
{
    Single,
    Double,
    Triple,

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
        float randomXPos = _randomShotRange.x - UnityEngine.Random.Range(0.0f,(_randomShotRange.x * 2.0f));
        float randomYPos = _randomShotRange.y - UnityEngine.Random.Range(0.0f,(_randomShotRange.y * 2.0f));
        float randomZPos = _randomShotRange.z - UnityEngine.Random.Range(0.0f,(_randomShotRange.z * 2.0f));

        playerPos +=  new Vector3(randomXPos,randomYPos,randomZPos);

        //打つ方向の基準を設定
        Vector3 targetDirection = (playerPos - transform.position).normalized;
        //玉を生成
        GameObject bullet = Instantiate(_enemyBullet.gameObject);

        bullet.GetComponent<EnemyBullet>()._targetDirection = targetDirection;
        bullet.transform.position = transform.position;

        //bullet.transform.LookAt(targetDirection);
        bullet.transform.LookAt(playerPos);

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
            GameObject bullet = Instantiate(_enemyBullet.gameObject);


            bullet.GetComponent<EnemyBullet>()._targetDirection = targetDirection;
            bullet.transform.position = transform.position;

            //bullet.transform.LookAt(targetDirection);
            bullet.transform.LookAt(playerPos);

            //Vector3 cross = Vector3.Cross(targetDirection,transform.right);
            //bullet.transform.Rotate(cross,90);

        
        }

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
