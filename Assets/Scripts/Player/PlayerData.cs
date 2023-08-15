using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerData : ElympicsMonoBehaviour, IInitializable
{
    public enum PlayerColor { None, Blue, Green, Yellow, Pink};

    [SerializeField]
    private MeshRenderer meshRenderer;

    public int PlayerId { private set; get; }
    public ElympicsPlayer Player { private set; get; }
    public PlayerColor PlayerColorEnum { private set; get; }
    public Color Color => PlayerColorToColor(PlayerColorEnum);
    public int PlayerColorInt => (int)PlayerColorEnum;
    public string Name => PlayerColorEnum.ToString();

    public static string GetNameById(int playerId)
    {
        try
        {
            return ((PlayerColor)(playerId + 1)).ToString();
        }
        catch
        {
            return "None";
        }
    }

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
            case PlayerColor.None:
                return Color.red;
            default:
                return Color.red;
        }
    }

    public void Initialize()
    {
        Player = PredictableFor;
        PlayerId = (int)PredictableFor;
        if (PlayerId < 0)
        {
            PlayerColorEnum = PlayerColor.None;
        }
        else
        {
            PlayerColorEnum = (PlayerColor)(PlayerId + 1);
        }

        Material material = new Material(meshRenderer.material);
        material.color = Color;
        meshRenderer.material = material;
    }
}
