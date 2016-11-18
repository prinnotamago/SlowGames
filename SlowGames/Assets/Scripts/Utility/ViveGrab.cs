using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGrab : MonoBehaviour {

    //[SerializeField]
    //Rigidbody _rigidbody;

    //[SerializeField]
    //SteamVR_TrackedObject[] _viveCon;
    [SerializeField]
    string _tagName = TagName.Weapon;

    GameObject _pickObj = null;

    bool _isPick = false;
    public bool isPick { get { return _isPick; } }

    SteamVR_TrackedObject _viveCon;

    // Use this for initialization
    void Start () {
        //_rigidbody = GetComponent<Rigidbody>();

        _viveCon = GetComponent<SteamVR_TrackedObject>();
	}

    // Update is called once per frame
    //void Update()
    //{
    //    Debug.Log(_isPick);
    //}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == _tagName)
        {
            if (_pickObj == col.gameObject)
            {
                var rigidbody = col.GetComponent<Rigidbody>();
                SteamVR_Controller.Device device = SteamVR_Controller.Input((int)_viveCon.index);
                if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                {
                    rigidbody.velocity = Vector3.zero;
                    col.gameObject.transform.position = transform.position;
                    col.gameObject.transform.rotation = transform.rotation;
                    col.gameObject.transform.Rotate(235, 0, 180);
                    if (rigidbody.useGravity)
                    {
                        rigidbody.useGravity = false;
                        //Debug.Log("もってる");
                    }
                }
                else
                {
                    rigidbody.useGravity = true;
                    _isPick = false;
                    _pickObj = null;
                    //Debug.Log("離した");
                }
            }
            else if(_pickObj == null)
            {
                SteamVR_Controller.Device device = SteamVR_Controller.Input((int)_viveCon.index);
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    _pickObj = col.gameObject;
                    _isPick = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == _tagName)
        {
            if (_pickObj == col.gameObject)
            {
                var rigidbody = col.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                _isPick = false;
                _pickObj = null;
            }
        }
    }
}
