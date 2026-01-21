using UnityEngine;

public class Player_GroundState : PlayerState
{
    public Player_GroundState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.canJump = true;
        player.coyoteTime = player.coyoteTimeValue;
        player.movementData.timesDashedInAir = 0;
        player.SetVelocity(0, player.Rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (player.Rb.linearVelocity.y < 0 && !player.groundDetected)
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }

        if (player.jumpPressedThisFrame)
        {
            stateMachine.ChangeState(player.jumpState);
            player.jumpPressedThisFrame = false;
            return;
        }

        if (player.basicAttackAction.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }
        
    }
}
