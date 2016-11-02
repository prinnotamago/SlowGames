using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _deathEffect;

    //Transform _targetPostion;


    public TargetPosition  _generatePostion;

    //たまにあたったら死にます
    void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.tag == TagName.Bullet)
        {
            //エフェクト
            var effect = Instantiate(_deathEffect);
            effect.transform.position = transform.position;
            //音
            //AudioManager.instance.play3DSe(effect,AudioName.SeName.Thunder);

            //死ぬ
            FindObjectOfType<GenerateManager>().AddDeathCount(_generatePostion);
            ScoreManager.instance.AddHitEnemyCount();
            Destroy(this.gameObject);
        }
    }


    //移動速度,移動時間,
    [SerializeField,Range(0,100)]
    public float _moveSpeed   = 3.0f;

    //待機時間
    [SerializeField,Range(0,5)]
    public float _stayTimeMax = 1.0f;

    //横移動の幅
    [SerializeField,Range(0,10)]
    public float _sideMoveRange = 8.0f;

    //行動中のカウントをする用
    public float _activeCounter = 0;
    [SerializeField,Range(0,5)]
    public float _activeTimeMax = 2;

    //何回にどのくらい撃つかの頻度.
    public int _shotFrequency = 3;

    //何発連続で撃つか
    public int _chamberValue = 1;
 
    //連続で撃つ時の遅延時間
    [SerializeField]
    public float _shotDelay = 1.0f;

}
