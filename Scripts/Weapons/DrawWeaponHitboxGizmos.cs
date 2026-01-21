using Unity.VisualScripting;
using UnityEngine;

namespace Weapons
{
    public class DrawWeaponHitboxGizmos : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private int hitboxIndex;
        

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 
                weapon.weaponData.attackRadius[hitboxIndex]);
        }
        
        
    }
}