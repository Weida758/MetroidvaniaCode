using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    [SerializeField] public SO_Weapon weaponData;
    [SerializeField] private LayerMask damageableLayers;
    [SerializeField] private Transform player;
    [field: FormerlySerializedAs("<hitboxes>k__BackingField")] [field: SerializeField] public Transform[] basicHitboxes { get; private set; }

    private void Awake()
    {
        if (basicHitboxes.Length > weaponData.comboLimit)
        {
            Debug.LogError("Amount of hitbox does not match amount of basic attacks");
        }
        
        if (player == null)
        {
            Debug.LogWarning("No player holding weapon");
        }
    }
    public void Attack(int attackIndex)
    {
        //basic hit boxes are the transform position of the game object representing the hitbox location
        Collider2D[] colliders = Physics2D.OverlapCircleAll(basicHitboxes[attackIndex].position, 
            weaponData.attackRadius[attackIndex], damageableLayers);

        foreach (Collider2D col in colliders)
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            damageable?.TakeDamage(weaponData.attackPower, player);
        }
        
    }
    
}
