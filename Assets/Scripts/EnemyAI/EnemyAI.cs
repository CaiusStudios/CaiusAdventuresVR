using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

/// <summary>
///   <para> This regroups three types of "AI" or how the enemy moves and behave.</para>
///   <para> The enemy either patrols through points, attack directly the Player, or
///   go through a 3-state state machine.</para>.
///   <para> only one of the three "AI" is possible at a time. </para>.
///   <para> Tick one of the three bool IsStateMachine, IsPureAttack, or IsPatrolling.</para>.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // related to State Machine, PureAttack and Patrolling
    private enum State
    {
        Wandering,
        ChaseTarget,
        GoingBackToStart,
    }
    
    // TODO: CHANGE so that only ONE choice is possible.
    [System.Serializable]
    public struct PossibleBehaviours
    {
        public bool StateMachine;
        public bool PureAttack;
        public bool Patrolling;
    }

    public PossibleBehaviours EnemyBehaviour;
    
    private State _state;
    private Vector3 _startingPosition;
    private Vector3 _wanderPosition;
    private float _reachedPositionDistance = 1.0f;  // TODO: make only one such variable? w.r.t. posTolerance below
    private float _nextShootTime = 0.0f;
    private SkullManager _thisSkullCombat;
    private Quaternion _rotationToTarget;
    private Vector3 lookRotationUpwards = Vector3.up;
    
    public float speedOfMovement = 4.0f;
    public float speedOfChase = 8.0f;
    public float chaseRange = 10.0f;
    public float attackRange = 5.0f;
    public float fireRate = 0.05f;
    public GameObject thePlayer;
    public GameObject stateMachineMaxCoordinate;
    public GameObject stateMachineMinCoordinate;
    
    // related to patrolling only
    private float posTolerance = 0.001f;  // tolerence under which gameObject arrived at target
    private int _currentPosId;  // id of the current target from the list of targets
    private Vector3 _initPos;  // initial position of the gameObject

    public List<Vector3> patrolPoints = new List<Vector3>();  // list of target for patrolling
    
    
    
    // -------------------------------------------------------------------
    // Awake, Start, LateUpdate
    // -------------------------------------------------------------------
    /// <summary>
    ///   <para> Initialize state, player (for its position), and manager (for attack) </para>
    /// </summary>
    private void Awake()
    {
        _state = State.Wandering;
        thePlayer = GameObject.FindWithTag("Player");
        _thisSkullCombat = gameObject.GetComponent<SkullManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _wanderPosition = GetWanderingPosition();
        if (EnemyBehaviour.Patrolling)
        {
            InitPatrolPos();
            
        }
    }
    
    // TODO: there must be a better way. 
    void LateUpdate()
    {
        if (EnemyBehaviour.StateMachine)
        {
            AIStateMachine();
        } else if (EnemyBehaviour.PureAttack)
        {
            AIPureAttack();
        } else if (EnemyBehaviour.Patrolling)
        {
            AIPatrolling(patrolPoints, speedOfMovement, posTolerance);
        }

    }

    
    // -------------------------------------------------------------------
    // PURE ATTACK
    // -------------------------------------------------------------------
    private void AIPureAttack()
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
    
    
    // -------------------------------------------------------------------
    // PATROLLING
    // -------------------------------------------------------------------
    private void InitPatrolPos()
    {
        _currentPosId = 0;  // move towards initial target point
        _initPos = transform.position;
        patrolPoints.Add(_initPos);
    }
    
    /// <summary>
    /// Given a list of points, move this gameObect at "moveSpeed" to each point
    /// consecutively, and reach same with tolerance "reachTargetTolerance".
    /// Restart from initial position of gameObject and Loop continuously.
    ///
    /// call with, e.g.: AIPatrolling(targetPoints, speedOfMovement, posTolerance);
    /// </summary>
    /// <param name="pointsToPatrol"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="reachTargetTolerance"></param>
    private void AIPatrolling(List<Vector3> pointsToPatrol, float moveSpeed, float reachTargetTolerance)
    {
        _rotationToTarget = Quaternion.LookRotation(pointsToPatrol[_currentPosId] - transform.position, Vector3.up);
        transform.rotation = _rotationToTarget;
        transform.position = Vector3.MoveTowards(transform.position, 
            pointsToPatrol[_currentPosId], moveSpeed*Time.deltaTime);

        
        if (Vector3.Distance(transform.position, pointsToPatrol[_currentPosId]) < reachTargetTolerance)
        {
            _currentPosId += 1;  // select next target point
            if (_currentPosId == pointsToPatrol.Count)
            {
                _currentPosId = 0;  // restart from initial target point.
            }
        }
    }
    
    
    // -------------------------------------------------------------------
    // STATE MACHINE
    // -------------------------------------------------------------------
    /// <summary>
    ///   <para> Starts and keeps wandering until player is in range. </para>
    ///   <para> When in range, chases and attacks it if within limit. </para>
    ///   <para> Goes back wandering if player not in range anymore. </para>.
    /// </summary>
    private void AIStateMachine()
    {
        switch (_state)
        {
            case State.Wandering:
                LogicWandering();
                break;
            case State.ChaseTarget:
                LogicChaseTarget();
                break;
            case State.GoingBackToStart:
                LogicGoingBackToStart();
                break;
        }
    }
    
    /// <summary>
    /// logic of Wandering state
    /// </summary>
    private void LogicWandering()
    {
        Vector3 lookForward = _wanderPosition - transform.position;
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
    }

    /// <summary>
    /// logic of ChaseTarget state
    /// </summary>
    private void LogicChaseTarget()
    {
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
                // Inflict damage
                _thisSkullCombat.Attack();
                // update time to next attack
                _nextShootTime = Time.time + fireRate;
            }
        }
                
        // stop the chase because we are too far away from target/player
        if (Vector3.Distance(transform.position, thePlayer.transform.position) > chaseRange)
        {
            _state = State.GoingBackToStart;
        }
    }


    /// <summary>
    /// logic of GoingBackToStart state
    /// </summary>
    private void LogicGoingBackToStart()
    {
        _rotationToTarget = Quaternion.LookRotation(_startingPosition - transform.position, lookRotationUpwards);
        transform.rotation = _rotationToTarget;
        transform.position = Vector3.MoveTowards(transform.position, _startingPosition,
            speedOfMovement*Time.deltaTime);
        if (Vector3.Distance(transform.position, _startingPosition) < _reachedPositionDistance)
        {
            _state = State.Wandering;
        }
    }
    
    
    /// <summary>
    ///   <para> It gives this gameObject a new random destination to move to.</para>
    /// </summary>
    private Vector3 GetWanderingPosition()
    {
        float[] limitX = new float[2];
        float[] limitZ = new float[2];

        limitX[0] = stateMachineMinCoordinate.transform.position.x;
        limitX[1] = stateMachineMaxCoordinate.transform.position.x;
        limitZ[0] = stateMachineMinCoordinate.transform.position.z;
        limitZ[1] = stateMachineMaxCoordinate.transform.position.z;
        
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
    
    /// <summary>
    ///   <para> While wandering, this gameObject checks if it should chase its target now.</para>
    /// </summary>
    /// /// <param name="theTarget"></param>
    private void FindTarget(GameObject theTarget)
    {
        if (Vector3.Distance(transform.position, theTarget.transform.position) < chaseRange)
        {
            // Yes Target is within range -> go chase it!
            _state = State.ChaseTarget;
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
