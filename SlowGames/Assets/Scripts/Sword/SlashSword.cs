using UnityEngine;
using System.Collections;

public class SlashSword : MonoBehaviour {

    // 1フレーム前の座標を保持
    Vector3 prev;

    // 斬る判定を出す加速度
    // これ以上の速度で切れば斬る判定
    [SerializeField]
    float acceleration;

    // 攻撃判定があるかないか
    bool isAttack = false;
    public bool IsAttack { get { return isAttack; } }

    // テスト用
    [SerializeField]
    MeshRenderer mat;

    // Use this for initialization
    void Start () {
        // 初めの座標を入れる
        prev = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("前 " + prev);
        //Debug.Log("後 " + transform.localPosition);
        Debug.Log(prev == transform.position);

        // 前フレームと違う座標に今いるなら
        //if (prev == transform.position) {
            
        //}

        // 前フレームの座標との差を出す
        var length = (prev - transform.position).magnitude;

        // 移動距離が加速度を超えているか
        if (length > acceleration)
        {
            mat.material.color = Color.red;
        }
        else
        {
            mat.material.color = Color.white;
        }

        // 判定があるかないか
        isAttack = (length > acceleration);

        // 毎フレーム座標を更新
        prev = transform.position;
    }
}
