using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerWire : MonoBehaviour
{
    [SerializeField]
    private List<WirePart> parts;

    [SerializeField]
    private Color offColor;
    [SerializeField]
    private Color onColor;

    [SerializeField]
    private bool invert;

    public void SetPower(bool power)
    {
        foreach (WirePart part in parts)
        {
            part.SetColor(power ^ invert ? onColor : offColor);
        }
    }
}
