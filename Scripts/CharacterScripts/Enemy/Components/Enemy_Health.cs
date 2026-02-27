using UnityEngine;

public class Enemy_Health : Health, IDamageable
{
    private Enemy enemy;

    protected override void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    
    public override void TakeDamage(float damage, Transform damager = null)
    {
        if (damager != null)
        {
            enemy.EnterBattleState(damager);
        }
        base.TakeDamage(damage, damager);
    }
    
}
