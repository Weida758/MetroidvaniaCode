using UnityEngine;
using System.Collections;
public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(StateMachine stateMachine, string animBoolName, Enemy enemy) : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //attackCooldown = enemy.attackCooldown;

    }
    public override void Update()
    {
        base.Update();
        //attackCooldown -= Time.deltaTime;
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.currentAttackCooldown = enemy.attackCooldown;
    }
    
}
