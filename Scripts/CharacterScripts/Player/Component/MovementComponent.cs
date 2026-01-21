using UnityEngine;

/// <summary>
/// Component that stores movement-related configuration and state for the player.
/// Contains parameters for jump limits, dash restrictions, and special movement behaviors.
/// </summary>
public class MovementComponent : MonoBehaviour
{
    /// <summary>
    /// The maximum upward velocity the player can achieve while jumping.
    /// </summary>
    [field: SerializeField] public float maxJumpVelocity { get; private set; } = 10;
    
    /// <summary>
    /// Maximum number of times the player can dash while in the air before touching the ground.
    /// </summary>
    [field: SerializeField] public int maxAmountDashInAir { get; private set; } = 1;
    
    /// <summary>
    /// Current count of how many times the player has dashed in the air.
    /// Reset when touching the ground or wall sliding.
    /// </summary>
    [field: SerializeField] public int timesDashedInAir = 0;
    
    /// <summary>
    /// Initial gravity scale applied when performing a jump attack.
    /// </summary>
    [field: SerializeField] public float jumpAttackGravityScale;
    
    /// <summary>
    /// Maximum gravity scale that can be reached during a jump attack.
    /// </summary>
    [field: SerializeField] public float maxJumpAttackGravityScale;
    
    /// <summary>
    /// Buffer duration for wall slide transitions.
    /// </summary>
    [field: SerializeField] public float wallSlideBuffer { get; private set; }
    
    /// <summary>
    /// Acceleration multiplier for gravity during jump attacks.
    /// Gradually increases gravity over time for more responsive falling.
    /// </summary>
    public float gravityAccWhileJumpAtk { get; private set; } = 1.002f;
}