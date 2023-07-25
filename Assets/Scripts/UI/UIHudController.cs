using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudController : Singleton<UIHudController>
{
    [SerializeField]
    private UIBarController healthBar;
    [SerializeField]
    private UICursorController cursorController;
    [SerializeField]
    private UIInteractionTooltip interactionTooltip;
    [SerializeField]
    private GameObject deathScreen;

    public UIInteractionTooltip InteractionTooltip => interactionTooltip;
    public GameObject DeathScreen => deathScreen;

    private void Start()
    {
        if (ClientProvider.Instance.IsInitialized)
        {
            RegisterPlayer();
        }
        else
        {
            ClientProvider.Instance.OnInitializeEvent += (_, _) => RegisterPlayer();
        }
    }

    private void RegisterPlayer()
    {
        PlayerData playerData = ClientProvider.Instance.ClientPlayer;
        PlayerHealth playerHealth = playerData.GetComponent<PlayerHealth>();
        playerHealth.HealthChangedEvent += healthBar.ChangeValue;
        healthBar.ChangeValue(1, 1);
        playerHealth.IsDeadChangedEvent += (e) => deathScreen.SetActive(e);
        deathScreen.SetActive(false);


        playerData.GetComponent<PlayerAimer>().AimDirectionChangedEvent += cursorController.SetAimDirection;
    }
}
