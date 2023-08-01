using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System.Linq;

public class ClientProvider : ElympicsSingleton<ClientProvider>
{
    public PlayerData ClientPlayer { get; private set; }
    public List<PlayerData> ClientPlayers { get; private set; }

    public bool IsInitialized { get; private set; } = false;
    public event System.EventHandler OnInitializeEvent;

    public void Initialize()
    {
        ClientPlayers = FindObjectsOfType<PlayerData>().OrderBy(x => x.PlayerId).ToList();
        ClientPlayer = ClientPlayers.Find(x => x.Player == Elympics.Player);
        if (ClientPlayer == null)
        {
            ClientPlayer = ClientPlayers[0];
        }
        IsInitialized = true;
        OnInitializeEvent?.Invoke(this, null);
    }

    public PlayerData GetPlayer(int playerId)
    { 
        return ClientPlayers.FirstOrDefault(x => x.PlayerId == playerId);
    }

    public PlayerData GetPlayer(ElympicsPlayer elympicsPlayer)
    {
        return ClientPlayers.FirstOrDefault(x => x.Player == elympicsPlayer);
    }
}
