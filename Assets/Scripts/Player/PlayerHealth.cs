using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerHealth : ElympicsMonoBehaviour, IInitializable
{
    [SerializeField]
    private float maxHealth = 100;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth.Value;
    public bool IsDead => isDead.Value;

    public event System.Action<float, float> HealthChangedEvent;
    public event System.Action<bool> IsDeadChangedEvent;

    private ElympicsFloat currentHealth = new ElympicsFloat(0, comparer: new ElympicsFloatEqualityComparer(0));
    private ElympicsBool isDead = new ElympicsBool(false);

    public void Initialize()
    {
        currentHealth.ValueChanged += (_, v) => HealthChangedEvent?.Invoke(v, maxHealth);
        currentHealth.Value = 100;
        isDead.ValueChanged += (_, v) => IsDeadChangedEvent?.Invoke(v);
        isDead.Value = false;
    }

    public bool TakeDamage(float damage)
    {
        if (!Elympics.IsServer)
        {
            return false;
        }

        currentHealth.Value -= damage;
        if (currentHealth.Value <= 0)
        {
            isDead.Value = true;
        }
        return currentHealth.Value <= 0;
    }
}
