using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFadeController : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration;

    private CanvasGroup canvasGroup;
    private float target;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        target = canvasGroup.alpha;
    }

    private void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, (1 / fadeDuration) * Time.deltaTime);
    }

    public void SetTarget(float target)
    {
        this.target = target;
    }
}
