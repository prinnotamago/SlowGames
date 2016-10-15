using UnityEngine;
using System.Collections;

enum GunType
{
    Single,
    Double,
    Triple,

}

public class EnemyShot : MonoBehaviour
{
    [SerializeField]
    EnemyBullet _enemyBullet;
   
    [SerializeField]
    Vector3 _randomShotRange = new Vector3(2,2,2);

    [SerializeField]
    float _shotDelay = 3.0f;

    public void Shot()
    {
        
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        //playerの位置を
        float randomXPos = _randomShotRange.x - Random.Range(0.0f,(_randomShotRange.x * 2.0f));
        float randomYPos = _randomShotRange.y - Random.Range(0.0f,(_randomShotRange.y * 2.0f));
        float randomZPos = _randomShotRange.z - Random.Range(0.0f,(_randomShotRange.z * 2.0f));

        Debug.Log(randomXPos+ "  " + randomYPos + "  " +randomZPos);

        playerPos +=  new Vector3(randomXPos,randomYPos,randomZPos);

        //打つ方向の基準を設定
        Vector3 targetDirection = (playerPos - transform.position).normalized;
        //玉を生成
        GameObject bullet = Instantiate(_enemyBullet.gameObject);

        bullet.GetComponent<EnemyBullet>()._targetDirection = targetDirection;
        bullet.transform.position = transform.position;
        bullet.transform.LookAt(targetDirection);


    }

    IEnumerator ShotDelay()
    {
        float delayMax = _shotDelay;
        float count = _shotDelay;
        while (true)
        {
            count -= Time.deltaTime;

            if (count < 0)
            {
                Shot();
                count = delayMax;
            }

            yield return null;
        }

    }

    void Start()
    {
        StartCoroutine(ShotDelay());
    }

    // Update is called once per frame
	void Update ()
    {
	    
	}
}
