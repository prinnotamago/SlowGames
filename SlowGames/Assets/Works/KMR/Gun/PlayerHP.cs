using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour {

    [SerializeField]
    int HP = 10;

    int _playerHp;

    public int PlayerHp
    {
        get { return _playerHp; }
    }


    void Start ()
    {
        _playerHp = HP;

    }
	

	void Update ()
    {
		
	}

   public void Damage()
    {
        _playerHp--;
    }
}
