using System;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public bool destroyParent;
    public GameObject fragments;
    public bool isDestroyed;  // ChainBallPosition.cs uses it to rotate the chainBall
    
    public float destroyDelay = 5.0f;
    
    //
    // is used by simple boxes, or also the ball of the ghost' chain
    //
    public void DestroyWithFragments()
    {
        isDestroyed = true;
        gameObject.SetActive(false);  //Destroy(gameObject, destroyDelay*Time.deltaTime);
        if (destroyParent)
        {
            // Destroy(transform.parent.gameObject, 0.5f);
            gameObject.SetActive(false);
        }

        if (fragments != null)
        {
            Instantiate(fragments, transform.position, transform.rotation);
        }
    }
}
