using UnityEngine;

public class AudioSourceTest : MonoBehaviour
{
    void Start()
    {
        //// 3D音用BGM再生
        //AudioManager.instance.play3DBgm(gameObject, AudioName.BgmName.stage01);

        AudioManager.instance.play3DSe(gameObject, AudioName.SeName.Thunder);

        //AudioManager.instance.playSe(AudioName.SeName.gun1);
    }

    void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    AudioManager.instance.stop3DBgm(gameObject, AudioName.BgmName.stage01);
        //}
    }
}
