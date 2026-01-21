using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_Idle(stateMachine, "idle", this);
        moveState = new Enemy_Move(stateMachine, "move", this);
        attackState = new Enemy_AttackState(stateMachine, "attack", this);
        battleState = new Enemy_BattleState(stateMachine, "battle", this);
        movementComponent = GetComponent<EnemyMovementComponent>();
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);
    }
    
}