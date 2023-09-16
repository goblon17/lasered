using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerSpawner : ElympicsSingleton<PlayerSpawner>
{
    [System.Serializable]
    public class PlayerScores
    {
        [SerializeField]
        private PlayerScore playerScore;
        [SerializeField]
        private FinalPlayerScore finalPlayerScore;

        public PlayerScore PlayerScore => playerScore;
        public FinalPlayerScore FinalPlayerScore => finalPlayerScore;

        public void SetScore(int val)
        {
            playerScore.Score.Value = val;
            finalPlayerScore.Score.Value = val;
        }

        public void SetDeath(bool val)
        {
            finalPlayerScore.Dead.Value = val;
        }
    }

    [SerializeField]
    private List<PlayerData> players;
    [SerializeField]
    private List<PlayerScores> scores;

    public PlayerScores GetPlayerScoreById(int playerId)
    {
        return scores.Find(x => x.PlayerScore.PlayerId == playerId);
    }

    public void SpawnPlayer(ElympicsPlayer player)
    {
        PlayerData playerData = players.Find(x => x.Player == player);
        if (playerData != null)
        {
            playerData.gameObject.SetActive(true);
        }
        PlayerScores playerScores = scores.Find(x => ElympicsPlayer.FromIndex(x.PlayerScore.PlayerId) == player);
        if (playerScores != null)
        {
            playerScores.PlayerScore.gameObject.SetActive(true);
            playerScores.FinalPlayerScore.gameObject.SetActive(true);
        }
    }
}
