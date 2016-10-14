
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField]
    GameObject Bullet;

    SteamVR_TrackedObject tracked_Object;
    SteamVR_Controller.Device device;
    //GameObject steamVR_Camera;
    //Vector2 position;

    void Start()
    {
        tracked_Object = GetComponent<SteamVR_TrackedObject>();
        //steamVR_Camera = FindObjectOfType<SteamVR_Camera>().gameObject;
    }

        void Update()
    {
        device = SteamVR_Controller.Input((int)tracked_Object.index);

        if (!device.GetPress(SteamVR_Controller.ButtonMask.Trigger)){ return;}
        //if (!Input.GetKeyDown(KeyCode.A)) { return; }
        GameObject Shotbullet = Instantiate(Bullet);
        //Vector3 front = transform.position;
        Shotbullet.transform.rotation = transform.rotation;
        Shotbullet.transform.position = transform.position + transform.forward;
    }
}
