using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceNumberStorage : MonoBehaviour {

    private static int _voiceNumber = 0;

    /// <summary>
    /// Voiceを選択しなおしてsetする関数
    /// Titleはこっちを呼ぶ
    /// </summary>
    public static void VoiceRadomSelect()
    {
        _voiceNumber = UnityEngine.Random.Range(0, 3);
        AudioManager.instance.loadNavigations(_voiceNumber);
    }

    /// <summary>
    /// お客さんの人数からVoiceを決める
    /// </summary>
    public static void VoiceCustomSelect()
    {
        _voiceNumber = CustomerCounter.instance.GetCount() % 3;
        AudioManager.instance.loadNavigations(_voiceNumber);
    }

    /// <summary>
    /// Voiceをシーンの最初に選択する関数
    /// Title以外はこっちを呼ぶ
    /// </summary>
    public static void setVoice()
    {
        AudioManager.instance.loadNavigations(_voiceNumber);
    }
}
