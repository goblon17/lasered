using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Elympics;

public class GameStateManager : ElympicsSingleton<GameStateManager>, IUpdatable
{
    public enum GameState
    {
        Prematch,
        GameplayMatchRunning,
        MatchEnded
    }

    public ElympicsBool ShouldStart { get; private set; } = new ElympicsBool(false);
    public ElympicsInt CurrentGameState { get; private set; } = new ElympicsInt((int)GameState.Prematch);

    private void Start()
    {
        ClientProvider.Instance.OnInitializeEvent += (_, _) => GameInitializer.Instance.InitializeMatch(() => ChangeGameState(GameState.GameplayMatchRunning));
        ShouldStart.ValueChanged += (_, v) =>
        {
            if (v)
            {
                ClientProvider.Instance.Initialize();
            }
        };
    }

    public void StartGame()
    {
        ShouldStart.Value = true;
    }

    private void ChangeGameState(GameState newGameState)
    {
        CurrentGameState.Value = (int)newGameState;
    }

    public void ElympicsUpdate()
    {
        if (GameManager.Instance.WinnerPlayerId.Value >= 0 && (GameState)CurrentGameState.Value == GameState.GameplayMatchRunning)
        {
            ChangeGameState(GameState.MatchEnded);
            Elympics.EndGame();
        }
    }
}
