using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System.Linq;

public class GameManager : ElympicsSingleton<GameManager>, IInitializable
{
    private HashSet<PlayerData> alivePlayers = new HashSet<PlayerData>();

    public ElympicsInt WinnerPlayerId { private set; get; } = new ElympicsInt(-1);

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
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.IsDeadChangedEvent += (v) =>
            {
                if (v)
                {
                    HandleDeath(player);
                }
            };
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

        if (alivePlayers.Count <= 1)
        {
            WinnerPlayerId.Value = alivePlayers.Single().PlayerId;
        }
    }
}
