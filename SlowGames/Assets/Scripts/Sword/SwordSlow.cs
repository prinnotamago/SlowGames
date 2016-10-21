using UnityEngine;
using System.Collections;

public class SwordSlow : MonoBehaviour {

    // スローにする時間のゲージ
    [SerializeField]
    float _slowGage;

    // スローにする時間の最大値
    [SerializeField]
    float _slowGageMax;

    // 敵を吹っ飛ばすスクリプト
    [SerializeField]
    Enemysbreaker _breaker;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (SlowMotion._instance.isSlow)
        {
            if(_slowGage > 0.0f)
            {
                _slowGage -= Time.deltaTime;
            }
            else
            {
                _slowGage = 0.0f;
                SlowMotion._instance.ResetSpeed();
            }
        }
        else
        {
            if (_slowGage < _slowGageMax)
            {
                _slowGage += Time.deltaTime;
            }
            else
            {
                _slowGage = _slowGageMax;
            }
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == TagName.Floor)
        {
            SlowMotion._instance.ResetSpeed();
            Debug.Log("通常");
            _breaker.Breaker();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == TagName.Floor)
        {
            SlowMotion._instance.GameSpeed(0.1f);
            Debug.Log("スロー");
        }
    }
}
