using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System.Linq;

public class GameManager : ElympicsSingleton<GameManager>, IInitializable
{
    [SerializeField]
    private List<Receiver> winReceivers = new List<Receiver>();

    public ElympicsInt WinnerPlayerId { private set; get; } = new ElympicsInt(-1);
    public int WinReceiversCount => winReceivers.Count;

    private HashSet<PlayerData> alivePlayers = new HashSet<PlayerData>();
    private Dictionary<int, HashSet<Receiver>> playerScores = new Dictionary<int, HashSet<Receiver>>();

    public void Initialize()
    {
        if (ClientProvider.Instance.IsInitialized)
        {
            SetupGame();
        }
        else
        {
            ClientProvider.Instance.OnInitializeEvent += (_, _) => SetupGame();
        }
    }

    private void SetupGame()
    {
        foreach (PlayerData player in ClientProvider.Instance.ClientPlayers)
        {
            alivePlayers.Add(player);
            print(player.Name);
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.IsDeadChangedEvent += (v) =>
            {
                if (v)
                {
                    HandleDeath(player);
                }
            };
            playerScores[player.PlayerId] = new HashSet<Receiver>();
        }

        foreach (Receiver receiver in winReceivers)
        {
            receiver.ActivateEvent += IncrementScore;
            receiver.DeactivateEvent += DecrementScore;
        }
    }

    private void HandleDeath(PlayerData playerData)
    {
        alivePlayers.Remove(playerData);
        playerData.gameObject.SetActive(false);
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (!Elympics.IsServer)
        {
            return;
        }

        var maxScore = FindMaxScore();

        if (alivePlayers.Count <= 1)
        {
            WinnerPlayerId.Value = alivePlayers.Single().PlayerId;
        }
        else if (winReceivers.Count > 0 && maxScore.Value.Count >= winReceivers.Count)
        {
            WinnerPlayerId.Value = maxScore.Key;
        }
    }

    private KeyValuePair<int, HashSet<Receiver>> FindMaxScore()
    {
        KeyValuePair<int, HashSet<Receiver>> result = playerScores.First();
        foreach (var score in playerScores)
        {
            if (score.Value.Count > result.Value.Count)
            {
                result = score;
            }
        }
        return result;
    }

    public void IncrementScore(Receiver receiver, int playerId)
    {
        if (!Elympics.IsServer || !playerScores.ContainsKey(playerId))
        {
            return;
        }

        playerScores[playerId].Add(receiver);
        PlayerSpawner.Instance.GetPlayerScoreById(playerId).Score.Value = playerScores[playerId].Count;
        CheckForWin();
    }

    public void DecrementScore(Receiver receiver, int playerId)
    {
        if (!Elympics.IsServer || !playerScores.ContainsKey(playerId))
        {
            return;
        }

        playerScores[playerId].Remove(receiver);
        PlayerSpawner.Instance.GetPlayerScoreById(playerId).Score.Value = playerScores[playerId].Count;
    }
}
