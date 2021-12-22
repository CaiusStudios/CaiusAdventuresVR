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
        float impact;
        float damage;
        Vector3 forceDirection;
        switch (collision.collider.tag)
        {
            case "DungeonBoss":
                impact = Vector3.Distance(transform.position, previousPosition) / Time.deltaTime;
                damage = Mathf.Min(impact * damageFactor, maxDamage);
                collision.gameObject.GetComponent<SkullManager>().HealthSystem.Damage((int)damage);
                forceDirection = (collision.gameObject.transform.position - player.transform.position).normalized * Mathf.Min(maxPushDistance, impact);
                collision.gameObject.GetComponent<EnemyAIPureAttack>().Push(forceDirection);
                break;
            case "Enemy":
                impact = Vector3.Distance(transform.position, previousPosition) / Time.deltaTime;
                damage = Mathf.Min(impact * damageFactor, maxDamage);
                collision.gameObject.GetComponent<SkullManager>().HealthSystem.Damage((int)damage);
                forceDirection = (collision.gameObject.transform.position - player.transform.position).normalized * Mathf.Min(maxPushDistance, impact);
                collision.gameObject.GetComponent<EnemyAIStateMachine>().Push(forceDirection);
                // Debug.Log("Enter " + collision.gameObject.tag + " with damage " + damage+ " and force " + forceDirection);
                break;
            case "Destroyable":
                collision.gameObject.GetComponent<Destroyable>().DestroyWithFragments();
                break;
        }
    }
}
