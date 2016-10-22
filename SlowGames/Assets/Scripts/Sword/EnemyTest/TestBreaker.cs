using UnityEngine;
using System.Collections;

public class TestBreaker : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject trackedObject;

    [SerializeField]
    Enemysbreaker breaker;

    // Use this for initialization
    void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            breaker.Breaker();
        }
        breaker.Breaker();
    }
}
