using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour {

    [SerializeField]
    int HP = 10;

    [SerializeField]
    Image _gameOverImage;

    [SerializeField]
    float _timeRecovery = 5.0f;

    [SerializeField]
    GameObject BarrierEffect;

    [SerializeField]
    RingEmission _ring;

    int _playerHp;

    public int PlayerHp
    {
        get { return _playerHp; }
    }

    bool _isHit;


    public Image gameOverImage
    {
        get { return _gameOverImage; }
    }

    Vector3 _enemyPos;

    public Vector3 EnemyPos
    {
        get { return _enemyPos; }
    }

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
            _enemyPos = other.transform.position;
           // Debug.Log(_enemyPos);
            BarrierEffectCreate(other.transform.position);
            Damage(1);
        }
    }
   public void Damage(int damageValue)
    {
        //ScoreManager.instance.AddInpactDamageCount();
        _playerHp -= damageValue;
        _isHit = true;

        if(_playerHp < HP / 2)
        {
            _ring.ColorMoveChange(_ring.GetComponent<Renderer>().material.color, Color.yellow);
        }
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

    public void BarrierEffectCreate(Vector3 enemyPosition)
    {
        var effect =  Instantiate(BarrierEffect);
        effect.transform.position = gameObject.transform.position;
    }

}
