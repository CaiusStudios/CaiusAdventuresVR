using System;
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
    }
    
    // TODO: CHANGE so that only ONE choice is possible.
    [Serializable]
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
    private float _attackRange;
    private Quaternion _rotationToTarget;

    public float speedOfMovement = 4.0f;
    public float speedOfChase = 4.0f;
    public float chaseRange = 15.0f;
    public float fireRate = 1f;
    public float stepBackMultiplayer = 1.25f;
    public GameObject thePlayer;
    public GameObject stateMachineMaxCoordinate;
    public GameObject stateMachineMinCoordinate;
    
    // related to patrolling only
    private float _posTolerance = 0.001f;  // tolerence under which gameObject arrived at target
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
        _attackRange = _thisSkullCombat.AttackRange;
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
    
    // TODO: move it in Start() 
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
            AIPatrolling(patrolPoints, speedOfMovement, _posTolerance);
        }

    }

    
    // -------------------------------------------------------------------
    // PURE ATTACK
    // -------------------------------------------------------------------
    private void AIPureAttack()
    {
        // move to target
        _rotationToTarget = Quaternion.LookRotation(thePlayer.transform.position - transform.position, Vector3.up);
        transform.rotation = _rotationToTarget;
        transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position,
            speedOfChase*Time.deltaTime);
                
        // Close enough to begin attack 'motion'
        if (Vector3.Distance(transform.position, thePlayer.transform.position) < _attackRange)
        {
            // and time allowed for next attack
            if (Time.time > _nextShootTime)
            {
                _thisSkullCombat.Attack();
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
        }
    }
    
    /// <summary>
    /// logic of Wandering state
    /// </summary>
    private void LogicWandering()
    {
        Vector3 lookForward = _wanderPosition - transform.position;
        _rotationToTarget = Quaternion.LookRotation(lookForward, Vector3.up);
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
        // initial distances
        float initDistanceToPlayer = Vector3.Distance(transform.position, thePlayer.transform.position);
        Vector3 initDistanceToPlayerVector3 = thePlayer.transform.position - transform.position;
        
        // look at the player and chase him
        if (initDistanceToPlayer > _posTolerance)  // enemy and player are distant
        {
            _rotationToTarget = Quaternion.LookRotation(initDistanceToPlayerVector3, Vector3.up);
        }
        transform.rotation = _rotationToTarget;  // the enemy look at the player
        if (_thisSkullCombat.CanAttack)  // the enemy stay idle if he just attacked (and cannot re-attack)
        {
            transform.position = Vector3.MoveTowards(  // the enemy effectively start chasing the player
                transform.position, 
                thePlayer.transform.position,
                speedOfChase*Time.deltaTime);
        }

        // update distance at each frame and check if close enough to begin attack/strike
        float updatedDistanceToPlayer = Vector3.Distance(transform.position, thePlayer.transform.position);
        if (updatedDistanceToPlayer <= _attackRange && _thisSkullCombat.CanAttack)
        {
            if (Time.time > _nextShootTime)
            {
                Vector3 forceDirection = (transform.position - thePlayer.transform.position).normalized;
                _thisSkullCombat.Attack(forceDirection, 1f/fireRate, stepBackMultiplayer);
                _nextShootTime = Time.time + 0.25f;
            }
        }
        
        // stop the chase because we are too far away from target/player
        if (Vector3.Distance(transform.position, thePlayer.transform.position) > chaseRange)
        {
            _state = State.Wandering;
            gameObject.GetComponent<Enemy>().HealthSystem.BarOnOff();  // switch bar off
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
    
    /// <summary>
    ///   <para> While wandering, this gameObject checks if it should chase its target now.</para>
    /// </summary>
    /// /// <param name="theTarget"></param>
    private void FindTarget(GameObject theTarget)
    {
        if (Vector3.Distance(transform.position, theTarget.transform.position) < chaseRange)
        {
            _state = State.ChaseTarget;
            gameObject.GetComponent<Enemy>().HealthSystem.BarOnOff();  // switch bar on
        }
    }
    /// <summary>
    ///   <para> An external impact changes this gameObject's position</para>
    ///   <para> (for non-convex enemy, change the transform.position; do not .AddForce because no RigidBody)</para>
    /// </summary>
    /// /// <param name="force"></param>
    public void Push(Vector3 force)
    {
        transform.position += force;
    }
}
