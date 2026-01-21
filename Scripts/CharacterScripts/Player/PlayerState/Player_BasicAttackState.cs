using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) :
        base(player, stateMachine, animBoolName)
    {
        comboLimit = player.weapon.weaponData.comboLimit;
    }

    private SO_Weapon playerWeapon;
    private float attackVelocityTimer;
    private const int ComboStartIndex = 0;
    public int comboIndex { get; private set; } = 0;
    private bool comboAttackQueued;
    private readonly int comboLimit;
    private float lastTimeAttacked;
    private int attackDirection;

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        playerWeapon = player.weapon.weaponData;
        ResetComboIndex();
        
        // Determine attack direction: use input if moving, otherwise use facing direction
        attackDirection = player.GetMoveInput().x != 0
            ? (int)player.GetMoveInput().x
            : player.facingDir;
        
        player._animator.SetInteger("BasicAttackIndex", comboIndex);
        attackVelocityTimer = playerWeapon.attackVelocityDuration;
        player.SetVelocity(playerWeapon.attackVelocity[comboIndex].x * attackDirection, 
            playerWeapon.attackVelocity[comboIndex].y );
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();
        
        // Queue next attack if player presses attack during current attack
        if (player.basicAttackAction.WasPressedThisFrame())
        {
            QueueNextAttack();
        }
        
        // Animation trigger signals attack animation has finished
        if (triggerCalled)
        {
            if (comboAttackQueued)
            {
                player._animator.SetBool(animBoolName, false);
                player.EnterAttackStateWithDelay();
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        // Advance combo and record attack time
        ++comboIndex;
        lastTimeAttacked = Time.time;
        ResetComboIndex();
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        // Stop forward momentum after attack velocity duration expires
        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, player.Rb.linearVelocity.y);
        }
    }

    private void ResetComboIndex()
    {
        // Reset combo if too much time has passed since last attack
        if (Time.time > lastTimeAttacked + playerWeapon.comboResetTime)
        {
            comboIndex = ComboStartIndex;
        }
        
        // Loop back to first attack if we've exceeded the combo limit
        if (comboIndex > comboLimit - 1)
        {
            comboIndex = ComboStartIndex;
        }
    }

    private void QueueNextAttack()
    {
        // Only queue if we haven't reached the last attack in the combo
        if (comboIndex < comboLimit - 1)
        {
            comboAttackQueued = true;
        }
    }
}