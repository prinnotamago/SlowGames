
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    int one_shot_count = 3;

    [SerializeField]
    float vibration_power = 200;

    [SerializeField]
    float burst_interval_time = 0.2f;

    [SerializeField]
    int max_burst_count = 3;

    int burst_count;

    bool is_shot = false;

    float time;
    SteamVR_TrackedObject tracked_Object;
    SteamVR_Controller.Device device;
    //GameObject steamVR_Camera;
    //Vector2 position;

    void Start()
    {
        burst_count = max_burst_count;
        time = burst_interval_time;
        tracked_Object = GetComponent<SteamVR_TrackedObject>();
        //steamVR_Camera = FindObjectOfType<SteamVR_Camera>().gameObject;
    }

        void Update()
    {
        device = SteamVR_Controller.Input((int)tracked_Object.index);
        ThreeBurst();
        //if (!device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)){ return;}
        //if (!Input.GetKeyDown(KeyCode.A)) { return; }
        is_shot = true;
        
    }

    void ThreeBurst()
    {
        if (is_shot == false) return;

            time -= Time.deltaTime;
            if (time > 0) return;

            //int vibration = 200 * i;
            AudioManager.instance.playSe(AudioName.SeName.gun1);
            device.TriggerHapticPulse(500);
            GameObject Shotbullet = Instantiate(Bullet);
            Shotbullet.transform.rotation = transform.rotation;
            Shotbullet.transform.position = transform.position + transform.forward;
            time = burst_interval_time;
            burst_count--;

        if (burst_count < 1)
        {
            burst_count = max_burst_count;
            is_shot = false;
        }
    }

}
