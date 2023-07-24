using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class UIUtils
{
    public enum Fade { FadeIn, FadeOut }

    public static IEnumerator ChangeLocalScaleCoroutine(GameObject gameObject, Vector3 targetScale, float duration, Func<bool> additionalConditions = null)
    {
        additionalConditions ??= () => true;

        Vector3 beginningScale = gameObject.transform.localScale;
        float timer = 0;
        while (timer < duration && additionalConditions())
        {
            yield return null;
            timer += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(beginningScale, targetScale, timer / duration);
        }
        if (additionalConditions())
        {
            gameObject.transform.localScale = targetScale;
        }
    }

    public static IEnumerator TextFadeCoroutine(TextMeshProUGUI text, float duration, Fade fadeDirection, Func<bool> additionalConditions = null)
    {
        additionalConditions ??= () => true;

        float counter = 0;
        float start = fadeDirection == Fade.FadeIn ? 0 : 1;
        float end = fadeDirection == Fade.FadeIn ? 1 : 0;

        if (!text.gameObject.activeSelf)
        {
            text.gameObject.SetActive(true);
        }

        Color vertexColor = text.color;

        while (counter < duration && additionalConditions())
        {
            counter += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(start, end, counter / duration);

            text.color = new Color(vertexColor.r, vertexColor.g, vertexColor.b, alpha);

            yield return null;
        }

        if (fadeDirection == Fade.FadeOut && additionalConditions())
        {
            text.gameObject.SetActive(false);
        }
    }

    public static IEnumerator CanvasGroupFadeCoroutine(CanvasGroup canvasGroup, float duration, Fade fadeDirection, Func<bool> additionalConditions = null, bool disableGameObject = true)
    {
        additionalConditions ??= () => true;

        float counter = 0;
        float start = fadeDirection == Fade.FadeIn ? 0 : 1;
        float end = fadeDirection == Fade.FadeIn ? 1 : 0;

        if (!canvasGroup.gameObject.activeSelf)
        {
            canvasGroup.gameObject.SetActive(true);
        }

        while (counter < duration && additionalConditions())
        {
            counter += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(start, end, counter / duration);

            canvasGroup.alpha = alpha;

            yield return null;
        }

        if (fadeDirection == Fade.FadeOut && additionalConditions() && disableGameObject)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
