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
    private RingEmission.Emissivecolor _emissiveColor = RingEmission.Emissivecolor.blue;

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

        if (_ring.isChange) return;
        if(_emissiveColor != _ring.emissiveColor) //自分のenumとringのenumが一緒じゃなかったら通る
        {
            _ring.ColorMoveChange(_ring.color[_ring.emissiveColor], _ring.color[_emissiveColor]); //リングの色から指定された色に変わるコルーチンを呼ぶ
            _ring.setEmissiveColor(_emissiveColor); //リングが持つenumを更新
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

        if(_playerHp > HP / 2) //HP100のとき50より大きかったらなら
        {
            _emissiveColor = RingEmission.Emissivecolor.blue;
        }
        else if(_playerHp > HP / 10) //HP100のとき10より大きかったら
        {
            _emissiveColor = RingEmission.Emissivecolor.yellow;
        }
        else
        {
            _emissiveColor = RingEmission.Emissivecolor.red;
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
