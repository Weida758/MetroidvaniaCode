using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "Scriptable Objects/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    public float value;

    public void SetValue(float newValue)
    {
        value = newValue;
    }
}
