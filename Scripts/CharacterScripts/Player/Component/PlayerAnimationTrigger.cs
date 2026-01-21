using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void CurrentStateTrigger()
    {
        player.CallAnimationTrigger();
    }

    public void AttackStart()
    {
        Debug.Log("Attack Start");
    }

    public void ApplyAttack()
    {
        player.weapon.Attack(player.basicAttackState.comboIndex);
    }
}
