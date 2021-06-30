using UnityEngine;
//
// current: rotate the ChainBall GameObject around rotationPoint
//
public class ChainBallPosition : MonoBehaviour
{
    public float speedOfSwing = 20.0f;
    
    private Transform _rotationPointTransform;
    private Destroyable _ballDestroyableScript;
    private Vector3 _axisRotation;  // = Vector3.right;
    private float zLimitFront = 6.0f;
    private float zLimitBack = 10.0f;
    private float _chainBallTransformZ;

    // Start is called before the first frame update
    void Start()
    {
        _axisRotation = transform.parent.right;
        // Within the children, get the Ball's script and the rotationPoint.
        _ballDestroyableScript = transform.Find("Ball").GetComponent<Destroyable>();
        _rotationPointTransform = transform.Find("rotationPoint").transform;

        // get initial position Z of the ball
        _chainBallTransformZ = transform.localPosition.z;
    }

    // Update is called once per frame
    // TODO: create separate functions outside of Update
    // TODO: manage multiple balls interactions
    void Update()
    {
        // no update if ball is destroyed.
        if (_ballDestroyableScript.isDestroyed) return;
        
        // update current position Z of the ball
        // _chainBallTransformZ = transform.localPosition.z;  
        
        // modify direction of the direction
        if ( transform.localPosition.z > zLimitBack)
        {
            _axisRotation = transform.parent.right;
        } else if (transform.localPosition.z < zLimitFront)
        {
            _axisRotation = transform.parent.right * -1.0f;
        }
        
        // apply rotation
        transform.RotateAround(_rotationPointTransform.position, _axisRotation,
            Time.deltaTime * speedOfSwing);
        
    }
}
