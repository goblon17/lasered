using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private List<SlicedFilledImage> slicedFilledImages = new List<SlicedFilledImage>();
    [SerializeField]
    private List<Image> images = new List<Image>();

    [Header("Config")]
    [SerializeField]
    private bool animateColor;
    [SerializeField]
    private Gradient gradient;

    [Header("Debug")]
    [SerializeField]
    [Range(0, 1)]
    private float value;

    private void OnValidate()
    {
        ChangeValue(value, 1);
    }

    public void ChangeValueInverted(float current, float maxVal)
    {
        ChangeValue((maxVal - current), maxVal);
    }

    public void ChangeValue(float current, float maxVal)
    {
        foreach (SlicedFilledImage image in slicedFilledImages)
        {
            float t = current / maxVal;
            image.fillAmount = t;

            if (animateColor)
            {
                image.color = gradient.Evaluate(1 - t);
            }
        }

        foreach (Image image in images)
        {
            float t = current / maxVal;
            image.fillAmount = t;

            if (animateColor)
            {
                image.color = gradient.Evaluate(1 - t);
            }
        }
    }
}
