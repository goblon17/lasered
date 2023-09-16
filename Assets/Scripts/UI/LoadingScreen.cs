using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private List<string> tips;
    [SerializeField]
    private float tipShowDuration;
    [SerializeField]
    private float tipFadeDuration;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI tipText;

    private Coroutine coroutine = null;
    private int index = 0;

    public void Show(string msg)
    {
        text.text = msg;
        gameObject.SetActive(true);
        index = Random.Range(0, tips.Count);
        tipText.text = tips[index];
        tipText.color = PlayerData.PlayerColorToColor((PlayerData.PlayerColor)Random.Range(0, System.Enum.GetValues(typeof(PlayerData.PlayerColor)).Length));
        StartCoroutine(TipCoroutine());
    }

    private IEnumerator TipCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(tipShowDuration);

            yield return UIUtils.TextFadeCoroutine(tipText, tipFadeDuration, UIUtils.Fade.FadeOut);

            index++;
            index %= tips.Count;
            tipText.text = tips[index];
            tipText.color = PlayerData.PlayerColorToColor((PlayerData.PlayerColor)Random.Range(0, System.Enum.GetValues(typeof(PlayerData.PlayerColor)).Length));

            yield return UIUtils.TextFadeCoroutine(tipText, tipFadeDuration, UIUtils.Fade.FadeIn);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
