using UnityEngine;

public class Player_JumpState : Player_AirState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player.canJump = false;
        
        // Apply initial jump impulse - different handling for coyote time vs normal jump
        if (player.coyoteTime > 0)
        {
            // Reset vertical velocity first for consistent coyote time jumps
            player.Rb.linearVelocity = new Vector2(player.Rb.linearVelocity.x, 0.5f);
            player.Rb.AddForce(Vector2.up * player.suddenJumpImpulseForce, ForceMode2D.Impulse);
        }
        else
        {
            player.Rb.AddForce(new Vector2(0, player.suddenJumpImpulseForce), ForceMode2D.Impulse);
        }
    }

    public override void Update()
    {
        base.Update();
        
        // Cap jump velocity to prevent excessive height
        player.Rb.linearVelocityY = Mathf.Clamp(player.Rb.linearVelocity.y, 0, player.movementData.maxJumpVelocity);
        
        // Transition to fall when: button released, max hold time reached, or falling
        if (!player.jumpHeld || player.jumpHoldTime > player.maxJumpHoldTime || player.Rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.jumpHoldTime += Time.deltaTime;
        
        // Variable jump height: apply decreasing force while jump is held
        if (player.jumpHoldTime < player.maxJumpHoldTime && player.jumpHeld)
        {
            player.Rb.gravityScale = player.gravityWhileJumping;
            
            // Lerp force from full to zero over the max hold time
            float jumpProgress = player.jumpHoldTime / player.maxJumpHoldTime;
            float forceAmount = Mathf.Lerp(player.jumpForce, 0, jumpProgress);
            player.Rb.AddForce(Vector2.up * forceAmount, ForceMode2D.Force);
        }
    }
}