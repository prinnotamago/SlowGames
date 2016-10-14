using UnityEngine;

public class AudioSourceTest : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.play3DBgm(gameObject, 0);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            AudioManager.instance.stop3DBgm(gameObject, AudioName.BgmName.stage01);
        }
    }
}
