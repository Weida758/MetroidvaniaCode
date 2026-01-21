using UnityEngine;

public class Player_DashState : PlayerState
{
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(player, stateMachine, animBoolName)
    {
    }

    private float dashTimer = 0;
    private int dashDir;
    private bool dashOnWall;  // True if dash was initiated while on a wall
    private bool hasLeftWall;  // Tracks if player has left the wall during dash

    public override void Enter()
    {
        base.Enter();
        player.canJump = false;
        player.wallJumpGraceTime = -1;
        player.Rb.gravityScale = 0;
        stateTimer = player.dashDuration;

        dashDir = player.GetMoveInput().x != 0 ? (int)player.GetMoveInput().x : player.facingDir;
        dashOnWall = player.wallDetected && !player.groundDetected;
        hasLeftWall = !dashOnWall;

        // When dashing off a wall, dash away from it and flip to face outward
        if (dashOnWall)
        {
            dashDir = -player.facingDir;
            player.Flip();
        }
    }

    public override void Update()
    {
        base.Update();
        dashTimer += Time.deltaTime;
        
        // Track when player leaves the wall during a wall dash
        if (!hasLeftWall && dashOnWall && !player.wallDetected)
        {
            hasLeftWall = true;
        }

        // Can only enter wall slide if: not dashing on wall, or has left the wall
        bool canEnterWallSlide = !dashOnWall || hasLeftWall;
        if (canEnterWallSlide && player.wallDetected)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
                return;
            }
            
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
        
        // Transition out when dash duration expires
        if (stateTimer < 0 && player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        else if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        // Dash decelerates from 2x speed to 10f over the dash duration
        float dashSpeed = Mathf.Lerp(player.dashSpeed * 2, 10f, dashTimer / player.dashDuration);
        player.SetVelocity(dashSpeed * dashDir, 0);
        Debug.Log("Not on wall dash");
    }

    public override void Exit()
    {
        base.Exit();
        dashTimer = 0;
        dashOnWall = false;
        hasLeftWall = false;
        player.Rb.gravityScale = player.normalGravity;
    }
}