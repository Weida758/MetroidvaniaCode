using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Main player controller that handles input, movement, and state management.
/// Extends Character with player-specific functionality including jumping, dashing, wall sliding, and combat.
/// </summary>
public class Player : Character
{
    //Input System asset and map
    private PlayerActionSet actionSet;
    private InputActionMap actionMap;
    
    //Input Actions
    /// <summary>
    /// Input action for player movement (horizontal and vertical).
    /// </summary>
    public InputAction moveAction {get; private set;}
    
    /// <summary>
    /// Input action for jumping. Handles both jump initiation and hold for variable jump height.
    /// </summary>
    public InputAction jumpAction { get; private set; }
    
    /// <summary>
    /// Input action for dashing.
    /// </summary>
    public InputAction dashAction { get; private set; }
    
    /// <summary>
    /// Input action for basic attack combo system.
    /// </summary>
    public InputAction basicAttackAction { get; private set; }

    //Player States
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    
    //Player Attributes
    private Vector2 moveInput;
    
    [Header("Player Movement Attributes")] 
    /// <summary>
    /// Component containing movement-related data like max jump velocity and dash limits.
    /// </summary>
    [field: SerializeField] public MovementComponent movementData { get; private set; }
    
    /// <summary>
    /// The base movement speed for horizontal movement.
    /// </summary>
    [field: SerializeField] public float moveSpeed { get; private set; } = 3;
    
    [Header("Jump")]
    /// <summary>
    /// Time remaining for coyote time - allows jumping shortly after leaving a platform.
    /// </summary>
    [ReadOnly] public float coyoteTime;
    
    /// <summary>
    /// True if jump was pressed this frame, used for jump buffering.
    /// </summary>
    [ReadOnly] public bool jumpPressedThisFrame;
    
    /// <summary>
    /// Duration of coyote time in seconds.
    /// </summary>
    [field: SerializeField] public float coyoteTimeValue { get; private set; } = 0.15f;
    
    /// <summary>
    /// Maximum duration the jump button can be held to increase jump height.
    /// </summary>
    [field: SerializeField] public float maxJumpHoldTime { get; private set; } = 0.5f;
    
    /// <summary>
    /// Current time the jump button has been held.
    /// </summary>
    [ReadOnly] public float jumpHoldTime;
    
    /// <summary>
    /// Whether the player is currently allowed to jump.
    /// </summary>
    [ReadOnly] public bool canJump;
    
    /// <summary>
    /// Whether the jump button is currently being held down.
    /// </summary>
    public bool jumpHeld { get; private set; }
    
    /// <summary>
    /// The upward force applied when holding the jump button.
    /// </summary>
    [field: SerializeField] public float jumpForce { get; private set; } = 5;
    
    /// <summary>
    /// Initial impulse force applied at the start of a jump.
    /// </summary>
    [field: SerializeField] public float suddenJumpImpulseForce { get; private set; }
    
    /// <summary>
    /// Gravity scale applied while actively jumping (holding jump button).
    /// </summary>
    [field: SerializeField] public float gravityWhileJumping { get; private set; } = 2.5f;
    
    /// <summary>
    /// The default gravity scale for the player.
    /// </summary>
    [field: SerializeField] public float normalGravity { get; private set; } = 3.5f;
    
    [Space]
    [Header("Movement Special Modifiers")]
    /// <summary>
    /// Multiplier applied to falling speed while wall sliding (0-1 range).
    /// </summary>
    [Range(0, 1)] public float wallSlideSlowMultiplier;
    
    /// <summary>
    /// Maximum gravity scale the player can reach while falling.
    /// </summary>
    [field: SerializeField] public float maxGravity { get; private set; } = 5f;
    
    /// <summary>
    /// Multiplier for horizontal movement speed while in the air (0-1 range).
    /// </summary>
    [Range(0, 1)] public float moveSpeedSlowMultiplierInAir;
    
    [Header("Dash")] 
    /// <summary>
    /// How long the dash state lasts in seconds.
    /// </summary>
    public float dashDuration = 0.25f;
    
    /// <summary>
    /// Cooldown duration between dashes.
    /// </summary>
    [field: SerializeField] public float dashCoolDownDuration { get; private set; }
    
    /// <summary>
    /// Current remaining cooldown time before the player can dash again.
    /// </summary>
    [NonSerialized] public float dashCoolDown;
    
    /// <summary>
    /// The speed at which the player dashes.
    /// </summary>
    public float dashSpeed = 20f;
    
    
    [Header("Wall Jump")] 
    /// <summary>
    /// Horizontal force applied during a wall jump.
    /// </summary>
    public float wallJumpHorizontalForce;
    
    /// <summary>
    /// Vertical velocity set during a wall jump.
    /// </summary>
    public float wallJumpVerticalForce;
    
    /// <summary>
    /// Grace period after leaving a wall where wall jump is still allowed.
    /// </summary>
    [ReadOnly] public float wallJumpGraceTime = 0;
    
    /// <summary>
    /// Duration of the wall jump grace period in seconds.
    /// </summary>
    [field: SerializeField] public float wallJumpGraceTimeValue { get; private set; } = 0.3f;
    
    [Header("Collision Detection")] 
    [SerializeField] private float wallCheckRadius;
    
    /// <summary>
    /// Whether a wall is currently detected adjacent to the player.
    /// </summary>
    public bool wallDetected { get; private set; }
    
    [SerializeField] private Transform wallCheckOne;
    [SerializeField] private Transform wallCheckTwo;
    private bool wallDetectedTop;
    private bool wallDetectedBottom;

    [Header("Weapon Details")] 
    private Coroutine queuedAttackCoroutine;
    
    /// <summary>
    /// The weapon currently equipped by the player.
    /// </summary>
    public Weapon weapon;
    
    [ReadOnly] [SerializeField] private string weaponName;


    protected override void Awake()
    {
        base.Awake();
        
        actionSet = new PlayerActionSet();
        actionMap = actionSet.Player;
        
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
        dashAction = actionMap.FindAction("Dash");
        basicAttackAction = actionMap.FindAction("BasicAttack");

        idleState = new Player_IdleState(this, stateMachine, "Idling");
        moveState = new Player_MoveState(this, stateMachine, "Moving");
        jumpState = new Player_JumpState(this, stateMachine, "JumpFall");
        fallState = new Player_FallState(this, stateMachine, "JumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "JumpFall");
        dashState = new Player_DashState(this, stateMachine, "Dashing");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "BasicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        actionSet.Enable();
        
        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>().normalized;
        moveAction.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();

        jumpAction.performed += HandleJump;
        jumpAction.canceled += HandleJumpCanceled;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        jumpAction.performed -= HandleJump;
        jumpAction.canceled -= HandleJumpCanceled;
        actionSet.Disable();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        //SetStateActive(wallSlideState, false);
    }

    protected override void Update()
    {
        base.Update();
        if (!weapon) return;
        weaponName = weapon.name;
    }
    
    private void HandleJump(InputAction.CallbackContext context)
    {
        if (groundDetected || stateMachine.currentState == wallSlideState || wallJumpGraceTime > 0 || coyoteTime > 0) 
        {
            jumpPressedThisFrame = true;
            jumpHeld = true;
        }
    }
    
    private void HandleJumpCanceled(InputAction.CallbackContext context)
    {
        jumpPressedThisFrame = false;
        jumpHeld = false;
        jumpHoldTime = 0;
    }

    protected override void HandleCollisionDetection()
    {
        base.HandleCollisionDetection();
        
        wallDetectedTop = Physics2D.OverlapCircle(wallCheckOne.position, wallCheckRadius, groundMask);
        wallDetectedBottom = Physics2D.OverlapCircle(wallCheckTwo.position, wallCheckRadius,groundMask);
        
        wallDetected = wallDetectedTop && wallDetectedBottom;
    }
    
    /// <summary>
    /// Queues the next attack in the combo chain with a one-frame delay.
    /// Stops any existing queued attack before starting a new one.
    /// Used by the attack system to chain combos smoothly.
    /// </summary>
    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null)
        {
            StopCoroutine(queuedAttackCoroutine);
        }

        queuedAttackCoroutine = StartCoroutine(CO_EnterAttackStateWithDeley());
    }
    
    //Wait for end of frame then transition into the next attack in queue
    private IEnumerator CO_EnterAttackStateWithDeley()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(wallCheckOne.position, wallCheckRadius);
        Gizmos.DrawWireSphere(wallCheckTwo.position, wallCheckRadius);
    }

    /// <summary>
    /// Gets the current normalized movement input from the player.
    /// </summary>
    /// <returns>A Vector2 containing the normalized horizontal and vertical input values.</returns>
    public Vector2 GetMoveInput() => moveInput;
}