using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject player;
    public float damageFactor = 5.0f;
    private float maxDamage = 10.0f;
    private float maxPushDistance = 10.0f;

    private Vector3 previousPosition;

    public void LateUpdate()
    {
        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
            case "DungeonBoss":
                float impact = Vector3.Distance(transform.position, previousPosition) / Time.deltaTime;
                float damage = Mathf.Min(impact * damageFactor, maxDamage);
                collision.gameObject.GetComponent<EnemyHealthDamageController>().TakeDamage((int)damage);
                Vector3 forceDirection = (player.transform.position - collision.gameObject.transform.position).normalized * Mathf.Min(maxPushDistance, impact);
                collision.gameObject.GetComponent<EnemyAIStateMachine>().Push(forceDirection);
                Debug.Log("Enter " + collision.gameObject.tag + " with damage " + damage+ " and force " + forceDirection);
                break;
            case "Destroyable":
                Debug.Log("We hit " + collision.gameObject.name);
                collision.gameObject.GetComponent<Destroyable>().DestroyWithFragments();
                break;
        }
    }
}
