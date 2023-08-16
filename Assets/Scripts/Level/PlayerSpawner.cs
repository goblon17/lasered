using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerSpawner : ElympicsSingleton<PlayerSpawner>
{
    [SerializeField]
    private List<PlayerData> players;
    [SerializeField]
    private List<PlayerScore> scores;

    public PlayerScore GetPlayerScoreById(int playerId)
    {
        return scores.Find(x => x.PlayerId == playerId);
    }

    public void SpawnPlayer(ElympicsPlayer player)
    {
        PlayerData playerData = players.Find(x => x.Player == player);
        if (playerData != null)
        {
            playerData.gameObject.SetActive(true);
        }
        PlayerScore playerScore = scores.Find(x => ElympicsPlayer.FromIndex(x.PlayerId) == player);
        if (playerScore != null)
        {
            playerScore.gameObject.SetActive(true);
        }
    }
}
