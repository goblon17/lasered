using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static Coroutine CallDelayed(this MonoBehaviour monoBehaviour, float delayTime, System.Action functionToCall)
    {
        return monoBehaviour.StartCoroutine(CallCoroutine(delayTime, functionToCall));
    }

    private static IEnumerator CallCoroutine(float delayTime, System.Action delayedFunction)
    {
        yield return new WaitForSeconds(delayTime);

        delayedFunction?.Invoke();
    }
}
