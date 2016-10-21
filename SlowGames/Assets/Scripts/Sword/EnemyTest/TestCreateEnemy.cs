using UnityEngine;
using System.Collections;

public class TestCreateEnemy : MonoBehaviour {

    [SerializeField]
    GameObject[] _createTarget;

    [SerializeField]
    GameObject _enemyObj;

    [SerializeField]
    GameObject _parent;

    [SerializeField]
    bool _auto = false;
    [SerializeField]
    float _createTime = 1.0f;
    float _autoCount = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.G) || _autoCount >= _createTime)
        {
            var enemyObj = Instantiate(_enemyObj);
            int num = Random.Range(0, _createTarget.Length);
            enemyObj.transform.position = _createTarget[num].transform.position;
            enemyObj.transform.parent = _parent.transform;
        }

        if (_auto)
        {
            if (_autoCount >= _createTime)
            {
                _autoCount = 0.0f;
            }

            _autoCount += Time.deltaTime;       
             
        }
    }
}
