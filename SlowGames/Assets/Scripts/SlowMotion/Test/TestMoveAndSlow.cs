using UnityEngine;
using System.Collections;

public class TestMoveAndSlow : MonoBehaviour {

    [SerializeField]
    GameObject _obj = null;

    [SerializeField]
    SlowMotion _slowMotion;

    [SerializeField]
    UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration _v;

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
            if (SlowMotion._instance.isSlow)
            {
                _obj.transform.position += new Vector3(10.0f, 0, 0) * Time.deltaTime;
            }
            else
            {
                _obj.transform.position += new Vector3(10.0f, 0, 0) * Time.deltaTime;
            }


            if (_obj.transform.position.x > 10)
            {
                _mode = Mode.Right;
            }
        }
        else
        {
            if (SlowMotion._instance.isSlow)
            {
                _obj.transform.position += new Vector3(-10.0f, 0, 0) * Time.deltaTime;
            }
            else
            {
                _obj.transform.position += new Vector3(-10.0f, 0, 0) * Time.deltaTime;
            }

            if (_obj.transform.position.x < -10)
            {
                _mode = Mode.Left;
            }
        }
       

        if (Input.GetKeyDown(KeyCode.S))
        {
            SlowMotion._instance.ResetSpeed();
            StartCoroutine(SlowEnd());
            Debug.Log("通常");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SlowMotion._instance.GameSpeed(0.1f);
            StartCoroutine(SlowStart());
            Debug.Log("スロー");
        }
    }

    IEnumerator SlowStart()
    {
        while (_v.intensity < 0.3f)
        {
            if (!SlowMotion._instance.isSlow) { break; }
            _v.intensity += 0.05f;
            yield return 0;
        }
    }

    IEnumerator SlowEnd()
    {
        while (_v.intensity > 0.0f)
        {
            if (SlowMotion._instance.isSlow) { break; }
            _v.intensity -= 0.05f;
            yield return 0;
        }
    }
}
