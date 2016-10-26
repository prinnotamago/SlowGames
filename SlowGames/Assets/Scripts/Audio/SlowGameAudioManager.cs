using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGameAudioManager : AudioManager
{
    AudioSource[] _slowSources = new AudioSource[SE_CHANNEL];

    /// <summary>
    /// index番号のclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="index">se番号</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource getNotSlowSe(int index)
    {
        int sourceIndex = -1;
        for (int i = 0; i < _slowSources.Length; ++i)
        {
            if (_slowSources[i].clip != null) continue;
            sourceIndex = i;
            break;
        }

        if (sourceIndex == -1) return null;

        var source = _slowSources[sourceIndex];
        source.clip = _seClips[index];
        return source;
    }

    /// <summary>
    /// nameのclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="name">se名</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource getNotSlowSe(AudioName.SeName name)
    {
        return getNotSlowSe((int)name);
    }

    /// <summary>
    /// SeのAudioSourceを取得
    /// </summary>
    /// <param name="channel">チャンネル番号</param>
    /// <returns></returns>
    public AudioSource getNotSlowSeSource(uint channel)
    {
        if (!(0 <= channel && channel < SE_CHANNEL)) return null;
        return _slowSources[channel];
    }

    /// <summary>
    /// index番号のSeを再生
    /// Seのチャンネル数が最大数使用していた場合、再生できない
    /// </summary>
    /// <param name="index">se番号</param>
    /// <param name="loop">ループするか</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource playNotSlowSe(int index, bool loop = false)
    {
        int sourceIndex = -1;
        for (int i = 0; i < _slowSources.Length; ++i)
        {
            var tempSource = _slowSources[i];
            if (tempSource.clip != null) continue;

            sourceIndex = i;
            break;
        }

        var count = 0;

        for (int i = 0; i < _slowSources.Length; ++i)
        {
            var tempSource = _slowSources[i];
            if (tempSource.clip == null) continue;

            for (int j = 0; j < _seClips.Length; ++j)
            {
                if (tempSource.clip.name == _seClips[j].name)
                {
                    count++;
                }
            }
        }

        if (count > LIMIT_SE_COUNT)
        {
            return null;
        }

        if (sourceIndex == -1) return null;

        var source = _slowSources[sourceIndex];
        source.clip = _seClips[index];
        source.Play();
        source.loop = loop;
        return source;
    }

    /// <summary>
    /// index番号のSeを再生
    /// Seのチャンネル数が最大数使用していた場合、再生できない
    /// </summary>
    /// <param name="name">se名</param>    
    /// <param name="loop">ループするか</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource playNotSlowSe(AudioName.SeName name, bool loop = false)
    {
        return playNotSlowSe((int)name, loop);
    }

    /// <summary>
    /// すべてのSeを停止
    /// </summary>
    public void stopAllNotSlowSe()
    {
        for (int i = 0; i < _slowSources.Length; ++i)
        {
            var source = _slowSources[i];
            if (source.clip == null) continue;
            source.Stop();
            source.loop = false;
            source.clip = null;
        }
    }

    /// <summary>
    /// Seを検索
    /// </summary>
    /// <param name="name">Seの名前</param>
    /// <returns>検索がヒットしなかったらnull</returns>
    public AudioSource findNotSlowSeSource(AudioName.SeName name)
    {
        AudioSource source = null;
        for (int i = 0; i < _slowSources.Length; ++i)
        {
            if (_slowSources[i].clip == null) continue;
            if (_slowSources[i].clip.name != name.ToString()) continue;
            source = _slowSources[i];
            break;
        }
        return source;
    }

    /// <summary>
    /// Seを検索(複数)
    /// </summary>
    /// <param name="name">Seの名前</param>
    /// <returns>検索がヒットしなかったら空</returns>
    public IEnumerable<AudioSource> findNotSlowSeSources(AudioName.SeName name)
    {
        List<AudioSource> sources = new List<AudioSource>();
        for (int i = 0; i < _slowSources.Length; ++i)
        {
            if (_slowSources[i].clip == null) continue;
            if (_slowSources[i].clip.name != name.ToString()) continue;
            sources.Add(_slowSources[i]);
        }
        return sources;
    }

    /// <summary>
    /// 特定のSeを止める
    /// </summary>
    /// <param name="name">Seの名前</param>
    /// <returns></returns>
    public AudioManager stopNotSlowSe(AudioName.SeName name)
    {
        var sources = findSeSources(name).GetEnumerator();
        while (sources.MoveNext())
        {
            var source = sources.Current;
            source.Stop();
            source.loop = false;
            source.clip = null;
        }
        return this;
    }

    override protected void Awake()
    {
        base.Awake();
    }

    override protected void Update()
    {
        base.Update();

        for (int i = 0; i < _slowSources.Length; ++i)
        {
            var source = _slowSources[i];
            if (source.clip == null) continue;
            if (source.isPlaying) continue;
            source.clip = null;
        }
    }
}
