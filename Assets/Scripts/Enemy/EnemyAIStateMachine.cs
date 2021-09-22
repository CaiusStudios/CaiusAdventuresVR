using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// reference: https://unitycodemonkey.com/video.php?v=db0KWYaWfeM
public class EnemyAIStateMachine : MonoBehaviour
{
    private enum State
    {
        Wandering,
        ChaseTarget,
        GoingBackToStart,
    }

    private State _state;
    private Vector3 _startingPosition;
    private Vector3 _wanderPosition;
    private float _reachedPositionDistance = 1.0f;
    private float _nextShootTime = 0.0f;
    private EnemyCombat _thisEnemyCombat;
    private Quaternion _rotationToTarget;

    public float speedOfMovement = 4.0f;
    public float speedOfChase = 8.0f;
    public float startChaseRange = 10.0f;
    public float stopChaseRange = 10.0f;
    public float attackRange = 10.0f;
    public float fireRate = 0.05f;
    public GameObject thePlayer;
    // public GameObject constraintZone;
    public GameObject constraintMaxCoordinate;
    public GameObject constraintMinCoordinate;
    private Vector3 lookRotationUpwards = Vector3.up;

    private void Awake()
    {
        _state = State.Wandering;
        thePlayer = GameObject.FindWithTag("Player");
        _thisEnemyCombat = gameObject.GetComponent<EnemyCombat>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _wanderPosition = GetWanderingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.Wandering:
                Vector3 lookForward = _wanderPosition - transform.position;
                //lookForward.y = 180;
                _rotationToTarget = Quaternion.LookRotation(lookForward, lookRotationUpwards);
                transform.rotation = _rotationToTarget;
                
                transform.position = Vector3.MoveTowards(transform.position, _wanderPosition,
                    speedOfMovement*Time.deltaTime);
        
                if (Vector3.Distance(transform.position, _wanderPosition) < _reachedPositionDistance)
                {
                    // this gameObject has reached its wander position. he needs a new one
                    _wanderPosition = GetWanderingPosition();
                }
                FindTarget(thePlayer);
                break;
            case State.ChaseTarget:
                //Debug.Log("Enemy is chasing us!");
                // move to target
                _rotationToTarget = Quaternion.LookRotation(thePlayer.transform.position - transform.position, lookRotationUpwards);
                transform.rotation = _rotationToTarget;
                transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position,
                    speedOfChase*Time.deltaTime);
                
                // Close enough to begin attack 'motion'
                if (Vector3.Distance(transform.position, thePlayer.transform.position) < attackRange)
                {
                    //Debug.Log("enemy is about to attack...");
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
                        _nextShootTime = Time.time + fireRate;
                    }
                }
                
                // stop the chase because we are too far away from target/player
                if (Vector3.Distance(transform.position, thePlayer.transform.position) > stopChaseRange)
                {
                    _state = State.GoingBackToStart;
                }
                break;
            case State.GoingBackToStart:
                _rotationToTarget = Quaternion.LookRotation(_startingPosition - transform.position, lookRotationUpwards);
                transform.rotation = _rotationToTarget;
                transform.position = Vector3.MoveTowards(transform.position, _startingPosition,
                    speedOfMovement*Time.deltaTime);
                if (Vector3.Distance(transform.position, _startingPosition) < _reachedPositionDistance)
                {
                    _state = State.Wandering;
                }

                break;
            default:
                break;
        }

    }

    private Vector3 GetWanderingPosition()
    {
        float[] limitX = new float[2];
        float[] limitZ = new float[2];

        limitX[0] = constraintMinCoordinate.transform.position.x;
        limitX[1] = constraintMaxCoordinate.transform.position.x;
        limitZ[0] = constraintMinCoordinate.transform.position.z;
        limitZ[1] = constraintMaxCoordinate.transform.position.z;
        
        Vector3 newWanderPosition;  // = _startingPosition + GetRandomDir() * Random.Range(0.0f, 1.0f);
        newWanderPosition.x = Random.Range(limitX[0], limitX[1]);
        newWanderPosition.y = _startingPosition.y;
        newWanderPosition.z = Random.Range(limitZ[0], limitZ[1]);
        
        return newWanderPosition;
    }
    
    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void FindTarget(GameObject TheTarget)
    {
        if (Vector3.Distance(transform.position, TheTarget.transform.position) < startChaseRange)
        {
            // Target is within range!
            _state = State.ChaseTarget;
        }
    }

    public void Push(Vector3 force)
    {
        transform.position += force;
    }
}
