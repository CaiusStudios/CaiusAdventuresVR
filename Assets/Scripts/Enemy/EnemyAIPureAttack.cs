using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// reference: https://unitycodemonkey.com/video.php?v=db0KWYaWfeM
public class EnemyAIPureAttack : MonoBehaviour
{
    private float _nextShootTime;
    private EnemyCombat _thisEnemyCombat;
    private Quaternion _rotationToTarget;
    
    public float speedOfChase = 8.0f;
    public float attackRange = 10.0f;
    public float fireRate = 0.05f;
    public GameObject thePlayer;
    private Vector3 lookRotationUpwards = Vector3.up;

    private void Awake()
    {
        thePlayer = GameObject.FindWithTag("Player");
        _thisEnemyCombat = gameObject.GetComponent<EnemyCombat>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // move to target
        _rotationToTarget = Quaternion.LookRotation(thePlayer.transform.position - transform.position, lookRotationUpwards);
        transform.rotation = _rotationToTarget;
        transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position,
            speedOfChase*Time.deltaTime);
                
        // Close enough to begin attack 'motion'
        if (Vector3.Distance(transform.position, thePlayer.transform.position) < attackRange)
        {
            Debug.Log("AI attack");
            // and time allowed for next attack
            if (Time.time > _nextShootTime)
            {
                // stop moving
                // ...
                // animation: attack
                // ...
                // Inflict damage
                _thisEnemyCombat.Attack();
                // update time to next attack
                _nextShootTime = Time.time + 1f / fireRate;
            }
        }
    }
}
