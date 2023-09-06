using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.UI;
using System.Linq;

public class PlayerScore : ElympicsMonoBehaviour
{
    [SerializeField]
    private List<Image> scoreImages;
    [SerializeField]
    private PlayerData.PlayerColor color;
    [SerializeField]
    private Image playerImage;

    public int PlayerId => ((int)color) - 1;
    public ElympicsInt Score { private set; get; } = new ElympicsInt(0);

    private void Awake()
    {
        Score.ValueChanged += OnValueChanged;
        Color colorValue = PlayerData.PlayerColorToColor(color);
        colorValue.a = playerImage.color.a;
        playerImage.color = colorValue;
        if (GameManager.IsInstanced)
        {
            OnValueChanged(0, 0);
        }
        else
        {
            GameManager.OnInstantiate += (_, _) => OnValueChanged(0, 0);
        }
    }

    private void OnValueChanged(int oldValue, int newValue)
    {
        foreach (var (image, i) in scoreImages.Select((x, i) => (x, i)))
        {
            image.gameObject.SetActive(i < newValue);
        }
    }
}
