using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    // とりあえずゲージを表示させる(テスト)
    [SerializeField]
    Image _image = null;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        _image.rectTransform.sizeDelta = new Vector2(_slowGage / _slowGageMax, 0.1f);

        if (SlowMotion._instance.isSlow)
        {
            if(_slowGage > 0.0f)
            {
                _slowGage -= Time.unscaledDeltaTime;
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
                _slowGage += Time.unscaledDeltaTime; ;
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
