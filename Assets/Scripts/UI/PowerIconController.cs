using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerIconController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float fadeDuration;
    [SerializeField]
    private Vector3 offset;

    [Header("References")]
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image iconFillImage;

    public event System.EventHandler OnHideEvent;

    private Transform followedTransform;

    private bool isShown = false;

    private bool powerState => powerStateFunc();
    private System.Func<bool> powerStateFunc = null;

    public void ShowIcon(Transform transform, System.Func<bool> powerState)
    {
        followedTransform = transform;
        isShown = true;
        powerStateFunc = powerState;
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

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(followedTransform.position + offset);

        transform.position = new Vector3(screenPoint.x, screenPoint.y, transform.position.z);

        iconFillImage.enabled = powerState;
    }
}
