using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundCollection : ScriptableObject
{
    [SerializeField]
    protected SoundSettings.SoundType soundType;
    [SerializeField]
    [MinMaxRange(0, 1)]
    protected RangeBoundariesFloat volume;

    public abstract void Play(AudioSource audioSource, Vector3? position = null);

    public abstract void Finish(AudioSource audioSource);
}
