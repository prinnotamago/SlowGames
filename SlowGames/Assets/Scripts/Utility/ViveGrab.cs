using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGrab : MonoBehaviour {

    //[SerializeField]
    Rigidbody _rigidbody;

    SteamVR_TrackedObject[] _viveCon;

	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();

        _viveCon = FindObjectsOfType<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	//void Update () {
	//	
	//}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == TagName.GameController)
        {
            bool isPick = false;
            foreach (var viveCon in _viveCon)
            {
                if (viveCon.gameObject == col.gameObject)
                {
                    SteamVR_Controller.Device device = SteamVR_Controller.Input((int)viveCon.index);
                    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        transform.position = col.gameObject.transform.position;
                        _rigidbody.velocity = Vector3.zero;
                        if (_rigidbody.useGravity)
                        {
                            _rigidbody.useGravity = false;
                        }
                        isPick = true;
                    }
                }
            }

            if (!isPick)
            {
                if (!_rigidbody.useGravity)
                {
                    _rigidbody.useGravity = true;
                }
            }
        }
    }
}
