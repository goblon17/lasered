using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerSpawner : ElympicsSingleton<PlayerSpawner>
{
    [SerializeField]
    private List<PlayerData> players;

    public void SpawnPlayer(ElympicsPlayer player)
    {
        PlayerData playerData = players.Find(x => x.Player == player);
        if (playerData != null)
        {
            playerData.gameObject.SetActive(true);
        }
    }
}
