using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy", menuName = "Scriptable Objects/Enemy/Slime")]
public class SO_TestEnemy : ScriptableObject
{
    public float health;
    public float attackPower;
    public SpriteRenderer sprite;

}
