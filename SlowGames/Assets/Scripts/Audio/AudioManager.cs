using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Audioを管理するクラス
/// </summary>
public class AudioManager : SingletonMonoBegaviour<AudioManager>
{
    enum Type
    {
        MASTER,
        BGM,
        SE,
		VOICE
    }

    [SerializeField]
    AudioClip[] _bgmClips = null;

    [SerializeField]
    protected AudioClip[] _seClips = null;

	[SerializeField]
	protected List<AudioClip> _voiceClips = null;

    AudioSource _bgmSource = null;

	List<AudioSource> _seSources = new List<AudioSource> ();

	List<AudioSource> _voiceSources = new List<AudioSource> ();

    List<AudioMixerGroup> _audioMixerGroup = new List<AudioMixerGroup>();

    List<AudioSource> _3dSources = new List<AudioSource>();

    //=====================================================================================================

    /// <summary>
    /// BGMのAudioSourceを取得
    /// </summary>
    public AudioSource getBgmSource
    {
        get
        {
            return _bgmSource;
        }
    }

    /// <summary>
    /// index番号のclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="index">bgm番号</param>
    /// <returns></returns>
    public AudioSource getBgm(int index)
    {
        _bgmSource.clip = _bgmClips[index];
        return _bgmSource;
    }

    /// <summary>
    /// nameのclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="name">bgm名</param>
    /// <returns></returns>
    public AudioSource getBgm(AudioName.BgmName name)
    {
        return getBgm((int)name);
    }

    /// <summary>
    /// index番号のBGMを再生
    /// </summary>
    /// <param name="index">bgm番号</param>
    /// <returns></returns>
    public AudioSource playBgm(int index)
    {
        _bgmSource.clip = _bgmClips[index];
        _bgmSource.Play();
        return _bgmSource;
    }

    /// <summary>
    /// nameのBGMを再生
    /// </summary>
    /// <param name="name">bgm名</param>
    /// <returns></returns>
    public AudioSource playBgm(AudioName.BgmName name)
    {
        return playBgm((int)name);
    }

    /// <summary>
    /// bgmを停止
    /// </summary>
    /// <returns></returns>
    public AudioSource stopBgm()
    {
        _bgmSource.Stop();
        return _bgmSource;
    }

    /// <summary>
    /// index番号のclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="index">se番号</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource getSe(int index)
    {
		return _seSources [index];
    }

    /// <summary>
    /// nameのclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="name">se名</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource getSe(AudioName.SeName name)
    {
        return getSe((int)name);
    }

    /// <summary>
    /// index番号のSeを再生
    /// Seのチャンネル数が最大数使用していた場合、再生できない
    /// </summary>
    /// <param name="index">se番号</param>
    /// <param name="loop">ループするか</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource playSe(int index, bool loop = false)
    {
		var source = _seSources [index];
        if (loop)
        {
            source.loop = loop;
            source.Play();
        }
        else
        {
            source.PlayOneShot(source.clip, source.volume);
        }

		return source;
    }

    /// <summary>
    /// index番号のSeを再生
    /// Seのチャンネル数が最大数使用していた場合、再生できない
    /// </summary>
    /// <param name="name">se名</param>    
    /// <param name="loop">ループするか</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource playSe(AudioName.SeName name, bool loop = false)
    {
        return playSe((int)name, loop);
    }

    /// <summary>
    /// すべてのSeを停止
    /// </summary>
    public void stopAllSe()
    {
		for(int i = 0; i < _seSources.Count; ++i){
			var source = _seSources [i];
			source.loop = false;
			source.Stop ();
		}
    }

    /// <summary>
    /// 音全体の音量を変更(デシベル単位)
    /// デシベルの参考URL：http://macasakr.sakura.ne.jp/decibel.html
    /// </summary>
    /// <param name="db">20から-80(0がAudioSourceのVolumeの1に当たる)</param>
    /// <returns></returns>
    public AudioManager changeMasterVolume(float db)
    {
        int type = (int)Type.MASTER;
        _audioMixerGroup[type].audioMixer.SetFloat("MasterVolume", db);
        return this;
    }

    /// <summary>
    /// BGMの音量を変更(デシベル単位)
    /// デシベルの参考URL：http://macasakr.sakura.ne.jp/decibel.html
    /// </summary>
    /// <param name="db">20から-80(0がAudioSourceのVolumeの1に当たる)</param>
    /// <returns></returns>
    public AudioManager changeBGMVolume(float db)
    {
        int type = (int)Type.BGM;
        _audioMixerGroup[type].audioMixer.SetFloat("BGMVolume", db);
        return this;
    }

    /// <summary>
    /// SEの音量を変更(デシベル単位)
    /// デシベルの参考URL：http://macasakr.sakura.ne.jp/decibel.html
    /// </summary>
    /// <param name="db">20から-80(0がAudioSourceのVolumeの1に当たる)</param>
    /// <returns></returns>
    public AudioManager changeSEVolume(float db)
    {
        int type = (int)Type.SE;
        _audioMixerGroup[type].audioMixer.SetFloat("SEVolume", db);
        return this;
    }

	/// <summary>
	/// SEの音量を変更(デシベル単位)
	/// デシベルの参考URL：http://macasakr.sakura.ne.jp/decibel.html
	/// </summary>
	/// <param name="db">20から-80(0がAudioSourceのVolumeの1に当たる)</param>
	/// <returns></returns>
	public AudioManager changeVoiceVolume(float db)
	{
		int type = (int)Type.VOICE;
		_audioMixerGroup[type].audioMixer.SetFloat("VoiceVolume", db);
		return this;
	}

    /// <summary>
    /// BGMのピッチを変更
    /// </summary>
    /// <param name="value">-3 to +3</param>
    /// <returns></returns>
    public AudioManager changeBGMPitch(float value)
    {
        _bgmSource.pitch = value;
        return this;
    }

    /// <summary>
    /// SEのピッチを変更
    /// </summary>
    /// <param name="value">-3 to +3</param>
    /// <returns></returns>
    public AudioManager changeSEPitch(float value)
    {
        foreach (var source in _seSources)
        {
            source.pitch = value;
        }
        return this;
    }

    /// <summary>
    /// フェードインしながらBGMを再生
    /// 現在のボリュームからMaxVolume(1.0f)までフェードする
    /// </summary>
    /// <param name="name">鳴らすBGM</param>
    /// <param name="fade_time">フェードする時間</param>
    /// <returns></returns>
    public AudioManager fadeInBGM(AudioName.BgmName name, float fade_time)
    {
        StopAllCoroutines();
        playBgm(name);
        StartCoroutine(fade(fade_time, _bgmSource.volume, 1.0f));
        return this;
    }

    /// <summary>
    /// フェードインしながらBGMを再生
    /// </summary>
    /// <param name="name">鳴らすBGM</param>
    /// <param name="fade_time">フェードする時間</param>
    /// <param name="start_volume">最初のボリューム</param>
    /// <param name="end_volume">最後のボリューム</param>
    /// <returns></returns>
    public AudioManager fadeInBGM(AudioName.BgmName name, float fade_time, float start_volume, float end_volume)
    {
        StopAllCoroutines();
        playBgm(name);
        StartCoroutine(fade(fade_time, start_volume, end_volume));
        return this;
    }

    /// <summary>
    /// フェードアウトでBGMのボリュームを下げる
    /// 現在のボリュームからMinVolume(0.0f)までフェードする
    /// </summary>
    /// <param name="fade_time">フェードする時間</param>
    /// <returns></returns>
    public AudioManager fadeOutBGM(float fade_time)
    {
        StopAllCoroutines();
        StartCoroutine(fade(fade_time, _bgmSource.volume, 0.0f));
        return this;
    }

    /// <summary>
    /// フェードアウトでBGMのボリュームを下げる
    /// </summary>
    /// <param name="fade_time">フェードする時間</param>
    /// <param name="start_volume">最初のボリューム</param>
    /// <param name="end_volume">最後のボリューム</param>
    /// <returns></returns>
    public AudioManager fadeOutBGM(float fade_time, float start_volume, float end_volume)
    {
        StopAllCoroutines();
        StartCoroutine(fade(fade_time, start_volume, end_volume));
        return this;
    }

    /// <summary>
    /// フェードしながらBGMを変更する(Out→In)
    /// 現在のボリュームからフェードする
    /// </summary>
    /// <param name="next_name">次鳴らすBGMの名前</param>
    /// <param name="fade_time">フェードする時間</param>
    /// <returns></returns>
    public AudioManager fadeChangeBGM(AudioName.BgmName next_name, float fade_time)
    {
        StopAllCoroutines();
        StartCoroutine(fadeOutIn(next_name, fade_time, _bgmSource.volume, 0.0f));
        return this;
    }

    /// <summary>
    /// フェードしながらBGMを変更する(Out→In)
    /// </summary>
    /// <param name="next_name">次鳴らすBGMの名前</param>
    /// <param name="fade_time">フェードする時間</param>
    /// <param name="start_volume">最初のボリューム</param>
    /// <param name="end_volume">最後のボリューム</param>
    /// <returns></returns>
    public AudioManager fadeChangeBGM(AudioName.BgmName next_name, float fade_time, float start_volume, float end_volume)
    {
        StopAllCoroutines();
        StartCoroutine(fadeOutIn(next_name, fade_time, start_volume, end_volume));
        return this;
    }

    /// <summary>
    /// Seを検索
    /// </summary>
    /// <param name="name">Seの名前</param>
    /// <returns>検索がヒットしなかったらnull</returns>
    public AudioSource findSeSource(AudioName.SeName name)
    {
        AudioSource source = null;
		for (int i = 0; i < _seSources.Count; ++i)
        {
            if (_seSources[i].clip.name != name.ToString()) continue;
            source = _seSources[i];
            break;
        }
        return source;
    }

    /// <summary>
    /// Seを検索(複数)
    /// </summary>
    /// <param name="name">Seの名前</param>
    /// <returns>検索がヒットしなかったら空</returns>
    public IEnumerable<AudioSource> findSeSources(AudioName.SeName name)
    {
        List<AudioSource> sources = new List<AudioSource>();
		for (int i = 0; i < _seSources.Count; ++i)
        {
            if (_seSources[i].clip.name != name.ToString()) continue;
            sources.Add(_seSources[i]);
        }
        return sources;
    }

    /// <summary>
    /// 特定のSeを止める
    /// </summary>
    /// <param name="name">Seの名前</param>
    /// <returns></returns>
    public AudioManager stopSe(AudioName.SeName name)
    {
        var sources = findSeSources(name).GetEnumerator();
        while (sources.MoveNext())
        {
            var source = sources.Current;
            source.Stop();
            source.loop = false;
        }
        return this;
    }

    /// <summary>
    /// 3D音SE再生
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="index">SE番号</param>
    /// <param name="loop">ループするか</param>
    /// <returns></returns>
    public AudioSource play3DSe(GameObject gameobject, int index, bool loop = false)
    {
        AudioSource audioSource = null;

        var audioSources = gameobject.GetComponents<AudioSource>();

        if (audioSources.Length != 0)
        {
            foreach (var tempAudioSource in audioSources)
            {
                if (tempAudioSource.isPlaying) continue;
                if (tempAudioSource.outputAudioMixerGroup.name != "SE") continue;
                var name = ((AudioName.SeName)index).ToString().GetHashCode();
                if (tempAudioSource.clip.name.GetHashCode() != name) continue;
                audioSource = tempAudioSource;
                break;
            }
        }

        if (audioSource == null)
        {
            audioSource = gameobject.AddComponent<AudioSource>();

            _3dSources.Add(audioSource);

            audioSource.dopplerLevel = 0.0f;
            audioSource.spatialBlend = 1.0f;

            int typeIndex = (int)Type.SE;
            audioSource.outputAudioMixerGroup = _audioMixerGroup[typeIndex];
        }

        audioSource.clip = _seClips[index];
        //audioSource.loop = loop;
        //audioSource.spatialize = true;

        //audioSource.PlayOneShot(audioSource.clip, audioSource.volume);

        if (loop)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(audioSource.clip, audioSource.volume);
        }

        return audioSource;
    }

    /// <summary>
    /// 3D音BGM再生
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="index">BGM番号</param>
    /// <param name="loop">ループするか</param>
    /// <returns></returns>
    public AudioSource play3DBgm(GameObject gameobject, int index, bool loop = true)
    {
        AudioSource audioSource = null;

        var audioSources = gameobject.GetComponents<AudioSource>();

        if (audioSources.Length != 0)
        {
            foreach (var tempAudioSource in audioSources)
            {
                if (tempAudioSource.isPlaying) continue;
                if (tempAudioSource.outputAudioMixerGroup.name != "BGM") continue;
                audioSource = tempAudioSource;
                break;
            }
        }

        if (audioSource == null)
        {
            audioSource = gameobject.AddComponent<AudioSource>();

            _3dSources.Add(audioSource);

            audioSource.dopplerLevel = 0.0f;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = 1;
            audioSource.maxDistance = 30;

            int typeIndex = (int)Type.BGM;
            audioSource.outputAudioMixerGroup = _audioMixerGroup[typeIndex];
        }

        audioSource.clip = _bgmClips[index];
        audioSource.loop = loop;

        audioSource.Play();

        return audioSource;
    }

    /// <summary>
    /// 特定のオブジェクト内の3D音SE再生
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="name">SEの名前</param>
    /// <param name="loop">ループするか</param>
    /// <returns></returns>
    public AudioSource play3DSe(GameObject gameobject, AudioName.SeName name, bool loop = false)
    {
        var index = (int)name;
        return play3DSe(gameobject, index, loop);
    }

    /// <summary>
    /// 特定のオブジェクト内の3D音BGM再生
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="name">BGMの名前</param>
    /// <param name="loop">ループするか</param>
    /// <returns></returns>
    public AudioSource play3DBgm(GameObject gameobject, AudioName.BgmName name, bool loop = false)
    {
        var index = (int)name;
        return play3DBgm(gameobject, index, loop);
    }

    /// <summary>
    /// 特定のオブジェクト内の3D音Seを止める
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="name">SEの名前</param>
    /// <returns></returns>
    public AudioManager stop3DSe(GameObject gameobject, AudioName.SeName name)
    {
        var audioSources = gameobject.GetComponents<AudioSource>();

        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip.name != name.ToString()) continue;
            audioSource.Stop();
        }
        return this;
    }

    /// <summary>
    /// 特定のオブジェクト内の3D音Bgmを止める
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="name">BGMの名前</param>
    /// <returns></returns>
    public AudioManager stop3DBgm(GameObject gameobject, AudioName.BgmName name)
    {
        var audioSources = gameobject.GetComponents<AudioSource>();

        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip.name != name.ToString()) continue;
            audioSource.Stop();
        }
        return this;
    }

    /// <summary>
    /// 特定のオブジェクト内の3D音Seを全て止める
    /// </summary>
    /// <param name="gameobject"></param>
    /// <returns></returns>
    public AudioManager stopAll3DSe(GameObject gameobject)
    {
        var audioSources = gameobject.GetComponents<AudioSource>();
        if (audioSources == null) return this;

        foreach (var audioSource in audioSources)
        {
            if (audioSource.outputAudioMixerGroup.name != "SE") continue;
            audioSource.Stop();
        }

        return this;
    }

    /// <summary>
    /// 特定のオブジェクト内の3D音Bgmを全て止める
    /// </summary>
    /// <param name="gameobject"></param>
    /// <returns></returns>
    public AudioManager stopAll3DBgm(GameObject gameobject)
    {
        var audioSources = gameobject.GetComponents<AudioSource>();
        if (audioSources == null) return this;

        foreach (var audioSource in audioSources)
        {
            if (audioSource.outputAudioMixerGroup.name != "BGM") continue;
            audioSource.Stop();
        }
        return this;
    }

    /// <summary>
    /// 特定のオブジェクトのAudioSourceを取得
    /// </summary>
    /// <param name="gameobject"></param>
    /// <returns></returns>
    public IEnumerable<AudioSource> get3DAudioSources(GameObject gameobject)
    {
        return gameobject.GetComponents<AudioSource>();
    }

    /// <summary>
    /// 3D音響用のピッチを変更
    /// </summary>
    /// <param name="value">-3 to +3</param>
    /// <returns></returns>
    public AudioManager change3DSourcePitch(float value)
    {
        foreach (var source in _3dSources)
        {
            if (source == null) continue;
            source.pitch = value;
        }
        return this;
    }

    /// <summary>
    /// 全てのAudioSourceのピッチを変更
    /// </summary>
    /// <param name="value">-3 to +3</param>
    /// <returns></returns>
    public AudioManager changeSourceAllPitch(float value)
    {
        change3DSourcePitch(value);
        changeBGMPitch(value);
        changeSEPitch(value);
        return this;
    }

	List<AudioSource> _slowSources = new List<AudioSource>();

    /// <summary>
    /// index番号のclipを入れ、AudioSourceを取得
    /// </summary>
    /// <param name="index">se番号</param>
    /// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
    public AudioSource getNotSlowSe(int index)
    {
		return _slowSources [index];
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
    public AudioSource getNotSlowSeSource(int channel)
    {
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
		var source = _slowSources [index];
		source.loop = loop;
        source.PlayOneShot(source.clip, source.volume);
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
		for (int i = 0; i < _slowSources.Count; ++i)
        {
            var source = _slowSources[i];
            source.Stop();
            source.loop = false;
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
		for (int i = 0; i < _slowSources.Count; ++i)
        {
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
		for (int i = 0; i < _slowSources.Count; ++i)
        {
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
        var sources = findNotSlowSeSources(name).GetEnumerator();
        while (sources.MoveNext())
        {
            var source = sources.Current;
            source.Stop();
            source.loop = false;
        }
        return this;
    }

	// ====================================================================

	/// <summary>
	/// index番号のclipを入れ、AudioSourceを取得
	/// </summary>
	/// <param name="index">se番号</param>
	/// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
	public AudioSource getVoice(int index)
	{
		return _voiceSources [index];
	}

	/// <summary>
	/// nameのclipを入れ、AudioSourceを取得
	/// </summary>
	/// <param name="name">se名</param>
	/// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
	public AudioSource getVoice(AudioName.VoiceName name)
	{
		return getVoice((int)name);
	}

	/// <summary>
	/// index番号のSeを再生
	/// Seのチャンネル数が最大数使用していた場合、再生できない
	/// </summary>
	/// <param name="index">se番号</param>
	/// <param name="loop">ループするか</param>
	/// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
	public AudioSource playVoice(int index, bool loop = false)
	{
		var source = _voiceSources [index];
		source.loop = loop;
        source.PlayOneShot(source.clip, source.volume);
		return source;
	}

	/// <summary>
	/// index番号のSeを再生
	/// Seのチャンネル数が最大数使用していた場合、再生できない
	/// </summary>
	/// <param name="name">se名</param>    
	/// <param name="loop">ループするか</param>
	/// <returns>Seのチャンネル数が最大数使用していた場合nullが返る</returns>
	public AudioSource playVoice(AudioName.VoiceName name, bool loop = false)
	{
		return playVoice((int)name, loop);
	}

	/// <summary>
	/// すべてのSeを停止
	/// </summary>
	public void stopAllVoice()
	{
		for (int i = 0; i < _voiceSources.Count; ++i) {
			var source = _voiceSources[i];
			source.Stop();
			source.loop = false;
		}
	}

	/// <summary>
	/// Seを検索
	/// </summary>
	/// <param name="name">Seの名前</param>
	/// <returns>検索がヒットしなかったらnull</returns>
	public AudioSource findVoiceSource(AudioName.VoiceName name)
	{
		AudioSource source = null;
		for (int i = 0; i < _voiceSources.Count; ++i)
		{
			if (_voiceSources[i].clip.name != name.ToString()) continue;
			source = _voiceSources[i];
			break;
		}
		return source;
	}

	/// <summary>
	/// Seを検索(複数)
	/// </summary>
	/// <param name="name">Seの名前</param>
	/// <returns>検索がヒットしなかったら空</returns>
	public IEnumerable<AudioSource> findVoiceSources(AudioName.VoiceName name)
	{
		List<AudioSource> sources = new List<AudioSource>();
		for (int i = 0; i < _voiceSources.Count; ++i)
		{
			if (_voiceSources[i].clip.name != name.ToString()) continue;
			sources.Add(_voiceSources[i]);
		}
		return sources;
	}

	/// <summary>
	/// 特定のSeを止める
	/// </summary>
	/// <param name="name">Seの名前</param>
	/// <returns></returns>
	public AudioManager stopVoice(AudioName.VoiceName name)
	{
		var sources = findVoiceSources(name).GetEnumerator();
		while (sources.MoveNext())
		{
			var source = sources.Current;
			source.Stop();
			source.loop = false;
		}
		return this;
	}

    /// <summary>
    /// ナビゲーションボイスをロードする
    /// </summary>
    /// <param name="navigationFileNumber">ロードしたいナビゲーションのフォルダナンバー</param>
    /// <returns></returns>
    public AudioManager loadNavigations(int navigationFolderNumber) {

        var path = navigationFolderNumber.ToString();
        var resources = Resources.LoadAll<AudioClip>("Audio/Navigations/" + path);
        _voiceClips.AddRange(resources);

        const int voiceType = (int)Type.VOICE;

        for (int i = 0; i < resources.Length; ++i)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = resources[i];
            source.outputAudioMixerGroup = _audioMixerGroup[voiceType];
            _voiceSources.Add(source);
        }

        return this;
    }

    //============================================================================================

    override protected void Awake()
    {
        base.Awake();

        var audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        var audioMixerGroup = audioMixer.FindMatchingGroups(string.Empty);

        _audioMixerGroup.AddRange(audioMixerGroup);

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;

        const int bgmType = (int)Type.BGM;

        _bgmSource.outputAudioMixerGroup = _audioMixerGroup[bgmType];

        const int seType = (int)Type.SE;

		_bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");
		_seClips = Resources.LoadAll<AudioClip>("Audio/SE");
        _voiceClips.AddRange(Resources.LoadAll<AudioClip>("Audio/Voice"));

		for (int i = 0; i < _seClips.Length; ++i) {
			var audioSource = gameObject.AddComponent<AudioSource> ();
			audioSource.clip = _seClips [i];
			_seSources.Add(audioSource);
			_slowSources.Add(audioSource);
            _seSources[i].outputAudioMixerGroup = _audioMixerGroup[seType];
        }

		const int voiceType = (int)Type.VOICE;

		for (int i = 0; i < _voiceClips.Count; ++i) {
			var source = gameObject.AddComponent<AudioSource> ();
			source.clip = _voiceClips [i];
			_voiceSources.Add (source);
			_voiceSources [i].outputAudioMixerGroup = _audioMixerGroup [voiceType];
		}
    }

    override protected void Update()
    {
        _3dSources.RemoveAll(source => source == null);
    }

    IEnumerator fade(float fade_time, float start_volume, float end_volume)
    {
        float time = 0.0f;

        while (1.0f >= time)
        {
            time += Time.deltaTime / fade_time;
            _bgmSource.volume = Mathf.Lerp(start_volume, end_volume, time);
            yield return null;
        }
    }

    IEnumerator fadeOutIn(AudioName.BgmName name, float fade_time, float start_volume, float end_volume)
    {
        float time = 0.0f;

        while (1.0f >= time)
        {
            time += Time.deltaTime / fade_time;
            _bgmSource.volume = Mathf.Lerp(start_volume, end_volume, time);
            yield return null;
        }

        time = 0.0f;
        playBgm(name);

        while (1.0f >= time)
        {
            time += Time.deltaTime / fade_time;
            _bgmSource.volume = Mathf.Lerp(end_volume, start_volume, time);
            yield return null;
        }
    }
}