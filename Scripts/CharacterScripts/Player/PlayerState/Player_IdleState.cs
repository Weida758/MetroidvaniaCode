using UnityEngine;

public class Player_IdleState : Player_GroundState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(
            player, 
            stateMachine,
            animBoolName
            )
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Rb.gravityScale = player.normalGravity;
    }

    public override void Update()
    {
        base.Update();
        if (Mathf.Abs(player.GetMoveInput().x) > 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
