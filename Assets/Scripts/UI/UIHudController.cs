using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudController : Singleton<UIHudController>
{
    [SerializeField]
    private UIBarController healthBar;
    [SerializeField]
    private UICursorController cursorController;

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
        playerData.GetComponent<PlayerHealth>().HealthChangedEvent += healthBar.ChangeValue;
        //healthBar.ChangeValue();

        playerData.GetComponent<PlayerAimer>().AimDirectionChangedEvent += cursorController.SetAimDirection;
    }
}
