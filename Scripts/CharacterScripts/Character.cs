using System;
using UnityEngine;

/// <summary>
/// Base class for all character entities in the game (Player, Enemies, etc.).
/// Provides core functionality for animation, physics, state management, and collision detection.
/// </summary>
public class Character : MonoBehaviour
{
    protected StateMachine stateMachine;
    
    /// <summary>
    /// The animator component attached to this character's child GameObject.
    /// </summary>
    public Animator _animator { get; protected set; }
    
    /// <summary>
    /// The Rigidbody2D component used for physics calculations.
    /// </summary>
    public Rigidbody2D Rb {get; protected set;}
    
    [ReadOnly] [SerializeField] protected String state;
    
    [SerializeField] protected float groundCheckDistance;

    [field: SerializeField] public LayerMask groundMask { get; protected set; }
    [SerializeField] protected Transform groundCheck;
    
    /// <summary>
    /// Indicates whether the character is currently touching the ground.
    /// Updated automatically via ground collision detection.
    /// </summary>
    public bool groundDetected { get; private set; }

    private bool _facingRight = true;
    
    /// <summary>
    /// The direction the character is facing: 1 for right, -1 for left.
    /// </summary>
    public int facingDir { get; private set; } = 1;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
    }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { } 
    protected virtual void Start() { }
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
        state = stateMachine.currentState.ToString();
    }
    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdateActiveState();
    }
    
    /// <summary>
    /// Sets the character's velocity and handles automatic flipping based on movement direction.
    /// </summary>
    /// <param name="xVelocity">Horizontal velocity to apply.</param>
    /// <param name="yVelocity">Vertical velocity to apply.</param>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    
    /// <summary>
    /// Sets the character's velocity without triggering automatic flipping.
    /// Useful for cases where direction change shouldn't affect facing direction (e.g., wall sliding).
    /// </summary>
    /// <param name="xVelocity">Horizontal velocity to apply.</param>
    /// <param name="yVelocity">Vertical velocity to apply.</param>
    public void SetVelocityNoFlip(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }
    
    private void HandleFlip(float xVelocity)
    {
        if (!_facingRight && xVelocity > 0)
        {
            Flip();
        }
        else if (xVelocity < 0 && _facingRight)
        {
            Flip();
        }
    }
    
    /// <summary>
    /// Flips the character's facing direction by rotating 180 degrees on the Y-axis.
    /// Updates the facingDir property accordingly.
    /// </summary>
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        _facingRight = !_facingRight;
        facingDir *= -1;
    }
    
    /// <summary>
    /// Notifies the current state that an animation trigger has been called.
    /// Typically invoked from animation events.
    /// </summary>
    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    /// <summary>
    /// Performs collision detection for ground and other environmental checks.
    /// Override this method in derived classes to add additional collision checks.
    /// </summary>
    protected virtual void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, 
            Vector2.down, groundCheckDistance, groundMask);
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        
    }
    
    /// <summary>
    /// Enables or disables a specific player state, preventing or allowing transitions into that state.
    /// </summary>
    /// <param name="currentState">The state to enable or disable.</param>
    /// <param name="value">True to enable the state, false to disable it. Defaults to true.</param>
    public void SetStateActive(PlayerState currentState, bool value = true)
    {
        currentState.SetStateActive(value);
    }
}