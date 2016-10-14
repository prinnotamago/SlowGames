using UnityEngine;
using System.Collections;

public class TestMoveAndSlow : MonoBehaviour {

    [SerializeField]
    GameObject obj = null;

    [SerializeField]
    SlowMotion slowMotion;

    enum Mode
    {
        Left,
        Right,
    }
    Mode mode = Mode.Left;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Time.unscaledDeltaTime);
        if(mode == Mode.Left)
        {
            obj.transform.position += new Vector3(10.0f, 0, 0) * Time.deltaTime;
            if(obj.transform.position.x > 10)
            {
                mode = Mode.Right;
            }
        }
        else
        {
            obj.transform.position += new Vector3(-10.0f, 0, 0) * Time.deltaTime;
            if (obj.transform.position.x < -10)
            {
                mode = Mode.Left;
            }
        }
       

        if (Input.GetKeyDown(KeyCode.A))
        {
            slowMotion.SetMotionTime(0.1f, 3.0f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SlowMotion.ResetSpeed();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SlowMotion.GameSpeed(0.1f);
        }
    }
}
