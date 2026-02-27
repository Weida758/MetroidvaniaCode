using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    
    [Header("Movement multipliers")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    
    [Tooltip("This is used to determine how much the player moves while attacking")]
    [SerializeField] private float attackGlide;
    private float _moveInputWhileAttacking;
    
    [Header("The layer of the ground")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Fields for Handling Attack")]
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    
    
    private InputActionMap _inputActionsMap;
    private Animator _animator;
    
    
    // All Input Actions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    
    
    private Vector2 _moveInput;
    
    
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private float _playerHalfHeight;
    
    //Movement logic decisions
    private bool _isGrounded;
    private bool _isFacingRight = true;
    private bool _canMove = true;
    private bool _canJump = true;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _inputActionsMap = _inputActions.FindActionMap("Player");
        _moveAction = _inputActionsMap.FindAction("Move");
        _jumpAction = _inputActionsMap.FindAction("Jump");
        _attackAction = _inputActionsMap.FindAction("Attack");
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _playerHalfHeight = _spriteRenderer.bounds.extents.y + 0.2f;
    }
    private void OnEnable()
    {
        
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _jumpAction.performed += OnJump;
        _attackAction.performed += OnAttack;
        
        _moveAction.Enable();
        _jumpAction.Enable();
        _attackAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _jumpAction.performed -= OnJump;
        _attackAction.performed -= OnAttack;

        _moveAction.Disable();
        _jumpAction.Disable();
        _attackAction.Disable();
    }
    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void Update()
    {
        
        _isGrounded = Physics2D.Raycast(_rb.position, Vector2.down, _playerHalfHeight, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector3.down * _playerHalfHeight, Color.red);
        HandleFlip();
        HandleAnimation();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>().normalized;
    }

    private void HandleAnimation()
    {
        _animator.SetFloat("xVelocity", _rb.linearVelocity.x);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.linearVelocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded && _canJump)
        {
            _rb.linearVelocity = new Vector2 (_rb.linearVelocity.x, jumpForce);
        }
        else
        {
            Debug.Log("Cannot Jump when not on the ground or attacking");
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        AttemptAttack();
    }

    private void MoveCharacter()
    {
        if (_canMove)
        {
            _rb.linearVelocity = new Vector2(_moveInput.x * speed, _rb.linearVelocity.y);
        }
        else
        {
            _rb.linearVelocity = new Vector2(attackGlide * _moveInputWhileAttacking, _rb.linearVelocity.y);
        }
    }


    private void HandleFlip()
    {
        if (_rb.linearVelocity.x > 0 && _isFacingRight is false)
        {
            FlipPlayer();
        }
        else if (_rb.linearVelocity.x < 0 && _isFacingRight)
        {
            FlipPlayer();
        }
    }
    [ContextMenu("FlipPlayer")]
    private void FlipPlayer()
    {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
    }

    private void AttemptAttack()
    {
        if (_isGrounded)
        {
            _animator.SetTrigger("Attack");
            _moveInputWhileAttacking = _moveInput.x;
        }
        
    }

    public void DamageEnemies()
    {
        Collider2D[] enemyHitColliders =
            Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
        foreach (Collider2D enemy in enemyHitColliders)
        {
            enemy.GetComponent<TestEnemy>().TakeDamage(20f);
        }
    }

    public void EnableJumpAndMovement(bool enable)
    {
        _canMove = enable;
        _canJump = enable;

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    
    
}
