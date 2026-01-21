using UnityEngine;



public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : 
        base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        
        if (player.GetMoveInput() == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.SetVelocity(player.GetMoveInput().x * player.moveSpeed, player.Rb.linearVelocity.y);
    }
}
