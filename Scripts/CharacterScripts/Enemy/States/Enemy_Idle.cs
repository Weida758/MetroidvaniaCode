using UnityEngine;
public class Enemy_Idle : Enemy_GroundedState
{
    public Enemy_Idle(StateMachine stateMachine, string animBoolName, Enemy enemy) :
        base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.movementComponent.idleTimer;
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            if (enemy.movementComponent != null)
            {
                stateMachine.ChangeState(enemy.moveState);
            }

        }
    }

}
