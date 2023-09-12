using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class PlayerHealth : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float regenerationBuffer;
    [SerializeField]
    private float regenerationTime;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth.Value;
    public bool IsDead => isDead.Value;

    public event System.Action<float, float> HealthChangedEvent;
    public event System.Action<bool> IsDeadChangedEvent;

    private ElympicsFloat currentHealth = new ElympicsFloat(0, comparer: new ElympicsFloatEqualityComparer(0));
    private ElympicsBool isDead = new ElympicsBool(false);

    private float regenerationSpeed => maxHealth / regenerationTime;
    private float regenerationBufferCounter = 0;

    public void Initialize()
    {
        currentHealth.ValueChanged += (_, v) => HealthChangedEvent?.Invoke(v, maxHealth);
        currentHealth.Value = maxHealth;
        isDead.ValueChanged += (_, v) => IsDeadChangedEvent?.Invoke(v);
        isDead.Value = false;
    }

    public bool TakeDamage(float damage)
    {
        if (!Elympics.IsServer || damage <= 0)
        {
            return false;
        }

        currentHealth.Value -= damage;
        regenerationBufferCounter = 0;
        if (currentHealth.Value <= 0)
        {
            isDead.Value = true;
        }
        return currentHealth.Value <= 0;
    }

    public void Heal(float amount)
    {
        if (!Elympics.IsServer || amount <= 0 || IsDead)
        {
            return;
        }

        currentHealth.Value += amount;
        if (currentHealth.Value >= maxHealth)
        {
            currentHealth.Value = maxHealth;
        }
    }

    public void Kill()
    {
        if (!Elympics.IsServer)
        {
            return;
        }

        TakeDamage(maxHealth);
    }

    private void UpdateRegeneration()
    {
        if (!Elympics.IsServer)
        {
            return;
        }

        float dTime = Elympics.TickDuration;
        if (regenerationBufferCounter < regenerationBuffer)
        {
            regenerationBufferCounter += dTime;
        }
        else
        {
            Heal(regenerationSpeed * dTime);
        }
    }

    public void ElympicsUpdate()
    {
        UpdateRegeneration();
    }
}
