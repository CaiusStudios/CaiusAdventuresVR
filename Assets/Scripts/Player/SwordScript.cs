using System;
using UnityEngine;

/// <summary>
/// The sword gives a damage = max(impact * baseDamage, maxDamage),
/// where impact is the movement/distance of the sword.
///
/// The sword pushes its collision back in impactDirection and with a force min(maxPushDistance, impact)
/// where impactDirection is the difference in positions of the Sword and the collision.
/// </summary>
public class SwordScript : MonoBehaviour
{
    public GameObject player;
    public float baseDamage = 5.0f;
    public float maxDamage = 10.0f;
    public float maxPushDistance = 10.0f;
    
    private Vector3 _previousPosition;

    public void LateUpdate()
    {
        _previousPosition = transform.position;  // keep track of the Sword movements to measure impact
    }

    /// <summary>
    ///   <para> Different impacts depending on type of object (boss, enemy, destroyable).</para>
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        float impact;
        float damage;
        Vector3 impactDirection;
        Vector3 forceDirection;
        switch (collision.collider.tag)
        {
            case "DungeonBoss":
                impact = Vector3.Distance(transform.position, _previousPosition) / Time.deltaTime;
                damage = Mathf.Min(impact * baseDamage, maxDamage);
                collision.gameObject.GetComponent<SkullManager>().HealthSystem.Damage((int)damage);
                
                forceDirection = (collision.gameObject.transform.position - player.transform.position).normalized * Mathf.Min(maxPushDistance, impact);
                collision.gameObject.GetComponent<EnemyAI>().Push(forceDirection);
                break;
            case "Enemy":
                impact = Vector3.Distance(transform.position, _previousPosition) / Time.deltaTime;
                damage = Mathf.Min(impact * baseDamage, maxDamage);
                collision.gameObject.GetComponent<SkullManager>().HealthSystem.Damage((int)damage);
                
                // impactDirection = (transform.position - previousPosition).normalized;
                impactDirection = (collision.gameObject.transform.position - player.transform.position).normalized;
                forceDirection = impactDirection * Mathf.Min(maxPushDistance, impact);
                collision.gameObject.GetComponent<EnemyAI>().Push(forceDirection);
                break;
            // case "Destroyable":
            //     collision.gameObject.GetComponent<Destroyable>().DestroyWithFragments();
            //     break;
        }
    }
}
