using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(StateMachine stateMachine, string animBoolName, Enemy enemy) : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
    
    
}
