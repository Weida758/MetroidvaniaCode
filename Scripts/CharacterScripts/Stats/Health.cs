using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth;
    protected float currentHealth { get; set; }
    protected Rigidbody2D rb;

   protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer = null)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, 100f);
        
        if (currentHealth <= 0f)
        {
            HandleDefeat();
        }
        Debug.Log("Taking Damage");
    }

    public virtual void Heal(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0f, 100f);
    }

    public virtual void HandleDefeat()
    {
        Debug.Log("Destroying: " + gameObject.name);
        Invoke(nameof(Defeat), 1f);
    }
    protected virtual void Defeat()
    {
        Destroy(gameObject);
    }
}
