using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerData : MonoBehaviour
{
    public enum PlayerColor { Blue, Green, Yellow, Pink, None};

    [SerializeField]
    private int playerId;
    public int PlayerId => playerId;
    public ElympicsPlayer Player => ElympicsPlayer.FromIndex(playerId);

    [SerializeField]
    private PlayerColor playerColor;
    public PlayerColor PlayerColorEnum => playerColor;
    public Color color => PlayerColorToColor(playerColor);

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
