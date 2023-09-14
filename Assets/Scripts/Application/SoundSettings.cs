using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Settings", menuName = "Game Settings/Sound Settings")]
public class SoundSettings : ScriptableObjectSingleton<SoundSettings>
{
    public enum SoundType { General, Sfx, Music };

    [SerializeField]
    [Range(0, 1)]
    private float generalSoundVolume;
    [SerializeField]
    [Range(0, 1)]
    private float sfxSoundVolume;
    [SerializeField]
    [Range(0, 1)]
    private float musicSoundVolume;

    public float MainVolume
    {
        get
        {
            return generalSoundVolume;
        }
        set
        {
            generalSoundVolume = value;
            VolumeChangedEvent?.Invoke(SoundType.General);
        }
    }

    public float SfxVolume
    {
        get
        {
            return sfxSoundVolume;
        }
        set
        {
            sfxSoundVolume = value;
            VolumeChangedEvent?.Invoke(SoundType.Sfx);
        }
    }

    public float MusicVolume
    {
        get
        {
            return musicSoundVolume;
        }
        set
        {
            musicSoundVolume = value;
            VolumeChangedEvent?.Invoke(SoundType.Music);
        }
    }

    public event System.Action<SoundType> VolumeChangedEvent;

    public float SfxVolumeMultiplier => generalSoundVolume * sfxSoundVolume;
    public float MusicVolumeMultiplier => generalSoundVolume * musicSoundVolume;

    public float GetVolumeMultiplier(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Sfx:
                return SfxVolumeMultiplier;
            case SoundType.Music:
                return MusicVolumeMultiplier;
            default:
                return generalSoundVolume;
        }
    }
}