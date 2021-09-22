using UnityEngine;

public class PlayerHealthDamageController : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;  // TODO: set it back to private
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        //Debug.Log("we are getting hit by a damage of " + damage);
        currentHealth -= damage;
    }
}
