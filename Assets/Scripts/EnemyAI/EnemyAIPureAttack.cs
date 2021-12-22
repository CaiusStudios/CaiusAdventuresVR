using UnityEngine;

/// <summary>
///   <para> Going from Wandering to Chasing/Attack back to Wandering.</para>
/// </summary>
/// <footer><a href="https://unitycodemonkey.com/video.php?v=db0KWYaWfeM">Original Source/Idea for this state machine</a></footer>
public class EnemyAIPureAttack : MonoBehaviour
{
    private float _nextShootTime;
    private SkullManager _thisSkullCombat;
    private Quaternion _rotationToTarget;
    
    public float speedOfChase = 8.0f;
    public float attackRange = 10.0f;
    public float fireRate = 0.05f;
    public GameObject thePlayer;
    private Vector3 lookRotationUpwards = Vector3.up;

    private void Awake()
    {
        thePlayer = GameObject.FindWithTag("Player");
        _thisSkullCombat = gameObject.GetComponent<SkullManager>();
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
            // and time allowed for next attack
            if (Time.time > _nextShootTime)
            {
                // Inflict damage
                _thisSkullCombat.Attack();
                // update time to next attack
                _nextShootTime = Time.time + 1f / fireRate;
            }
        }
    }
    
    /// <summary>
    ///   <para> An external impact changes this gameObject's position</para>
    /// </summary>
    /// /// <param name="force"></param>
    public void Push(Vector3 force)
    {
        transform.position += force;
    }
}
