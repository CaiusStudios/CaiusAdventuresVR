using System.Collections;
using UnityEngine;
/// <summary>
///   <para> This represents any kind of enemy (including ghosts, bosses).</para>
///   <para> They all have fighting skills (methods), and some kind of health (points or boss-specific).</para>
/// </summary>
public class Enemy : MonoBehaviour
{
    private Collider[] _hitPlayer;

    
    // -------------------------------------------------------------------
    // Properties
    // -------------------------------------------------------------------
    private HealthSystem _healthSystem;  // the overall health system of the enemy
    public HealthSystem HealthSystem
    {
        get { return _healthSystem; }
        set { _healthSystem = value; }
    }
    
    private int _strength = 15;  // the damage inflicted by an attack of this enemy.
    public int Strength
    {
        get { return _strength; }
        set { _strength = value; }
    }
    
    private float _attackRange = 2.5f;  // how far can the enemy attack
    public float AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }

    private bool _canAttack = true;

    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    
    private LayerMask _playerMask;  // to which LayerMask does the enemy attack
    public LayerMask PlayerMask
    {
        get { return _playerMask; }
        set { _playerMask = value; }
    }
    
    private Transform _attackPoint;  // which part of the enemy is really attacking
    public Transform AttackPoint
    {
        get { return _attackPoint; }
        set { _attackPoint = value; }
    }
    

    // -------------------------------------------------------------------
    // Fight related methods
    // -------------------------------------------------------------------
    /// <summary>
    ///   <para> It is called by the EnemyAI scripts of the enemy to inflict damage.</para>
    ///   <para> the _forceDirection is for the enemy to take a step back at each strike.</para>
    ///   <para> Remove the parameter, Attack(), to strike continuously.</para>
    /// </summary>
    public void Attack(Vector3 forceDirection, float timeToNextAttack, float stepBackMultiplayer)
    {
        // detect player in range of attack
        _hitPlayer = Physics.OverlapSphere(_attackPoint.position, _attackRange, _playerMask);
        
        foreach (Collider player in _hitPlayer)
        {
            // make sure the player itself is hit - and not its blade, etc.
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerManager>().HealthSystem.Damage(_strength);
            }
            
            StartCoroutine(StepBack(forceDirection, timeToNextAttack, stepBackMultiplayer));
        }
    }
    
    IEnumerator StepBack(Vector3 forceDirection, float waitTillNextAttack, float stepBackMultiplayer)
    {
        Vector3 adjustedForceDirection = forceDirection * _attackRange * stepBackMultiplayer;
        if (adjustedForceDirection == Vector3.zero)
        {
            adjustedForceDirection = Vector3.back * _attackRange * stepBackMultiplayer;
        }
        
        gameObject.GetComponent<EnemyAI>().Push(adjustedForceDirection);
        CanAttack = false;  // signal that this gameObject cannot attack for the moment 
        yield return new WaitForSeconds(waitTillNextAttack);
        CanAttack = true;  // enemy has waited long enough and can re-attack (and move again toward the player) 
    }
    
    /// <summary>
    /// Inflict a damage to the player. The enemy does NOT step back here.
    /// </summary>
    public void Attack()
    {
        // detect player in range of attack
        _hitPlayer = Physics.OverlapSphere(_attackPoint.position, _attackRange, _playerMask);
        
        foreach (Collider player in _hitPlayer)
        {
            // make sure the player itself is hit - and not its blade, etc.
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerManager>().HealthSystem.Damage(_strength);
            }
        }
    }
    

    /// <summary>
    ///   <para> To Debug the Attack() method in Unity's Scene.</para>
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) { return; }
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    // -------------------------------------------------------------------
    // Health related methods: see HealthSystem.cs
    // -------------------------------------------------------------------
    /// <summary>
    ///   <para> Given prefab bar is instantiated with that amount of health points.</para>
    /// </summary>
    /// <param name="pfHealthBar"></param>
    /// <param name="maxHealth"></param>
    public void SetupHealthSystem(Transform pfHealthBar, int maxHealth)
    {
        Vector3 thisPosition = transform.position;   // TODO: Needed? it's to avoid the msg "Repeated property access".
        Transform healthBarTransform = Instantiate(
            pfHealthBar, 
            new Vector3(thisPosition.x, 5, thisPosition.z),
            Quaternion.identity, transform);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        
        // Create initial health status in health system + listen onDeath
        _healthSystem = new HealthSystem(maxHealth);
        healthBar.Setup(_healthSystem);
    }
    
    /// <summary>
    ///   <para> It is called when the enemy has no health points left.</para>
    /// </summary>
    /// <param name="aGameObject"></param>
    public void Die(GameObject aGameObject)
    {
        aGameObject.SetActive(false);
    }
}