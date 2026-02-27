using System.Collections;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private SO_TestEnemy enemyType;
    private float _health;
    private float _attackPower;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = enemyType.health;
        _attackPower = enemyType.attackPower;

    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(ChangeColor());
        Debug.Log(gameObject.name + " took damage");
        if (_health <= 0)
        {
            Defeat();
            Debug.Log(gameObject.name + " defeated");
        }
    }

    private IEnumerator ChangeColor()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = Color.white;
    }

    private void Defeat()
    {
        Destroy(gameObject);
    }
}
