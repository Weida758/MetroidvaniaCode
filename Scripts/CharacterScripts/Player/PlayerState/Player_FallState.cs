using UnityEngine;

public class Player_FallState : Player_AirState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(player, stateMachine, animBoolName)
    {
    }

    private readonly float gravityAcceleration = 1.0005f;

    public override void Enter()
    {
        base.Enter();
        player.Rb.gravityScale = player.normalGravity;
    }
    
    public override void Update()
    {
        base.Update();

        // Gradually increase gravity for faster, more responsive falling
        if (player.Rb.gravityScale < player.maxGravity)
        {
            player.Rb.gravityScale *= gravityAcceleration;
        }
        player.Rb.gravityScale = Mathf.Clamp(player.Rb.gravityScale, 0, player.maxGravity);
        
        // Countdown grace timers
        player.wallJumpGraceTime -= Time.deltaTime;
        player.coyoteTime -= Time.deltaTime;
        
        // Allow jump shortly after leaving ground (coyote time)
        if (player.coyoteTime > 0 && player.jumpPressedThisFrame && player.canJump)
        {
            player.jumpPressedThisFrame = false;
            stateMachine.ChangeState(player.jumpState);
            Debug.Log("Coyote time jump");
            return;
        }
        
        // Allow wall jump shortly after leaving wall (grace period)
        if (player.jumpPressedThisFrame && player.wallJumpGraceTime > 0)
        {
            stateMachine.ChangeState(player.wallJumpState);
            player.jumpPressedThisFrame = false;
            Debug.Log("Wall Jump Grace");
            return;
        }
        
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            player.Rb.gravityScale = player.normalGravity;
            return;
        }

        if (player.wallDetected && player.wallSlideState.isStateActive())
        {
            stateMachine.ChangeState(player.wallSlideState);
            player.Rb.gravityScale = player.normalGravity;
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}