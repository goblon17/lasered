using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class GameStateManager : ElympicsSingleton<GameStateManager>, IInitializable, IUpdatable
{
    public enum GameState
    {
        Prematch,
        GameplayMatchRunning,
        MatchEnded
    }

    public ElympicsInt CurrentGameState { get; private set; } = new ElympicsInt((int)GameState.Prematch);

    public void Initialize()
    {
        GameInitializer.Instance.InitializeMatch(() => ChangeGameState(GameState.GameplayMatchRunning));
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
        }
    }
}
