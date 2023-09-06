using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuPlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Vector2 timeRange;
    [SerializeField]
    private List<string> triggers;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(AnimationCoroutine());
    }

    private IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSecondsRealtime(Random.Range(timeRange.x, timeRange.y));
        animator.SetTrigger(triggers[Random.Range(0, triggers.Count)]);
    }
}
