using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudController : Singleton<UIHudController>
{
    [SerializeField]
    private UIBarController healthBar;

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
        ClientProvider.Instance.ClientPlayer.GetComponent<PlayerHealth>().HealthChangedEvent += healthBar.ChangeValue;
        //healthBar.ChangeValue();
    }
}
