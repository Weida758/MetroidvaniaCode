using System;
using System.Collections;
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

        StartCoroutine(DamageEffect());
        Debug.Log("Taking Damage");
    }

    private IEnumerator DamageEffect()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.15f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        Debug.Log("returning to original color");
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
