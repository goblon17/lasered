using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIconController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float fadeDuration;

    [Header("References")]
    [SerializeField]
    private CanvasGroup canvasGroup;

    public event System.EventHandler OnHideEvent;

    private Transform followedTransform;

    private bool isShown = false;

    public void ShowIcon(Transform transform)
    {
        followedTransform = transform;
        isShown = true;
        StartCoroutine(UIUtils.CanvasGroupFadeCoroutine(canvasGroup, fadeDuration, UIUtils.Fade.FadeIn, () => isShown));
    }

    public void HideIcon()
    {
        isShown = false;
        StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine()
    {
        yield return StartCoroutine(UIUtils.CanvasGroupFadeCoroutine(canvasGroup, fadeDuration, UIUtils.Fade.FadeOut, null, false));
        OnHideEvent?.Invoke(this, null);
    }

    private void LateUpdate()
    {
        if (followedTransform == null)
        {
            return;
        }

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(followedTransform.position);

        transform.position = new Vector3(screenPoint.x, screenPoint.y, transform.position.z);
    }
}
