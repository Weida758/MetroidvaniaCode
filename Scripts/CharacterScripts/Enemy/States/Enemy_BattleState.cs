using System.IO;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{

    private Transform playerTransform;
    private float lastTimeInBattle;
    public Enemy_BattleState(StateMachine stateMachine, string animBoolName, Enemy enemy) : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (playerTransform == null)
        {
            playerTransform = enemy.PlayerDetection().transform;
        }
        UpdateBattleTimer();
        //null check, if null, assign to enemy's reference of the player
        playerTransform ??= enemy.player;
        enemy.movementComponent.moveSpeed *= 2;
        Debug.Log("I Entered Battle State");
    }

    public override void Update()
    {
        base.Update();
        enemy.currentAttackCooldown -= Time.deltaTime;

        if (enemy.PlayerDetection())
        {
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
            Debug.Log("Battle Time Over");
            return;
        }
        
        if (WithinAttackRange() && enemy.facingDir == DirectionToPlayer() && playerTransform.position.y < enemy.spriteBoundY + 2)
        {
            if (enemy.currentAttackCooldown <= 0)
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (enemy.movementComponent && !enemy.wallDetected)
            {
                enemy.SetVelocity(enemy.movementComponent.moveSpeed * DirectionToPlayer(),
                    enemy.Rb.linearVelocity.y);
                Debug.Log("Moving in Battle State with: " + DirectionToPlayer());
            }
        }
    }
    
    private void UpdateBattleTimer() => lastTimeInBattle = Time.time;
    private bool BattleTimeIsOver() => Time.time > lastTimeInBattle + enemy.battleTimeDuration;

    public override void Exit()
    {
        base.Exit();

        enemy.movementComponent.moveSpeed /= 2;
    }
    private bool WithinAttackRange()
    {
        return DistanceToPlayer() < enemy.attackDistance.value;
    }
    private float DistanceToPlayer()
    {
        if (playerTransform == null)
        {
            return float.MaxValue;
        }

        return Mathf.Abs(playerTransform.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (playerTransform == null)
        {
            return 0;
        }

        if (Mathf.Abs(playerTransform.position.x - enemy.transform.position.x) <= 0.1f)
        {
            return 0;
        }

        return playerTransform.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
