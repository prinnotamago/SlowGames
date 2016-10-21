using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject _deathEffect;

    Transform _targetPostion;

    public GeneratePosition _generatePostion;

    //たまにあたったら死にます
    void OnCollisionEnter(Collision other)
    {
       
        if (other.gameObject.tag == "Bullet")
        {
            //エフェクト
            var effect = Instantiate(_deathEffect);
            effect.transform.position = transform.position;
            //音
            //AudioManager.instance.play3DSe(effect,AudioName.SeName.Thunder);

            //死ぬ
            FindObjectOfType<GenerateManager>().AddDeathCount(_generatePostion);
            Destroy(this.gameObject);
        }
    }


}
