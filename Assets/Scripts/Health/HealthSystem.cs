using System;
using UnityEngine;

/// <summary>
///   <para> Independent class to manage Health (enemy, player).</para>
///   <para> Use System.Event to update health. </para>
/// </summary>
public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDeath;
    public event EventHandler OnShowBar;
    
    private int _health;
    private int _healthMax;
    
    // CONSTRUCTOR
    public HealthSystem(int healthMax)
    {
        _healthMax = healthMax;
        _health = healthMax;
    }

    // GETTER
    public int GetHealth()
    {
        return _health;
    }

    public float GetHealthPercent()
    {
        return (float)_health / _healthMax;
    }

    // SETTER
    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0)
        {
            _health = 0;  // one cannot have negative health
            if (OnDeath != null) OnDeath(this, EventArgs.Empty);
        }
        // if there is subscriber to this event, trigger it.
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;
        if (_health > _healthMax) _health = _healthMax;
        // if there is subscriber to this event, trigger it.
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void BarOnOff()
    {
        if (OnShowBar != null) OnShowBar(this, EventArgs.Empty);
    }
}