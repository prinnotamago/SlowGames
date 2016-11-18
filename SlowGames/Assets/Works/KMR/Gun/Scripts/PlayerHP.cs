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

    [SerializeField]
    Image _gameOverImage;

    void Start ()
    {
        _playerHp = HP;
        _gameOverImage.enabled = false;
    }

   
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _playerHp = 0;
        }
      
        if (_playerHp <= 0)
        {
            _gameOverImage.enabled = true;
        }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagName.EnemyBullet)
        {
            Damage();
        }
    }
   public void Damage()
    {
        _playerHp--;
    }
}
