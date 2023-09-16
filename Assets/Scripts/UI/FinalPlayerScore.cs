using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.UI;
using System.Linq;

public class FinalPlayerScore : ElympicsMonoBehaviour
{
    [SerializeField]
    private List<Image> scoreImages;
    [SerializeField]
    private PlayerData.PlayerColor color;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image playerAliveImage;
    [SerializeField]
    private Image playerDeadImage;
    [SerializeField]
    [Range(0, 1)]
    private float deadColorMultiplier;

    public int PlayerId => ((int)color) - 1;
    public ElympicsInt Score { private set; get; } = new ElympicsInt(0);
    public ElympicsBool Dead { private set; get; } = new ElympicsBool(false);

    private void Awake()
    {
        Score.ValueChanged += OnScoreValueChanged;
        Dead.ValueChanged += OnDeadValueChanged;
        Color colorValue = PlayerData.PlayerColorToColor(color);
        playerAliveImage.color = colorValue;
        playerDeadImage.color = new Color(colorValue.r * deadColorMultiplier, colorValue.g * deadColorMultiplier, colorValue.b * deadColorMultiplier, colorValue.a);

        colorValue.a = backgroundImage.color.a;
        backgroundImage.color = colorValue;
        if (GameManager.IsInstanced)
        {
            OnGameManagerInstantiate(null, null);
        }
        else
        {
            GameManager.OnInstantiate += OnGameManagerInstantiate;
        }
    }

    private void OnGameManagerInstantiate(object _, System.EventArgs __)
    {
        OnScoreValueChanged(0, 0);
        OnDeadValueChanged(false, false);

        GameManager.OnInstantiate -= OnGameManagerInstantiate;
    }

    private void OnScoreValueChanged(int oldValue, int newValue)
    {
        foreach (var (image, i) in scoreImages.Select((x, i) => (x, i)))
        {
            image.gameObject.SetActive(i < newValue);
        }
    }

    private void OnDeadValueChanged(bool oldValue, bool newValue)
    {
        playerAliveImage.gameObject.SetActive(!newValue);
        playerDeadImage.gameObject.SetActive(newValue);
    }
}
