using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private List<SlicedFilledImage> images = new List<SlicedFilledImage>();

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
        foreach (SlicedFilledImage image in images)
        {
            float t = current / maxVal;
            image.fillAmount = t;

            if (animateColor)
            {
                image.color = gradient.Evaluate(1 - t);
            }
        }
    }

    public void ImageSetEnabled(int index, bool enabled)
    {
        if (index > images.Count || index < 0)
        {
            return;
        }
        images[index].enabled = enabled;
    }
}
