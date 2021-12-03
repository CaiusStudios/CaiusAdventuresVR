using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerMask;
    public int enemyCurrentStrikeStrength = 5;

    private Collider[] _hitPlayer;
    
    public int maxHealth = 100;
    private int _currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }
    
    public void Attack()
    {
        // detect player in range of attack
        _hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
        
        // damage the player
        foreach (Collider player in _hitPlayer)
        {
            // make sure the player itself is hit - and not its blade, etc.
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerManager>().TakeDamage(enemyCurrentStrikeStrength);
            }
        }
    }
    
    // Draw Gizmo
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    //
    // Enemy Health
    //
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        gameObject.SetActive(false);
    }
    
}
