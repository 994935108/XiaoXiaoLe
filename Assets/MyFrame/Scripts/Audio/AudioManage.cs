using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class AudioSetting
{
    [Range(0,1)]
    public static float volume=1;
    [Range(0, 1)]
    public static float sound =1;

   
}

    public class AudioManage : SingletonGameObject<AudioManage>
    {
        private Dictionary<GameObject, List<AudioSource>> mAudioSourceDic = new Dictionary<GameObject, List<AudioSource>>();
        private AudioSource mBgAudioSource;
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="audioObj"></param>
        /// <param name="audioClipPath"></param>
        public void PlayBGMusic(GameObject audioObj, string audioClipPath)
        {
            if (mBgAudioSource == null)
            {
                GameObject BgAudioSource = new GameObject("BgAudioSource");
                mBgAudioSource = audioObj.AddComponent<AudioSource>();
                AudioClip audioClip = ResourcesManage.Instance.Load<AudioClip>(audioClipPath);
                mBgAudioSource.clip = audioClip;
                SettingDefaultVolumeValue(mBgAudioSource);
                IsLoop(mBgAudioSource,true);
                mBgAudioSource.Play();
            }

        }
        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBGMusic()
        {
            if (mBgAudioSource != null)
            {
                if (mBgAudioSource.isPlaying)
                {
                    mBgAudioSource.Stop();

                }
            }
        }
        /// <summary>
        /// 暂停背景音乐播放
        /// </summary>
        public void PauseBGMusic()
        {
            if (mBgAudioSource != null)
            {
                if (mBgAudioSource.isPlaying)
                {
                    mBgAudioSource.Pause();
                }
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioObj"></param>
        /// <param name="audioClipPath"></param>
        public void PlaySound(GameObject audioObj, string audioClipPath, bool isLoop)
        {

            //先把播放结束的停掉
            if (mAudioSourceDic.ContainsKey(audioObj))
            {
                for (int i = 0; i < mAudioSourceDic[audioObj].Count; i++)
                {
                    if (!mAudioSourceDic[audioObj][i].isPlaying)
                    {
                        Destroy(mAudioSourceDic[audioObj][i]);
                        mAudioSourceDic[audioObj].Remove(mAudioSourceDic[audioObj][i]);
                    }
                }

            }
            ResourcesManage.Instance.LoadAsync<AudioClip>(audioClipPath, (audioClip) =>
            {
                AudioSource audioSource = audioObj.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.loop = isLoop;
                audioSource.Play();
                if (mAudioSourceDic.ContainsKey(audioObj))
                {
                    mAudioSourceDic[audioObj].Add(audioSource);
                }
                else
                {
                    mAudioSourceDic.Add(audioObj, new List<AudioSource> { audioSource });
                }
            });

        }

        /// <summary>
        /// 根据AudioSource停止播放音效
        /// </summary>
        /// <param name="audioObj"></param>
        /// <param name="audioClipName"></param>
        public void StopSoundByAudioSource(GameObject audioObj, AudioSource audioSource)
        {

            if (mAudioSourceDic.ContainsKey(audioObj))
            {
                //先把播放结束的停掉
                for (int i = mAudioSourceDic[audioObj].Count - 1; i >= 0; i--)
                {
                    Debug.Log("当前的名字：" + mAudioSourceDic[audioObj][i].clip.name + "   长度" + mAudioSourceDic[audioObj].Count);
                    if (audioSource != null && mAudioSourceDic[audioObj][i] == audioSource)
                    {
                        if (mAudioSourceDic[audioObj][i].isPlaying)
                        {
                            mAudioSourceDic[audioObj][i].Stop();
                            Destroy(mAudioSourceDic[audioObj][i]);
                            mAudioSourceDic[audioObj].Remove(mAudioSourceDic[audioObj][i]);
                        }
                    }
                    else
                    {
                        if (!mAudioSourceDic[audioObj][i].isPlaying)
                        {
                            Destroy(mAudioSourceDic[audioObj][i]);
                            mAudioSourceDic[audioObj].Remove(mAudioSourceDic[audioObj][i]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 停止gameobject上对应名字的音效
        /// </summary>
        /// <param name="audioObj"></param>
        /// <param name="audioClipName"></param>
        public void StopSoundByAudioClipName(GameObject audioObj, string audioClipName)
        {
            if (mAudioSourceDic.ContainsKey(audioObj))
            {
                //先把播放结束的停掉
                for (int i = mAudioSourceDic[audioObj].Count - 1; i >= 0; i--)
                {
                    Debug.Log("当前的名字：" + mAudioSourceDic[audioObj][i].clip.name + "   长度" + mAudioSourceDic[audioObj].Count);
                    if (mAudioSourceDic[audioObj][i].clip.name.Equals(audioClipName))
                    {
                        if (mAudioSourceDic[audioObj][i].isPlaying)
                        {
                            mAudioSourceDic[audioObj][i].Stop();
                            Destroy(mAudioSourceDic[audioObj][i]);
                            mAudioSourceDic[audioObj].Remove(mAudioSourceDic[audioObj][i]);
                        }
                    }
                    else
                    {
                        if (!mAudioSourceDic[audioObj][i].isPlaying)
                        {
                            Destroy(mAudioSourceDic[audioObj][i]);
                            mAudioSourceDic[audioObj].Remove(mAudioSourceDic[audioObj][i]);
                        }
                    }
                }
            }

        }
        /// <summary>
        ///  停暂停gameobject上对应名字的音效
        /// </summary>
        /// <param name="audioObj"></param>
        /// <param name="audioClipName"></param>

        public void PauseSound(GameObject audioObj, string audioClipName)
        {
            if (mAudioSourceDic.ContainsKey(audioObj))
            {
                //先把播放结束的停掉
                for (int i = mAudioSourceDic[audioObj].Count - 1; i >= 0; i--)
                {
                    Debug.Log("当前的名字：" + mAudioSourceDic[audioObj][i].clip.name + "   长度" + mAudioSourceDic[audioObj].Count);
                    if (mAudioSourceDic[audioObj][i].clip.name.Equals(audioClipName))
                    {
                        if (mAudioSourceDic[audioObj][i].isPlaying)
                        {
                            mAudioSourceDic[audioObj][i].Pause();

                        }
                    }

                }
            }

        }
        /// <summary>
        /// 关闭场景中所有声音
        /// </summary>
        public void StopAllSound()
        {
            if (mBgAudioSource != null && mBgAudioSource.isPlaying)
            {
                mBgAudioSource.Stop();
            }

            foreach (var item in mAudioSourceDic.Keys)
            {
                for (int i = mAudioSourceDic[item].Count - 1; i >= 0; i--)
                {
                    if (mAudioSourceDic[item][i].isPlaying)
                    {
                        mAudioSourceDic[item][i].Stop();
                        Destroy(mAudioSourceDic[item][i]);
                        mAudioSourceDic[item].Remove(mAudioSourceDic[item][i]);
                    }
                }
            }

        }
    #region AudioSource 设置

    /// <summary>
    /// 所有音量禁音
    /// </summary>
    public void VolumeSilence() {
        if (mBgAudioSource!=null) {
            mBgAudioSource.volume = 0;
            AudioSetting.volume = 0;
        }
    }
    /// <summary>
    /// 所有音效禁音
    /// </summary>

    public void SoundSilence()
    {
        List<AudioSource>[] audioSources = mAudioSourceDic.Values.ToArray();
        AudioSetting.sound = 0;
        for (int i = 0; i < audioSources.Length; i++)
        {
            for (int j = 0; j < audioSources[i].Count; j++)
            {
                audioSources[i][j].volume = 0;
            }
        }
    }
    /// <summary>
    /// 设置所有音频音量大小
    /// </summary>
    public void SettingAllVolumeValue(float value) {
        if (mBgAudioSource != null)
        {
            mBgAudioSource.volume = value;
            AudioSetting.volume = value;
        }
    }
    /// <summary>
    /// 设置所有音频源音效大小
    /// </summary>
    public void SettingAllSoundValue(float value)
    {
        List<AudioSource>[] audioSources = mAudioSourceDic.Values.ToArray();
        AudioSetting.sound = value;
        for (int i = 0; i < audioSources.Length; i++)
        {
            for (int j = 0; j < audioSources[i].Count; j++)
            {
                audioSources[i][j].volume = value;
            }
        }
    }

    /// <summary>
    /// 设置当前音频源音量大小
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="volumeValue"></param>
    public void SettingVolumeValue(AudioSource audioSource, float volumeValue)
    {
        AudioSetting.volume = volumeValue;
        audioSource.volume = AudioSetting.volume;
    }
    /// <summary>
    /// 设置当前音频源音量大小
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="soundValue"></param>

    public  void SettingSoundValue(AudioSource audioSource, float soundValue)
    {
        AudioSetting.sound = soundValue;
        audioSource.volume = AudioSetting.sound;
    }
    /// <summary>
    /// 设置默认音量大小
    /// </summary>
    /// <param name="audioSource"></param>
    public  void SettingDefaultVolumeValue(AudioSource audioSource)
    {
        audioSource.volume = AudioSetting.volume;
    }

    /// <summary>
    /// 设置默认音效音量大小
    /// </summary>
    /// <param name="audioSource"></param>
    public  void SettingDefaultSoundValue(AudioSource audioSource)
    {
        audioSource.volume = AudioSetting.sound;
    }

    /// <summary>
    /// 是否循环播放
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="isLoop"></param>
    public  void IsLoop(AudioSource audioSource, bool isLoop)
    {
        audioSource.loop = isLoop;
    }

# endregion
}

