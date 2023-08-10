using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerCanvasController : MonoBehaviour
{
    [SerializeField]
    private UIBarController healthBar;
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        playerHealth.HealthChangedEvent += (v, m) => healthBar.ChangeValue(v, m);
        healthBar.ChangeValue(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        if (ClientProvider.IsInstanced)
        {
            CheckForPlayer();
        }
        else
        {
            ClientProvider.OnInstantiate += (_, _) =>
            {
                CheckForPlayer();
            };
        }
    }

    private void CheckForPlayer()
    {
        if (ClientProvider.Instance.IsInitialized)
        {
            DisableHealthBar();
        }
        else
        {
            ClientProvider.Instance.OnInitializeEvent += (_, _) =>
            {
                DisableHealthBar();
            };
        }
    }

    private void DisableHealthBar()
    {
        if (ClientProvider.Instance.ClientPlayer == playerData)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
}
