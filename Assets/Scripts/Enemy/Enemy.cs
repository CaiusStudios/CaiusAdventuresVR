using System;
using UnityEngine;
/// <summary>
///   <para> This represents any kind of enemy (including ghosts, bosses).</para>
///   <para> They all have fighting skills (methods), and some kind of health.</para>
/// </summary>
public class Enemy : MonoBehaviour
{
    private Collider[] _hitPlayer;

    // -------------------------------------------------------------------
    // Properties
    // -------------------------------------------------------------------
    // the overall health system of the enemy
    private HealthSystem _healthSystem;
    public HealthSystem HealthSystem
    {
        get { return _healthSystem; }
        set { _healthSystem = value; }
    }

    // the damage inflicted by an attack of this enemy.
    private int _strength = 5;
    public int Strength
    {
        get { return _strength; }
        set { _strength = value; }
    }
    
    // how far can the enemy attack
    private float _attackRange = 0.5f;
    public float AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }
    
    // to which LayerMask does the enemy attack
    private LayerMask _playerMask;
    public LayerMask PlayerMask
    {
        get { return _playerMask; }
        set { _playerMask = value; }
    }
    
    // the starting point of the attack; which part of the enemy is really attacking (it's whole body, weapon, etc.)
    private Transform _attackPoint;
    public Transform AttackPoint
    {
        get { return _attackPoint; }
        set { _attackPoint = value; }
    }
    

    // -------------------------------------------------------------------
    // Fight related methods
    // -------------------------------------------------------------------
    /// <summary>
    ///   <para> It is called by the EnemyAI scripts of the enemy.</para>
    /// </summary>
    public void Attack()
    {
        // detect player in range of attack
        _hitPlayer = Physics.OverlapSphere(_attackPoint.position, _attackRange, _playerMask);

        // damage the player
        foreach (Collider player in _hitPlayer)
        {
            // make sure the player itself is hit - and not its blade, etc.
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerManager>().TakeDamage(_strength);
            }
        }
    }

    /// <summary>
    ///   <para> To Debug the Attack() method in Unity's Scene.</para>
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
        {
            return;
        }

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
        _healthSystem.OnDeath += HealthSystemOnOnDeath;
    }

    /// <summary>
    ///   <para> Trigger when enemy's life points reach zero.</para>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystemOnOnDeath(object sender, EventArgs e)
    {
        Die(gameObject);
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