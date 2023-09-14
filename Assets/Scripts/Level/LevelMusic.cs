using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private float startingVolume;

    private void Awake()
    {
        startingVolume = audioSource.volume;
        SoundSettings.Instance.VolumeChangedEvent += OnVolumeChanged;
        OnVolumeChanged(SoundSettings.SoundType.Music);
    }

    private void OnDestroy()
    {
        SoundSettings.Instance.VolumeChangedEvent -= OnVolumeChanged;
    }

    private void OnVolumeChanged(SoundSettings.SoundType soundType)
    {
        if (soundType != SoundSettings.SoundType.Music && soundType != SoundSettings.SoundType.General)
        {
            return;
        }

        audioSource.volume = startingVolume * SoundSettings.Instance.MusicVolumeMultiplier;
    }
}
