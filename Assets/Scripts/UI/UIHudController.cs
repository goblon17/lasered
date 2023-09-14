using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudController : Singleton<UIHudController>
{
    [SerializeField]
    private float damagedFadeDuration;
    [SerializeField]
    private float damagedScreenDuration;

    [SerializeField]
    private UIBarController healthBar;
    [SerializeField]
    private UICursorController cursorController;
    [SerializeField]
    private UIInteractionTooltip interactionTooltip;
    [SerializeField]
    private GameObject deathScreen;
    [SerializeField]
    private CanvasGroup damagedScreenCanvasGroup;
    [SerializeField]
    private SettingMenuController settingMenuController;

    public UIInteractionTooltip InteractionTooltip => interactionTooltip;
    public GameObject DeathScreen => deathScreen;

    private float previousHealth;
    private float alphaTarget = 0;
    private float damagedScreenEndTimestamp = 0;

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

        damagedScreenCanvasGroup.alpha = 0;
    }

    private void RegisterPlayer()
    {
        PlayerData playerData = ClientProvider.Instance.ClientPlayer;
        PlayerHealth playerHealth = playerData.GetComponent<PlayerHealth>();
        previousHealth = playerHealth.CurrentHealth;
        playerHealth.HealthChangedEvent += OnHealthChanged;
        healthBar.ChangeValue(1, 1);
        playerHealth.IsDeadChangedEvent += deathScreen.SetActive;
        deathScreen.SetActive(false);


        playerData.GetComponent<PlayerAimer>().AimDirectionChangedEvent += cursorController.SetAimDirection;
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        healthBar.ChangeValue(currentHealth, maxHealth);

        if (currentHealth < previousHealth)
        {
            alphaTarget = 1;
            damagedScreenEndTimestamp = Time.time + damagedFadeDuration + damagedScreenDuration;
        }
        previousHealth = currentHealth;
    }

    private void Update()
    {
        damagedScreenCanvasGroup.alpha = Mathf.MoveTowards(damagedScreenCanvasGroup.alpha, alphaTarget, (1 / damagedFadeDuration) * Time.deltaTime);

        if (Time.time > damagedScreenEndTimestamp)
        {
            alphaTarget = 0;
        }
    }

    public void ToggleSettings()
    {
        settingMenuController.Toggle();
    }
}
