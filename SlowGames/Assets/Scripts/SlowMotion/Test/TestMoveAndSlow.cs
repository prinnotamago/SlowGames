using UnityEngine;
using System.Collections;

public class TestMoveAndSlow : MonoBehaviour {

    [SerializeField]
    GameObject _obj = null;

    [SerializeField]
    SlowMotion _slowMotion;

    enum Mode
    {
        Left,
        Right,
    }
    Mode _mode = Mode.Left;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Time.unscaledDeltaTime);
        if(_mode == Mode.Left)
        {
            _obj.transform.position += new Vector3(10.0f, 0, 0) * Time.deltaTime;
            if(_obj.transform.position.x > 10)
            {
                _mode = Mode.Right;
            }
        }
        else
        {
            _obj.transform.position += new Vector3(-10.0f, 0, 0) * Time.deltaTime;
            if (_obj.transform.position.x < -10)
            {
                _mode = Mode.Left;
            }
        }
       

        if (Input.GetKeyDown(KeyCode.S))
        {
            SlowMotion._instance.ResetSpeed();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SlowMotion._instance.GameSpeed(0.1f);
        }
    }
}
