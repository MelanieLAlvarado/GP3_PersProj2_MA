using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate(float newHealth);
    public event OnHealthChangedDelegate OnHealthChanged;
    public event Action OnDead;

    [SerializeField] private float maxHealth = 100;
    private float _health;

    public float GetHealth() { return _health; }
    private void Awake()
    {
        _health = maxHealth;
    }
    public void ChangeHealth(float amt, GameObject instigator)
    {
        if (amt == 0 || _health == 0) { return; }

        _health = Mathf.Clamp(_health + amt, 0, maxHealth);
        OnHealthChanged?.Invoke(_health);

        if (_health <= 0)
        { 
            OnDead?.Invoke();
        }
    }
}
