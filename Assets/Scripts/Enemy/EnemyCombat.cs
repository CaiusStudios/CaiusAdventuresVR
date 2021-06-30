using System;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerMask;
    public int enemyCurrentStrikeStrength = 5;

    private Collider[] _hitPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        // Play an attack animation
        // animator.SetTrigger("Attack");
        
        // detect player in range of attack
        _hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
        
        // damage the player
        foreach (Collider player in _hitPlayer)
        {
            Debug.Log("the enemy is attacking (and hit " + player.name +")");
            // make sure the player itself is hit - and not its blade, etc.
            if (player.CompareTag("Player"))
            {
                Debug.Log(player.name + " hit by " + gameObject.name);
                player.GetComponent<PlayerHealthDamageController>().TakeDamage(enemyCurrentStrikeStrength);
            }
        }
    }
    
    //
    // Draw Gizmo
    //
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
