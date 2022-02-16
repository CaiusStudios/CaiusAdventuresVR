using System;
using UnityEngine;

public class myCursor : OVRCursor
{
    public OVRGazePointer myOVRGazePointer;
    private void Start()
    {
        myOVRGazePointer = OVRGazePointer.instance;
    }

    public override void SetCursorRay(Transform ray)
    {
        throw new System.NotImplementedException();
    }

    public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
    {
        throw new System.NotImplementedException();
    }
}
