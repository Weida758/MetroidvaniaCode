using UnityEngine;

public class TestPlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private TestPlayerController _player;
    
    private void AttackBegun()
    {
        _player.EnableJumpAndMovement(false);
        Debug.Log("Attack Begun");
    }

    private void AttackEnd()
    {
        _player.EnableJumpAndMovement(true);
        Debug.Log("Attack Ended");
    }

    public void DamageEnemies() => _player.DamageEnemies();
}
