using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeBoundaries<T>
{
    public T Min;
    public T Max;
}

[System.Serializable]
public class RangeBoundariesFloat : RangeBoundaries<float>
{
    public float GetRandomBetween()
    {
        return Random.Range(Min, Max);
    }

    public bool IsBetween(float value)
    {
        return value > Min && value < Max;
    }
}
