using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    private float timer = 0.1f;
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) :
        base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.wallJumpGraceTime > 0)
        {
            player.SetVelocity(player.Rb.linearVelocity.x, player.wallJumpVerticalForce);
            Debug.Log("wall Jumped during grace");
        }
        else
        {
            player.Flip();
            player.Rb.AddForceY(.1f, ForceMode2D.Impulse);
            player.SetVelocity(player.wallJumpHorizontalForce * player.facingDir, player.wallJumpVerticalForce);
            Debug.Log("wall Jumped not during grace");
        }

        player.wallJumpGraceTime = 0;
        timer = 0.1f;

    }


    public override void Update()
    {
        base.Update();
        //player.Rb.AddForceX(player.wallJumpHorizontalForce * player.facingDir, ForceMode2D.Impulse);
        timer -= Time.deltaTime;
        if (player.wallDetected)
        {
            Debug.Log("WallJump to WallSlide");
            stateMachine.ChangeState(player.wallSlideState);
            return;

        }
        
        if (player.Rb.linearVelocity.y < 0 && !player.wallDetected)
        {
            Debug.Log("WallJump to fall");
            stateMachine.ChangeState(player.fallState);
            return;

        }

    }

    public override void Exit()
    {
        base.Exit();

    }
    

    public override void FixedUpdate()
    {
        if (timer > 0)
        {
            return;
        }
        player.SetVelocity(player.GetMoveInput().x * player.moveSpeed,  player.Rb.linearVelocity.y);
        
    }
}
