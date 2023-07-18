using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject
{
    public enum PlayerColor { Blue, Green, Yellow, Pink, None};

    public static Color PlayerColorToColor(PlayerColor playerColor)
    {
        switch (playerColor)
        {
            case PlayerColor.Blue:
                return Color.blue;
            case PlayerColor.Green:
                return Color.green;
            case PlayerColor.Yellow:
                return Color.yellow;
            case PlayerColor.Pink:
                return Color.magenta;
            default:
                return Color.red;
        }
    }
}
