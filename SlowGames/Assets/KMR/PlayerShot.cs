
using UnityEngine;
[RequireComponent(typeof(Reload))]
public class PlayerShot : MonoBehaviour
{
    Reload reload;

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

    [SerializeField]
    int max_bullets_numbers = 20;

    int bullets_number;

    public int MaxBulletsNumbers
    {
        get { return max_bullets_numbers; }
        set { max_bullets_numbers = value; }
    }

    public int BulletsNumber
    {
        get { return bullets_number; }
        set { bullets_number = value; }
    }

    bool is_shot = false;

    float time;

    int burst_count;

    SteamVR_TrackedObject tracked_Object;
    SteamVR_Controller.Device device;
    //GameObject steamVR_Camera;
    //Vector2 position;

    void Start()
    {
        bullets_number = max_bullets_numbers;
        reload = GetComponent<Reload>();
        burst_count = max_burst_count;
        time = burst_interval_time;
        if (!SteamVR.active) return;
        tracked_Object = GetComponent<SteamVR_TrackedObject>();
        //steamVR_Camera = FindObjectOfType<SteamVR_Camera>().gameObject;
    }

        void Update()
    {
        if (SteamVR.active) { device = SteamVR_Controller.Input((int)tracked_Object.index); }
        ThreeBurst();
        if (!SteamVR.active && !Input.GetKeyDown(KeyCode.A)||
            SteamVR.active && !device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)){ return;}
        //if (!Input.GetKeyDown(KeyCode.A)) { return; }
        is_shot = true;
        burst_count = one_shot_count;
    }

    void ThreeBurst()
    {
        if (reload.isReload) return;
        if (is_shot == false) return;

            time -= Time.deltaTime;
            if (time > 0) return;

            //int vibration = 200 * i;
            AudioManager.instance.playSe(AudioName.SeName.gun1);
        if (SteamVR.active)
        {
            device.TriggerHapticPulse(1000);
        }
            GameObject Shotbullet = Instantiate(Bullet);
        Shotbullet.transform.rotation = transform.rotation;
        Shotbullet.transform.Rotate(45,0,0);
        //弾の発生位置変更
//            Shotbullet.transform.position = transform.position;
            Shotbullet.transform.position = transform.position;

            time = burst_interval_time;
            burst_count--;
        bullets_number--;
        if (burst_count < 1)
        {
            burst_count = one_shot_count;
            is_shot = false;
        }
    }

}
