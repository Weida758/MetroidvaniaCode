using System;
using UnityEngine;

public class EnemyMovementComponent : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb { get; private set; }
    public float idleTimer;
    [Header("Battle Detail")] 
    
    public float moveAnimSpeedMultiplier;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }
    
}
