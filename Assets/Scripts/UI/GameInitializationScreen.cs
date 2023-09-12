using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInitializationScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI count;
    [SerializeField]
    private CanvasGroup screen;

    private void Awake()
    {
        if (GameInitializer.IsInstanced)
        {
            GameInitializer.Instance.CurrentTimeToStartMatch.ValueChanged += UpdateTimeToStartMatchDisplay;
        }
        else
        {
            GameInitializer.OnInstantiate += (o, _) =>
            {
                GameInitializer.Instance.CurrentTimeToStartMatch.ValueChanged += UpdateTimeToStartMatchDisplay;
            };
        }

        if (GameStateManager.IsInstanced)
        {
            ProcessScreenViewAtStartOfTheGame();
        }
        else
        {
            GameStateManager.OnInstantiate += (_, _) => ProcessScreenViewAtStartOfTheGame();
        }
    }

    private void ProcessScreenViewAtStartOfTheGame()
    {
        SetScreenDisplayBasedOnCurrentGameState(-1, GameStateManager.Instance.CurrentGameState.Value);
        GameStateManager.Instance.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
    }

    private void UpdateTimeToStartMatchDisplay(float lastValue, float newValue)
    {
        count.text = Mathf.Ceil(newValue).ToString();
    }

    private void SetScreenDisplayBasedOnCurrentGameState(int lastGameState, int newGameState)
    {
        if (screen != null)
        {
            screen.alpha = (GameStateManager.GameState)newGameState == GameStateManager.GameState.Prematch ? 1.0f : 0.0f;
        }
    }
}
