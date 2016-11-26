using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour {

    [SerializeField]
    int HP = 10;

    int _playerHp;

    public int PlayerHp
    {
        get { return _playerHp; }
    }

    bool _isHit;

    [SerializeField]
    Image _gameOverImage;

    public Image gameOverImage
    {
        get { return _gameOverImage; }
    }

    [SerializeField]
    float _timeRecovery = 5.0f;

    float _time;

    void Start ()
    {
        _time = _timeRecovery;
        _isHit = false;
        _playerHp = HP;
        _gameOverImage.gameObject.SetActive(false);
    }

   
	void Update ()
    {
        //Debug.Log(_isHit);
        TimeOutRecovery();

        if (_isHit) { _isHit = false; }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _playerHp = 0;
        }
        
       // Debug.Log(_playerHp);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_playerHp <= 0) return;
        if (other.tag == TagName.EnemyBullet)
        {
            Damage();
        }
    }
   public void Damage()
    {
        ScoreManager.instance.AddInpactDamageCount();
        _playerHp--;
        _isHit = true;
    }

    void TimeOutRecovery()
    {
        if (_isHit || _playerHp == HP)
        {
            _time = _timeRecovery;
            return;
        }

        _time -= Time.unscaledDeltaTime;

        if(_time <= 0)
        {
            _playerHp++;
            if(_playerHp >= HP)
            {
                _playerHp = HP;
            }
        }

    }

}
