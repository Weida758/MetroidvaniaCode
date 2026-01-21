using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) :
        base(player, stateMachine, animBoolName)
    {
    }

    private bool touchedGround = false;

    public override void Enter()
    {
        base.Enter();
        player.Rb.gravityScale = player.movementData.jumpAttackGravityScale;
        touchedGround = false;
    }

    public override void Update()
    {
        base.Update();
        if (player.Rb.gravityScale < player.movementData.maxJumpAttackGravityScale)
        {
            player.Rb.gravityScale *= player.movementData.gravityAccWhileJumpAtk;
        }

        player.Rb.gravityScale = Mathf.Clamp(player.Rb.gravityScale, 0,
            player.movementData.maxJumpAttackGravityScale);
        
        if (player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            player._animator.SetTrigger("JumpAttackTrigger");
            player.SetVelocity(0, player.Rb.linearVelocity.y);
        }

        if (triggerCalled && player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        if (touchedGround == false)
        {
            player.SetVelocity(
                player.GetMoveInput().x * (player.moveSpeed * player.moveSpeedSlowMultiplierInAir),
                player.Rb.linearVelocity.y);
        }
    }
}
