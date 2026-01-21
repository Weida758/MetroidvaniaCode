using UnityEngine;

public class Enemy_Move : Enemy_GroundedState

{
    public Enemy_Move(StateMachine stateMachine, string animBoolName, Enemy enemy) : 
        base(stateMachine, animBoolName, enemy) { }

    public override void Enter()
    {
        base.Enter();
        enemy.Flip();

    }

    public override void Update()
    {
        base.Update();
        
        enemy.movementComponent.SetVelocity(enemy.movementComponent.moveSpeed * enemy.facingDir, enemy.movementComponent.rb.linearVelocity.y);

        if (!enemy.groundDetected)
        {
            enemy.movementComponent.SetVelocity(0, enemy.Rb.linearVelocity.y);
            stateMachine.ChangeState(enemy.idleState);
            Debug.Log("Ground Not Detected");
            return;
        }

        if (enemy.wallDetected)
        {
            enemy.movementComponent.SetVelocity(0, enemy.Rb.linearVelocity.y);
            stateMachine.ChangeState(enemy.idleState);
            Debug.Log("Wall Detected");
            return;
        }
    }
}
