using UnityEngine;

public abstract class PlayerState : CharacterBaseState
{
    protected readonly Player player;
    protected PlayerState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(stateMachine, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        player._animator.SetBool(animBoolName, true);
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;
        player.dashCoolDown -= Time.deltaTime;
        player._animator.SetFloat("yVelocity", player.Rb.linearVelocity.y);

        if (player.dashAction.WasPressedThisFrame() && CanDash())
        {
            player.dashCoolDown = player.dashCoolDownDuration;
            if (stateMachine.currentState is Player_AirState or Player_WallJumpState)
            {
                player.movementData.timesDashedInAir += 1;
                Debug.Log("Times DashedInAir: " + player.movementData.timesDashedInAir);
            }
            stateMachine.ChangeState(player.dashState);
            return;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        player._animator.SetBool(animBoolName, false);
    }

    private bool CanDash()
    {

        if (stateMachine.currentState == player.dashState)
        {
            return false;
        }

        if (player.movementData.timesDashedInAir >= player.movementData.maxAmountDashInAir)
        {
            return false;
        }

        if (player.dashCoolDown > 0)
        {
            return false;
        }
        

        return true;
    }

    
}
