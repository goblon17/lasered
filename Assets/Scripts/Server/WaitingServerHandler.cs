using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.Assertions;
using System.Linq;

public class WaitingServerHandler : ElympicsSingleton<WaitingServerHandler>, IServerHandlerGuid, IUpdatable
{
    [SerializeField] private float timeForPlayersToConnect = 30f;
    [SerializeField] private float connectingTimeoutCheckDelta = 5f;
    [SerializeField] private bool shouldGameEndAfterAnyDisconnect = false;

    private readonly HashSet<ElympicsPlayer> _playersConnected = new HashSet<ElympicsPlayer>();
    private int _playersNumber;
    private int _humanPlayersNumber;

    private bool _gameCancelled = false;
    private bool _isReadyLocally = false;
    private readonly ElympicsBool _gameReady = new ElympicsBool(false);

    public event System.Action OnGameReady;
    public bool IsGameReady => _gameReady.Value;

    protected override void Awake()
    {
        base.Awake();
        OnGameReady += () => Debug.Log("All players are ready");
    }

    public void OnServerInit(InitialMatchPlayerDatasGuid initialMatchPlayerDatas)
    {
        if (!IsEnabledAndActive)
            return;

        // Ensure initial synchronization variables are correct
        Assert.IsFalse(timeForPlayersToConnect < 0f || connectingTimeoutCheckDelta < 0f || connectingTimeoutCheckDelta > timeForPlayersToConnect);

        _playersNumber = initialMatchPlayerDatas.Count;
        _humanPlayersNumber = initialMatchPlayerDatas.Count(x => !x.IsBot);
        Debug.Log($"Game initialized with {_humanPlayersNumber} human players and {initialMatchPlayerDatas.Count - _humanPlayersNumber} bots");

        StartCoroutine(WaitForClientsToConnect());
    }

    private IEnumerator WaitForClientsToConnect()
    {
        System.DateTime waitForPlayersFinishTime = System.DateTime.Now + System.TimeSpan.FromSeconds(timeForPlayersToConnect);

        while (System.DateTime.Now < waitForPlayersFinishTime && !GameStateAlreadyDetermined)
        {
            Debug.Log("Waiting for all players to connect...");
            yield return new WaitForSeconds(connectingTimeoutCheckDelta);
        }

        if (GameStateAlreadyDetermined)
            yield break;

        EndGameForcefully("Not all players have connected, therefore the game cannot start and so it ends");
    }

    public void OnPlayerDisconnected(ElympicsPlayer player)
    {
        if (!IsEnabledAndActive)
            return;

        Debug.Log($"Player {player} disconnected");
        _playersConnected.Remove(player);
        if (ClientProvider.Instance != null && ClientProvider.Instance.IsInitialized)
        {
            PlayerData data = ClientProvider.Instance.GetPlayer(player);
            if (data != null)
            {
                data.GetComponent<PlayerHealth>().Kill();
            }
        }

        if (_gameCancelled)
            return;

        if (shouldGameEndAfterAnyDisconnect || NoHumanPlayersInGame)
            EndGameForcefully("Therefore, the game has ended");
    }


    public void OnPlayerConnected(ElympicsPlayer player)
    {
        if (!IsEnabledAndActive)
            return;

        Debug.Log($"Player {player} connected");
        _playersConnected.Add(player);
        if (PlayerSpawner.Instance != null && !GameStateAlreadyDetermined)
        {
            PlayerSpawner.Instance.SpawnPlayer(player);
        }

        if (NotAllPlayersConnected || GameStateAlreadyDetermined)
            return;

        BeginTheGame();
    }

    private bool GameStateAlreadyDetermined => _gameReady.Value || _gameCancelled;
    private bool NoHumanPlayersInGame => _playersConnected.Count == _playersNumber - _humanPlayersNumber;
    private bool NotAllPlayersConnected => _playersConnected.Count != _playersNumber;

    private void BeginTheGame()
    {
        _gameReady.Value = true;
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.StartGame();
        }
    }

    private void EndGameForcefully(string message)
    {
        _gameCancelled = true;
        Debug.Log(message);
        Elympics.EndGame();
    }

    public void ElympicsUpdate()
    {
        if (_isReadyLocally)
            return;

        if (!_gameReady.Value)
            return;

        OnGameReady?.Invoke();
        _isReadyLocally = true;
    }
}
