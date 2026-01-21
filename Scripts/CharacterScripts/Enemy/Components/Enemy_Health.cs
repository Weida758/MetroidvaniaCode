using UnityEngine;

public class Enemy_Health : Health
{
    private Enemy enemy;
    
    
    public override void TakeDamage(float damage, Transform damager)
    {
        if (damager.GetComponent<Player>() != null)
        {
            enemy.EnterBattleState(damager);
        }
        base.TakeDamage(damage,  damager);
    }
    
}
