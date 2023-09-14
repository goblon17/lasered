using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Collection/Looping")]
public class LoopingSoundCollection : SoundCollection
{
    [SerializeField]
    private List<AudioClip> begin;
    [SerializeField]
    private List<AudioClip> loop;
    [SerializeField]
    private List<AudioClip> finish;

    public override void Play(AudioSource audioSource, Vector3? position = null)
    {
        if (position != null)
        {
            Debug.LogError("Tried to play looping sound at position. This is not allowed!");
            return;
        }

        audioSource.Stop();

        AudioClip beginClip = begin[Random.Range(0, begin.Count)];
        AudioClip loopClip = loop[Random.Range(0, loop.Count)];
        float calculatedVolume = volume.GetRandomBetween() * SoundSettings.Instance.GetVolumeMultiplier(soundType);

        audioSource.clip = loopClip;
        audioSource.loop = true;

        audioSource.PlayOneShot(beginClip, calculatedVolume);
        audioSource.PlayDelayed(beginClip.length);
        
    }

    public override void Finish(AudioSource audioSource)
    {
        AudioClip finishClip = finish[Random.Range(0, finish.Count)];
        float calculatedVolume = volume.GetRandomBetween() * SoundSettings.Instance.GetVolumeMultiplier(soundType);

        audioSource.Stop();
        audioSource.PlayOneShot(finishClip, calculatedVolume);
    }
}
