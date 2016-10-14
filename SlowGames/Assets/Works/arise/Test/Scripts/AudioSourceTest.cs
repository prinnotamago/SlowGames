using UnityEngine;

public class AudioSourceTest : MonoBehaviour
{
    void Start()
    {
        // 3D音用BGM再生
        AudioManager.instance.play3DBgm(gameObject, AudioName.BgmName.stage01);
        // 2D音用BGM再生
        AudioManager.instance.playBgm(AudioName.BgmName.stage01);

        // 2D用SE停止
        AudioManager.instance.stopSe(AudioName.SeName.Thunder);
        // 2D用SEを全て停止
        AudioManager.instance.stopAllSe();

        // Se全ての音量変更 ※デシベル単位
        AudioManager.instance.changeSEVolume(0.0f);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            AudioManager.instance.stop3DBgm(gameObject, AudioName.BgmName.stage01);
        }
    }
}
