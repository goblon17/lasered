using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using TMPro;
using UnityEngine.UI;

public class PlayerScore : ElympicsMonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private PlayerData.PlayerColor color;
    [SerializeField]
    private Image playerImage;

    public int PlayerId => ((int)color) - 1;
    public ElympicsInt Score { private set; get; } = new ElympicsInt(0);

    private void Awake()
    {
        Score.ValueChanged += OnValueChanged;
        playerImage.color = PlayerData.PlayerColorToColor(color);
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
        text.text = $"{newValue}/{GameManager.Instance.WinReceiversCount}";
    }
}
