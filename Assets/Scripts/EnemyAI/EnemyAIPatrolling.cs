using System.Collections.Generic;
using UnityEngine;

public class EnemyAIPatrolling : MonoBehaviour
{
    private float posTolerance = 0.001f;  // tolerence under which gameObject arrived at target
    private int _currentPosId;  // id of the current target from the list of targets
    private Vector3 _initPos;  // initial position of the gameObject
    private Quaternion _rotationToTarget;
    
    public List<Vector3> targetPoints = new List<Vector3>();  // list of target for patrolling
    public float speedOfMovement = 10.0f;  // how fast gameObject moves from one point to another
    public bool returnToInitialPosition = true;  // add initial position of gameObject as final target point (== continuous looping)

    // Start is called before the first frame update
    void Start()
    {
        _currentPosId = 0;  // move towards initial target point
        
        if (returnToInitialPosition)
        {
            _initPos = transform.position;
            targetPoints.Add(_initPos);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        PatrolThroughPoints(targetPoints, speedOfMovement, posTolerance);
    }
    
    //
    // Given a list of points, move this gameObect at "moveSpeed" to each point
    // consecutively, and reach same with tolerance "reachTargetTolerance".
    // Restart from initial position of gameObject and Loop continuously.
    //
    private void PatrolThroughPoints(List<Vector3> pointsToPatrol, float moveSpeed, float reachTargetTolerance)
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
}
