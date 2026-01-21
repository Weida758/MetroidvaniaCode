using UnityEngine;

public class Player_AirState : PlayerState
{
    public Player_AirState(Player player, StateMachine stateMachine, string animBoolName) : 
        base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        //player.SetVelocity(player.GetMoveInput().x * (player.moveSpeed * player.moveSpeedSlowMultiplierInAir),  player.Rb.linearVelocity.y);


        if (player.basicAttackAction.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }

    public override void FixedUpdate()
    {
        player.SetVelocity(player.GetMoveInput().x * (player.moveSpeed * player.moveSpeedSlowMultiplierInAir),  player.Rb.linearVelocity.y);
    }
}
