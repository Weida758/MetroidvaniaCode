using UnityEngine;


public class EnemyState : CharacterBaseState
{
    protected readonly Enemy enemy;
    private Animator animator;
    public EnemyState(StateMachine stateMachine, string animBoolName, Enemy enemy) : 
        base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        if (enemy != null)
        {
            animator = enemy._animator;
        }
        else
        {
            Debug.Log("No animator on enemy");
        }
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool(animBoolName, true);
    }
    
    public override void Exit(){
        base.Exit();
        animator.SetBool(animBoolName, false);
    }

    public override void Update()
    {
        base.Update();

        animator.SetFloat("xVelocity", enemy.Rb.linearVelocity.x);
        //Calculates the animation speed based on the movement speed of the enemy
        enemy.movementComponent.moveAnimSpeedMultiplier = Mathf.Clamp(enemy.movementComponent.moveAnimSpeedMultiplier
            ,Mathf.Sqrt(enemy.movementComponent.moveSpeed / 3), 2);
        
        enemy._animator.SetFloat("moveAnimSpeedMultiplier", enemy.movementComponent.moveAnimSpeedMultiplier);
        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(enemy.attackState);
        }
    }
    
    
    
    
    
    
}
    

    
    
    
    