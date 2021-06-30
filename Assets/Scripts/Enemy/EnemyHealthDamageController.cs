using UnityEngine;

public class EnemyHealthDamageController : MonoBehaviour
{
    public int maxHealth = 100;
    public int damageInflictionPoint = 5;  // damage that this gameObject inflict
    
    public int _currentHealth;  // TODO: set it back to private
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        // Play hurt animation
        // ...
        
        // check if died
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        
        // Play dead animation
        // ....
        
        // Disable the enemy
        // ...
        gameObject.SetActive(false);
    }
}
