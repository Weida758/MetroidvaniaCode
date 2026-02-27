using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

//The base class of all enemies, not intended to be used non-inherited
public class Enemy : Character
{

    public SpriteRenderer spriteRenderer { get; protected set; }
    public float spriteBoundY { get; private set; }

    //--------------EnemyStates-------------------
    public Enemy_Idle idleState { get; protected set; }
    public Enemy_Move moveState { get; protected set; }
    public Enemy_AttackState attackState { get; protected set; }
    public Enemy_BattleState  battleState { get; protected set; }

    public EnemyMovementComponent movementComponent { get; protected set; }
    
    //--------------Wall Detection-----------------
    [field: SerializeField] public Transform wallCheckPointOne { get; private set; }
    [field: SerializeField] public Transform wallCheckPointTwo { get; private set; }
    [field: SerializeField] public float wallCheckRadius;
    [ReadOnly] [SerializeField] public bool wallDetected;

    [Header("Player Detection")] 
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Transform detectionOrigin;
    [field: SerializeField] public float playerDetectionDistance { get; private set; } = 10;

    [Header("Battle Details")] 
    public float battleTimeDuration;
    public FloatVariable attackDistance;
    [field: SerializeField] public  float attackCooldown { get; private set; }
    public float currentAttackCooldown;
    
    [ReadOnly] public float time;
    public Transform player { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteBoundY = spriteRenderer.bounds.max.y;
    }
    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(detectionOrigin.position, Vector2.right * facingDir, playerDetectionDistance, playerLayer | groundMask);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        return hit;
    }
    protected override void HandleCollisionDetection()
    {
        base.HandleCollisionDetection();

        bool wallCheckOne =
            Physics2D.OverlapCircle(wallCheckPointOne.position, wallCheckRadius, groundMask);
        bool wallCheckTwo =
            Physics2D.OverlapCircle(wallCheckPointTwo.position, wallCheckRadius, groundMask);

        wallDetected = wallCheckOne || wallCheckTwo;
    }

    public virtual void EnterBattleState(Transform player)
    {

        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return;
        }
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    protected override void Update()
    {
        base.Update();
        time = Time.time;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(wallCheckPointOne.position, wallCheckRadius);
        Gizmos.DrawWireSphere(wallCheckPointTwo.position, wallCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(detectionOrigin.position, 
                new Vector3(detectionOrigin.position.x + (facingDir * playerDetectionDistance), 
                detectionOrigin.position.y));

        movementComponent = GetComponent<EnemyMovementComponent>();
        if (movementComponent != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(detectionOrigin.position,
                new Vector3(
                    detectionOrigin.position.x + (facingDir * attackDistance.value),
                    detectionOrigin.position.y));
        }
    }
}

