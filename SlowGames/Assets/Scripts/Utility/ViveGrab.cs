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

    [SerializeField]
    private GameObject _viveConObj = null;

    GameObject _pickObj = null;

    bool _isPick = false;
    public bool isPick { get { return _isPick; } }

    SteamVR_TrackedObject _viveCon;

    Rigidbody _pickObjRigidbody = null;

    [SerializeField]
    bool _isDrop = false;

    // Use this for initialization
    void Start () {
        //_rigidbody = GetComponent<Rigidbody>();

        _viveCon = GetComponent<SteamVR_TrackedObject>();
	}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_isPick);
        if (!_isDrop && _isPick)
        {
            _pickObjRigidbody.velocity = Vector3.zero;
            _pickObj.gameObject.transform.position = transform.position;
            _pickObj.gameObject.transform.rotation = transform.rotation;
            _pickObj.gameObject.transform.Rotate(235, 0, 180);
        }
        
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == _tagName)
        {
            _viveConObj.SetActive(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == _tagName)
        {
            if (_pickObj == col.gameObject)
            {
                SteamVR_Controller.Device device = SteamVR_Controller.Input((int)_viveCon.index);
                if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                {
                    _pickObjRigidbody.velocity = Vector3.zero;
                    col.gameObject.transform.position = transform.position;
                    col.gameObject.transform.rotation = transform.rotation;
                    col.gameObject.transform.Rotate(235, 0, 180);
                    if (_pickObjRigidbody.useGravity)
                    {
                        _pickObjRigidbody.useGravity = false;
                        //Debug.Log("もってる");
                    }
                }
                else
                {
                    if (!_isDrop) { return; }
                    _pickObjRigidbody.useGravity = true;
                    _isPick = false;
                    _pickObj = null;
                    _pickObjRigidbody = null;
                    //Debug.Log("離した");
                }
            }
            else if(_pickObj == null)
            {
                SteamVR_Controller.Device device = SteamVR_Controller.Input((int)_viveCon.index);
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    _pickObj = col.gameObject;
                    _pickObjRigidbody = col.GetComponent<Rigidbody>();
                    _isPick = true;
                    _viveConObj.GetComponent<ViveConPosMover>().enabled = false;
                    _viveConObj.SetActive(false);

                    foreach (var i in col.gameObject.GetComponents<BoxCollider>())
                    {
                        i.enabled = false;
                    }
                    GetComponent<BoxCollider>().enabled = false;

                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == _tagName)
        {
            _viveConObj.SetActive(false);
        }

        if (!_isDrop) { return; }
        if (col.gameObject.tag == _tagName)
        {
            if (_pickObj == col.gameObject)
            {
                //var rigidbody = col.GetComponent<Rigidbody>();
                _pickObjRigidbody.useGravity = true;
                _pickObjRigidbody = null;
                _isPick = false;
                _pickObj = null;
            }
        }
    }
}
