using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System.Linq;

public class GameManager : ElympicsSingleton<GameManager>, IInitializable
{
    [SerializeField]
    private List<Receiver> winReceivers = new List<Receiver>();

    private HashSet<PlayerData> alivePlayers = new HashSet<PlayerData>();

    public ElympicsInt WinnerPlayerId { private set; get; } = new ElympicsInt(-1);

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

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
            playerScores[player.PlayerId] = 0;
        }

        foreach (Receiver receiver in winReceivers)
        {
            receiver.ActivateEvent.AddListener(IncrementScore);
            receiver.DeactivateEvent.AddListener(DecrementScore);
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
        else if (winReceivers.Count > 0 && maxScore.Value >= winReceivers.Count)
        {
            WinnerPlayerId.Value = maxScore.Key;
        }
    }

    private KeyValuePair<int, int> FindMaxScore()
    {
        KeyValuePair<int, int> result = playerScores.First();
        foreach (var score in playerScores)
        {
            if (score.Value > result.Value)
            {
                result = score;
            }
        }
        return result;
    }

    public void IncrementScore(int playerId)
    {
        if (!Elympics.IsServer || !playerScores.ContainsKey(playerId))
        {
            return;
        }

        playerScores[playerId]++;
        CheckForWin();
    }

    public void DecrementScore(int playerId)
    {
        if (!Elympics.IsServer || !playerScores.ContainsKey(playerId))
        {
            return;
        }

        playerScores[playerId]--;
    }
}
