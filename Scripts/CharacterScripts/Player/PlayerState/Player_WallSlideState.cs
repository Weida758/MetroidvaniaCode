using System;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(player, stateMachine, animBoolName)
    {
    }

    private int currentFacingDirection;

    public override void Enter()
    {
        base.Enter();
        player.movementData.timesDashedInAir = 0;
        currentFacingDirection = player.facingDir;
    }
    
    public override void Update()
    {
        base.Update();
        
        if (player.jumpPressedThisFrame)
        {
            stateMachine.ChangeState(player.wallJumpState);
            player.jumpPressedThisFrame = false;
            return;
        }
        
        // Check if player is trying to move away from the wall
        if (Mathf.Abs(player.GetMoveInput().x) > 0.2f)
        {
            int inputDir = player.GetMoveInput().x > 0 ? 1 : -1;
            
            // If input is opposite to wall, detach and start grace period for wall jump
            if (inputDir != currentFacingDirection)
            {
                stateMachine.ChangeState(player.fallState);
                player.Flip();
                player.wallJumpGraceTime = player.wallJumpGraceTimeValue;
                return;
            }
        }
        
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            player.Flip();
            return;
        }

        if (!player.wallDetected)
        {
            stateMachine.ChangeState(player.fallState);
            Debug.Log("Wall Not detected");
            return;
        }
        
        HandleWallSlide();
    }

    private void HandleWallSlide()
    {
        // Fast slide down when holding down, otherwise slow slide
        if (player.GetMoveInput().y < -0.2f)
        {
            player.SetVelocityNoFlip(0, player.Rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocityNoFlip(0, player.Rb.linearVelocity.y * player.wallSlideSlowMultiplier);
        }
    }
}