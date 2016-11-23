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

    public Image gameOverImage
    {
        get { return _gameOverImage; }
    }

    void Start ()
    {
        _playerHp = HP;
        _gameOverImage.gameObject.SetActive(false);
    }

   
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _playerHp = 0;
        }
      
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
    }
}
