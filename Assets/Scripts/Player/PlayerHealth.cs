using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerHealth : ElympicsMonoBehaviour, IInitializable
{
    [SerializeField]
    private float maxHealth = 100;

    public event System.Action<float, float> HealthChangedEvent;

    private ElympicsFloat currentHealth = new ElympicsFloat(0);

    public void Initialize()
    {
        currentHealth.Value = 100;
        currentHealth.ValueChanged += (_, v) => HealthChangedEvent?.Invoke(v, maxHealth);
    }

    public bool TakeDamage(float damage)
    {
        if (!Elympics.IsServer)
        {
            return false;
        }

        currentHealth.Value -= damage;
        return currentHealth.Value <= 0;
    }
}
