using UnityEngine;
//
// current: rotate the Wings GameObject around rotationPoint
//
public class WingPosition : MonoBehaviour
{
    public float directionOfWing; // -1 == left,  +1 == right
    public float speedOfSwing;  // e.g. 20.0f relatively slow; 100.0f fast, etc
    public Transform rotationPointTransform;

    private Vector3 _axisRotation;
    private float xRotationLimitUp = -0.5f;
    private float xRotationLimitDown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _axisRotation = transform.parent.forward;
        if (directionOfWing < 0)
        {
            _axisRotation *= directionOfWing;  // we have a LEFT wing
        }

    }

    // Update is called once per frame
    void Update()
    {

        rotateWing();
        

    }

    // TODO: adapt wing rotation when the parent is rotating
    private void rotateWing()
    {
        // _rotationToTarget = Quaternion.LookRotation(thePlayer.transform.position - transform.position, lookRotationUpwards);
        // transform.rotation = _rotationToTarget;
        // transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position,
        //     speedOfChase*Time.deltaTime);
        
        // rotate w.r.t. parent
        // Quaternion _rotationWing = Quaternion.LookRotation(transform.parent.position - transform.position,
        //     Vector3.up);
        // transform.rotation = _rotationWing;
        
        // modify direction of the direction
        if ( transform.localPosition.y < xRotationLimitUp)
        {
            _axisRotation = transform.parent.forward * directionOfWing;
        } else if (transform.localPosition.y > xRotationLimitDown)
        {
            _axisRotation = transform.parent.forward * -1.0f * directionOfWing;
        }
        
        // apply rotation
        transform.RotateAround(rotationPointTransform.position, _axisRotation, 
            Time.deltaTime * speedOfSwing);

    } 
}
