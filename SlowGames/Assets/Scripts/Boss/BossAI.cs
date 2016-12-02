using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    [SerializeField]
    int _hp = 10;

    [SerializeField]
    int[] _changeStateHP;
    int _changeStateIndex = 0;

    [SerializeField]
    BossPurgeParts[] _parts;

    [SerializeField]
    int[] _purgeHP;
    int _purgeIndex = 0;

    enum BossState
    {
        START,
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LAST,
    }
    BossState _state = BossState.START;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        switch (_state)
        {
            case BossState.START:
                StartUpdate();
                break;
            case BossState.LEVEL_1:
                Level_1_Update();
                break;
            case BossState.LEVEL_2:
                Level_2_Update();
                break;
            case BossState.LEVEL_3:
                Level_3_Update();
                break;
            case BossState.LAST:
                LastUpdate();
                break;
        }
        Debug.Log(_hp);
        Debug.Log(_state);
	}

    void StartUpdate()
    {
        if(transform.position.y > 2.5f)
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
        else
        {
            _state = BossState.LEVEL_1;
        }
    }

    void Level_1_Update()
    {

    }

    void Level_2_Update()
    {
        transform.position += Vector3.right * Random.Range(-5, 5) * Time.deltaTime;
    }

    void Level_3_Update()
    {
        transform.position += Vector3.up * Random.Range(-5, 5) * Time.deltaTime;
    }

    void LastUpdate()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        // 出現時は当たらないようにする
        if (_state == BossState.START) { return; }

        // 弾が当たったら体力を減らす
        if (col.gameObject.tag == TagName.Bullet)
        {
            --_hp;
            if (_purgeIndex < _parts.Length && _purgeHP[_purgeIndex] == _hp)
            {
                PartsPurge();
                ++_purgeIndex;
            }

            if (_changeStateIndex < _changeStateHP.Length && _changeStateHP[_changeStateIndex] == _hp)
            {
                _state++;
                ++_changeStateIndex;
            }

            if(_hp == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void PartsPurge()
    {
        List<BossPurgeParts> parts = new List<BossPurgeParts>();
        foreach(var part in _parts)
        {
            if (!part.isPurge)
            {
                parts.Add(part);
            }
        }

        if(parts.Count == 0) { return; }

        int rand = Random.Range(0, parts.Count);
        parts[rand].Purge();
    }
}
