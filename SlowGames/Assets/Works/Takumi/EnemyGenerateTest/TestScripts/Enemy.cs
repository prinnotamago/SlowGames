using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _deathEffect;
    
    //たまにあたったら死にます
    void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.tag == "Bullet")
        {
            //エフェクト
            var effect = Instantiate(_deathEffect);
            effect.transform.position = transform.position;
            //音
            AudioManager.instance.play3DSe(effect,AudioName.SeName.Thunder);

            //死ぬ
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
//        SlowMotion.instance.GameSpeed(0.5f);

    }
}
