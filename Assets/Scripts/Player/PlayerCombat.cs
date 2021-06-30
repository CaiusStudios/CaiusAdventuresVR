using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int playerCurrentStrikeStrength = 50;
    public float attackRate = 1.0f;
    public Animator animator;
    private PlayerAudioFX _playerAudioFx;
    
    private Collider[] _hitEnemies;
    private bool _freezeAttack;
    
    // Update is called once per frame
    void Update()
    {
        // if the movements are frozen, attack is frozen as well in this script.
        _freezeAttack = FindObjectOfType<DiscoverPads>().freezeMovements;
        _playerAudioFx = FindObjectOfType<PlayerAudioFX>();
        
        if (Input.GetKeyDown(KeyCode.Mouse0) & !_freezeAttack)
        {
            Attack();
            _playerAudioFx.PlayAudioFX();
        }
    }

    void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("SwingVert");
        
        // detect enemies in range of attack
        // Physics.OverlapSphereNonAlloc(attackPoint.position, attackRange, _hitEnemies, enemyLayers);
        _hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // damage them
        foreach (Collider enemy in _hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("DungeonBoss"))
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<EnemyHealthDamageController>().TakeDamage(playerCurrentStrikeStrength);   
            }

            if (enemy.CompareTag("Destroyable"))
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Destroyable>().DestroyWithFragments();
            }
        }
    }


    
    //
    // Draw Gizmo (in Scene) for debug purposes: shows the range of attack
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
