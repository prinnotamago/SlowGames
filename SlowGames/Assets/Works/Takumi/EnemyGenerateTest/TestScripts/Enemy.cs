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
            _deathEffect.transform.position = transform.position;

            Destroy(this.gameObject);
        }
    }
}
