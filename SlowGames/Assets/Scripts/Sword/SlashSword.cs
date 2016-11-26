using UnityEngine;
using System.Collections;

public class SlashSword : MonoBehaviour {

    // 1フレーム前の座標を保持
    Vector3 prev;

    /// <summary>
    /// 斬る判定を出す加速度
    /// これ以上の速度で切れば斬る判定
    /// </summary>
    [SerializeField]
    float acceleration;

    // 攻撃判定があるかないか
    bool isAttack = false;
    /// <summary>
    /// 攻撃判定があるかないか
    /// </summary>
    public bool IsAttack { get { return isAttack; } }

    // テスト用
    //[SerializeField]
    //MeshRenderer mat;

    // 斬る種類
    public enum SlashPattern
    {
        NONE,               // 斬ってない

        UP_DOWN,            // 上から下に斬る
        DOWN_UP,            // 下から上に斬る
        RIGHT_LEFT,         // 右から左に斬る
        LEFT_RIGHT,         // 左から右に斬る

        UPRIGHT_DOWNLEFT,   // 右上から左下に斬る
        UPLEFT_DOWNRIGHT,   // 左上から右下に斬る

        DOWNRIGHT_UPLEFT,   // 右下から左上に斬る
        DOWNLEFT_UPRIGHT,   // 左下から右上に斬る

        ALL_RANGE,          // 全方向
    }
    SlashPattern _pattern = SlashPattern.NONE;
    public SlashPattern pattern { get { return _pattern; } }

    /// <summary>
    /// 斜め斬りの感度
    /// 1.0 以下にしてください
    /// </summary>
    [SerializeField]
    float _slantingNum = 0.1f;

    /// <summary>
    /// このシーンのカメラ
    /// </summary>
    [SerializeField]
    Camera _camera;

    // Use this for initialization
    void Start () {
        // 初めの座標を入れる
        prev = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("前 " + prev);
        //Debug.Log("後 " + transform.localPosition);
        //Debug.Log(prev == transform.position);

        // 前フレームと違う座標に今いるなら
        //if (prev == transform.position) {
            
        //}

        // 前フレームの座標との差を出す
        var length = (prev - transform.position).magnitude;

        var a = _camera.WorldToViewportPoint(prev);
        var b = _camera.WorldToViewportPoint(transform.position);
        var c = a - b;

        _pattern = SlashPattern.NONE;

        //Debug.Log("a : " + a);
        //Debug.Log("b : " + b);
        //Debug.Log("c     : " + c);
        //Debug.Log("a - b : " + (a - b));

        // 移動距離が加速度を超えているか
        if (length > acceleration)
        {
            //mat.material.color = Color.red;

            _pattern = UpdatePattern(c);
        }
        else
        {
            //mat.material.color = Color.white;
        }

        // 判定があるかないか
        isAttack = (length > acceleration);

        // 毎フレーム座標を更新
        prev = transform.position;

       //Debug.Log(_pattern);
    }

    SlashPattern UpdatePattern(Vector3 c)
    {
        SlashPattern pattern = SlashPattern.NONE;

        bool tate = (c.y > 0);  // +上　-下
        bool yoko = (c.x > 0);  // +右　-左
        float xySizeDefference = (c.x == 0.0f) ? 2.0f : Mathf.Abs((c.y / c.x)); // 2.0f は縦斬りにするための適当な値(1.0f より大きければなんでもよい)
        bool naname = (_slantingNum < xySizeDefference && xySizeDefference <= 1.0f);

        //Debug.Log(c);
        //Debug.Log(xySizeDefference);

        // 斜めかどうか
        if (naname) // 斜めなら
        {
            // 下から上に斬ったなら
            if (tate)
            {
                // 左から右に斬ったなら
                if (yoko)
                {
                    pattern = SlashPattern.UPRIGHT_DOWNLEFT;
                }
                // 右から左に斬ったなら
                else
                {
                    pattern = SlashPattern.UPLEFT_DOWNRIGHT;
                }
            }
            // 上から下に斬ったなら
            else
            {
                // 左から右に斬ったなら
                if (yoko)
                {
                    pattern = SlashPattern.DOWNRIGHT_UPLEFT;
                }
                // 右から左に斬ったなら
                else
                {
                    pattern = SlashPattern.DOWNLEFT_UPRIGHT;
                }
            }
        }
        else // 斜めじゃないなら
        {
            // 1.0f より大きいということは縦に斬ったということ
            if(xySizeDefference > 1.0f)
            {
                if (tate)
                {
                    pattern = SlashPattern.UP_DOWN;
                }
                else
                {
                    pattern = SlashPattern.DOWN_UP;
                }
            }
            else
            {
                if (yoko)
                {                
                    pattern = SlashPattern.RIGHT_LEFT;
                }
                else
                {
                    pattern = SlashPattern.LEFT_RIGHT;
                }
            }
        }

        return pattern;
    }
}
