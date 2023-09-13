using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundModule : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<string, SoundCollection> sounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string key, bool atPoint = false)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }
        if (!sounds.ContainsKey(key))
        {
            Debug.LogError($"{name}'s Sound Module doesn't contain {key} sound");
            return;
        }

        sounds[key].Play(audioSource, atPoint ? transform.position : null);
    }

    public void FinishSound(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }
        if (!sounds.ContainsKey(key))
        {
            Debug.LogError($"{name}'s Sound Module doesn't contain {key} sound");
            return;
        }

        sounds[key].Finish(audioSource);
    }
}
