using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDirection : MonoBehaviour
{

    GameObject a;

    PlayerHP _playerHP;

    void Start()
    {
        a = GameObject.FindGameObjectWithTag(TagName.Player);
        _playerHP = a.GetComponent<PlayerHP>();
        DirectionMove(a.transform.position,_playerHP.EnemyPos);
    }


    void Update()
    {

    }

    public void DirectionMove(Vector3 player,Vector3 enemy)
    {
        var enemyDirection = (enemy - player).normalized;
        float angle = Vector3.Angle(player, enemyDirection);
        gameObject.transform.Rotate(0,angle,0);
    }

}
