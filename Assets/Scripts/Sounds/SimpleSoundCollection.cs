using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Collection/Simple")]
public class SimpleSoundCollection : SoundCollection
{
    [SerializeField]
    private List<AudioClip> clips;

    public override void Play(AudioSource audioSource, Vector3? position = null)
    {
        if (clips.Count == 0)
        {
            Debug.LogWarning($"No audioClips in {name} SoundCollection");
            return;
        }

        AudioClip clip = clips[Random.Range(0, clips.Count)];
        float calculatedVolume = volume.GetRandomBetween() * SoundSettings.Instance.GetVolumeMultiplier(soundType);

        if (position != null)
        {
            AudioSource.PlayClipAtPoint(clip, position.Value, calculatedVolume);
        }
        else
        {
            audioSource.PlayOneShot(clip, calculatedVolume);
        }
    }

    public override void Finish(AudioSource audioSource) { }
}
