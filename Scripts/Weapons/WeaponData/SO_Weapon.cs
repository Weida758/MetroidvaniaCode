using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class SO_Weapon : ScriptableObject
{
    public enum Weight
    {
        Light,
        Medium,
        Heavy
    }

    [field: SerializeField] public new string name { get; private set; }
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float[] attackRadius { get; private set; }
    [field: SerializeField] public Vector2[] hitboxPosition { get; private set; }
    [field: SerializeField] public int itemID { get; private set; }
    [field: SerializeField] public Weight weight { get; private set; }
    [field: SerializeField] public Vector2[] attackVelocity { get; private set; }
    [field: SerializeField] public float attackVelocityDuration { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public float comboResetTime { get; private set; } = 1;
    [field: SerializeField] public int comboLimit { get; private set; } = 3;




}
