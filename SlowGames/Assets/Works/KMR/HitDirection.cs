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
        //var enemyDirection = (player - enemy).normalized;
        ////Vector3 a = player;
        //var c = new Vector2(enemyDirection.x, enemyDirection.z);
        //var b = a.transform.TransformDirection(Vector3.forward);
        //var d = new Vector2(b.x, b.z);
        //float angle = Vector2.Angle(c, d);
        ////Debug.Log(angle);
        ////Quaternion s = Quaternion.Euler(Vector3.down);

        //gameObject.transform.eulerAngles = Vector3.up * angle + Vector3.up * 90.0f;
        //Debug.Log(gameObject.transform.eulerAngles);
        transform.LookAt(enemy);

    }

}
