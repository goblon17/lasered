using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWire : MonoBehaviour
{
    [SerializeField]
    private List<WirePart> parts;

    public void SetColor(PlayerData.PlayerColor playerColor)
    {
        SetColor(PlayerData.PlayerColorToColor(playerColor));
    }

    public void SetColor(Color color)
    {
        foreach (WirePart part in parts)
        {
            part.SetColor(color);
        }
    }
}
