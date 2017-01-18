using UnityEngine;
using System.Collections;

public class TitlePlaneManager : MonoBehaviour {

    // 生成する座標の最大値
    [SerializeField]
    Vector2 _createPosMax = new Vector2(12, 40);

    // 生成する座標の最小値
    [SerializeField]
    Vector2 _createPosMin = new Vector2(-12, -40);

    // 生成する板
    [SerializeField]
    GameObject _planeObj = null;

    // 移動の速さの最大値
    [SerializeField]
    float _speedMax = 1.0f;
    // 移動の速さの最大値
    [SerializeField]
    float _speedMin = 0.3f;


    // 何秒に１回生成判定するか
    [SerializeField]
    float _createTimeDecision = 2.0f;
    float _createTime = 0.0f;

    // Use this for initialization
    void Start () {
        // 生成する時間をリセット
        _createTime = _createTimeDecision;
    }
	
	// Update is called once per frame
	void Update () {
        // 生成する時間じゃないなら
	    if(_createTime > 0)
        {
            // 時間を減らす
            _createTime -= Time.deltaTime;
        }
        // 生成する時間なら
        else
        {
            // 生成する
            var obj = Instantiate(_planeObj);
            var x = Random.Range(_createPosMin.x, _createPosMax.x);
            var y = Random.Range(_createPosMin.y, _createPosMax.y);
            obj.transform.position = new Vector3(x, y, -12);
            obj.transform.parent = transform;
            var speed = Random.Range(_speedMin, _speedMax);
            obj.GetComponent<TitlePlaneMove>().speed = speed;

            // 生成する時間をリセット
            _createTime = _createTimeDecision;
        }
	}
}
